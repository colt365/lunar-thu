using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;

using SmartMe.Core;
using SmartMe.Core.Pipeline;
using SmartMe.Core.Data;
using SmartMe.Core.Record;
using SmartMe.Web;
using SmartMe.Web.Search;
using SmartMe.Windows.Externel;
using System.Windows.Threading;


namespace SmartMe.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        #region fields
        private List<string> _queryHistories = new List<string>();
        public List<string> QueryHistories
        {
            get { return _queryHistories; }
            set { _queryHistories = value; }
        }
		private string _bindingString;
        public string BindingString
        {
            get { return _bindingString; }
            set { _bindingString = value; }
        }

        WebResourceManager _webResourceManager = null;
        InputQueryRecordManager _inputQueryRecordManager = null;
        QueryResultRecordManager _queryResultRecordManager = null;
        Pipeline _pipeline = null;
        QueryResultHandler _resultHandler = null;

        // For DelayedQuery Method
        private DateTime _lastInputTime = DateTime.Now;
        private Object _lastInputTimeLock = new Object();
		private string _lastQueryText = "";

        private TimeSpan _defaultInputQueryObsoletedTime = new TimeSpan(2, 0, 0, 0);

        public TimeSpan InputQueryObsoletedTime
        {
            get;
            set;
        }

        // History Window
        private HistoryWindow _historyWindow = null;
        
		// NotifyIcon
        private System.Windows.Forms.NotifyIcon _notifyIcon = null;

        protected DisplayResultHandler _displayResultHandler = null;
        public DisplayResultHandler DisplayResultHandler
        {
            get { return _displayResultHandler; }
        }

        #endregion

        #region construction

        public MainWindow(System.Windows.Forms.NotifyIcon notifyIcon)
		{
			this.InitializeComponent();
            this._notifyIcon = notifyIcon;
            this._displayResultHandler = new DisplayResultHandler(ShowDetailedGrid);

			// Insert code required on object creation below this point.
            InitializePipeline();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_historyWindow != null)
            {
                _historyWindow.Close();
            }

            Properties.Settings setting = App.Current.Resources["SettingsDataSource"] as Properties.Settings;
            if (setting != null)
            {
                setting.Save();
            }
        }

        private void InitializePipeline()
        {
            _pipeline = new Pipeline();
            _resultHandler = new QueryResultHandler(this);

            _webResourceManager = new WebResourceManager(_pipeline, _resultHandler);
            _pipeline.InputTextSubscriberManager.AddSubscriber(_webResourceManager);

            _webResourceManager.AddSearchEngine(new GoogleSearchEngine());
            _webResourceManager.AddSearchEngine(new BaiduSearchEngine());
            _webResourceManager.AddSearchEngine(new SogouSearchEngine());
            _webResourceManager.AddSearchEngine(new WikipediaSearchEngine());
            _webResourceManager.AddSearchEngine(new DictCn());
            // _webResourceManager.AddSearchEngine(new GoogleSuggestion());

            InputQueryObsoletedTime = _defaultInputQueryObsoletedTime;
            //_inputQueryRecordManager = new InputQueryRecordManager(
            //    "data\\query.xml", InputQueryObsoletedTime);
            _pipeline.InputTextSubscriberManager.AddSubscriber(_inputQueryRecordManager);

            _queryResultRecordManager = new QueryResultRecordManager(
                    "data", new TimeSpan(30, 0, 0, 0));
            _pipeline.QueryResultSubscriberManager.AddSubscriber(_queryResultRecordManager);

        }
        #endregion

        #region DetailedInfoWindow
        private void ShowDetailedGrid(object sender, DisplayResultEventArgs args)
        {
            ShowDetailedGrid(args.Title, args.Description, args.Url);
        }
        private void ShowDetailedGrid(string title, string description, string url)
        {
            DetailedControl.Title = title;
            DetailedControl.Description = description;
            DetailedControl.Link = url;
            DetailedControl.Show();
        }

        private void HideDetailedGrid()
        {
            DetailedControl.Hide();
        }
        #endregion DetailedInfoWindow

        #region Query
        private void AddQueryHistory(string queryText)
        {
            if (this.QueryHistories.Contains(queryText)) 
            {
                RemoveQueryHistory(queryText);
            }
            this.QueryHistories.Insert(0, queryText);
        }
        private void RemoveQueryHistory(string queryText)
        {
            this.QueryHistories.Remove(queryText);
        }
        #endregion Query

        #region Functional
        private void DoDelayedQuery(string text, InputQueryType queryType, int milliSecondsTimedOut)
        {
            DateTime curInputTime = DateTime.Now;
            lock (_lastInputTimeLock)
            {
                if (_lastInputTime < curInputTime)
                {
                    _lastInputTime = curInputTime;
                }
            }
            Thread.Sleep(milliSecondsTimedOut);
            if (_lastInputTime == curInputTime)
            {
                if (text == _lastQueryText)
                {
                    return;
                }
                _DoQuery(text, queryType);
            }
        }
        internal void DoDirectQuery(string text, InputQueryType queryType)
        {
            lock (_lastInputTimeLock)
            {
                _lastInputTime = DateTime.Now;
            }
            _DoQuery(text, queryType);
        }
        private void _DoQuery(string text, InputQueryType queryType)
        {
            if (text == string.Empty)
            {
                return;
            }
            switch (queryType)
            {
                case InputQueryType.Text:
                case InputQueryType.FileName:
                    {
                        InputQuery query = new InputQuery(text);
                        query.QueryType = InputQueryType.Text;

                        _lastQueryText = text;
                        AddQueryHistory(text);

                        this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(
                            delegate()
                            {
                                _pipeline.OnInputTextReady(query);
                            })
                        );

                        break;
                    }
                case InputQueryType.FtpUri:
                case InputQueryType.HttpUri:
                    {
                        Shell shell = new Shell();
                        shell.DoOpenWebBrowser(text);
                        break;
                    }
                default:
                    break;
            }
        }
        #endregion Functional

        #region Mouse Drag and Drop
        public void Window_Drop(object sender, System.Windows.DragEventArgs e)
		{
			// TODO: Add event handler implementation here.
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Window_Drop:" + DateTime.Now.ToUniversalTime());
            sb.AppendLine("sender:" + sender.ToString());
            sb.AppendLine("e.Data.GetType:" + e.Data.GetType());
            sb.AppendLine("e.Data.Formats:" + string.Join("; ",e.Data.GetFormats(true)));
            sb.AppendLine("e.Source.GetType:" + e.Source.GetType());
            sb.AppendLine("e.Source.ToString:" + e.Source.ToString());
            sb.AppendLine("e.OriginalSource.GetType:" + e.OriginalSource.GetType());
            sb.AppendLine("e.OriginalSource.ToString:" + e.OriginalSource.ToString());
            sb.AppendLine("-- convert e.Data : --");
            Debug(sb.ToString());

            Externel.DragArgDispatcher dispatcher = new Externel.DragArgDispatcher();
            string text = string.Empty;
            InputQueryType type = new InputQueryType();
            bool isOK = dispatcher.TryGetQuery(e, ref text, ref type);
            if (isOK)
            {
                InputTextBox.Text = text;
                DoDirectQuery(text, type);
            }

            EnableKeyBoardInput();
		}
		private void Window_PreviewDrop(object sender, System.Windows.DragEventArgs e)
        {
            DisableKeyBoardInput();
        }
        private void Window_PreviewDragLeave(object sender, System.Windows.DragEventArgs e)
        {
            EnableKeyBoardInput();
        }
        private void Window_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
        {
            DisableKeyBoardInput();
        }
		private void Window_PreviewDragEnter(object sender, System.Windows.DragEventArgs e)
        {
            DisableKeyBoardInput();
        }
		private void DisableKeyBoardInput()
		{
            InputTextBox.IsEnabled = false;
		}
        private void EnableKeyBoardInput()
        {
            InputTextBox.IsEnabled = true;
        }
        #endregion Mouse Drag and Drop

        #region Search Bar
		private string GetSearchTextBoxQuery()
		{
			string text = InputTextBox.Text;
            string trimedText = text.Trim(new char[] { ' ', '\t', '\r', '\n' });
			return trimedText;
		}
		
		private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			string query = GetSearchTextBoxQuery();
        	DoDirectQuery(query, InputQueryType.Text);
        }
		
		private void InputTextBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
			string query = GetSearchTextBoxQuery(); 
            if (e.Key == Key.Enter)
            {
                DoDirectQuery(query, InputQueryType.Text);
            }
            else // DoWaitedQuery
            {
                int milliSecondsTimedOut = 500; // 500 毫秒
                ThreadStart threadStart = delegate {
                    this.DoDelayedQuery(query, InputQueryType.Text, milliSecondsTimedOut);
                };
                Thread thread = new Thread(threadStart);
                thread.IsBackground = true;
                thread.Start();
            }
        }
        private void InputTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
            if (InputTextBox.Text == "")
            {
                InputTextBox.Text = "搜索栏";
                InputTextBox.Opacity = 0.8;
            }
		}
		private void InputTextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			if (InputTextBox.Text == "搜索栏")
			{
				InputTextBox.Text = "";
			}
            InputTextBox.SelectAll();
			InputTextBox.Opacity = 1.0;
        }
        #endregion Search Bar


        #region QueryResultHandler

        class QueryResultHandler : IQueryResultHandler
        {
            private QueryResult _currentQueryResult = null;
            public QueryResult CurrentQueryResult
            {
                get { return _currentQueryResult; }
                set { _currentQueryResult = value; }
            }

            private MainWindow _parent;
            Dictionary<string, IQueryResultControl> _uiDict;

            public QueryResultHandler(MainWindow parent)
            {
                _parent = parent;

                _uiDict = new Dictionary<string, IQueryResultControl>();
                _uiDict.Add(SearchEngineType.Google.ToString(), _parent.GoogleResultControl);
                _uiDict.Add(SearchEngineType.Baidu.ToString(),  _parent.BaiduResultControl);
                _uiDict.Add(SearchEngineType.Sogou.ToString(), _parent.SogouResultControl);
                _uiDict.Add(SearchEngineType.Wikipedia.ToString(), _parent.WikipediaResultControl);
                _uiDict.Add(DictType.Dict_cn.ToString(), _parent.DictcnResultControl);

                // Display:
                _parent.GoogleResultControl.DisplayResult += _parent.DisplayResultHandler;
                _parent.BaiduResultControl.DisplayResult += _parent.DisplayResultHandler;
                _parent.SogouResultControl.DisplayResult += _parent.DisplayResultHandler;
                _parent.WikipediaResultControl.DisplayResult += _parent.DisplayResultHandler;
            }

            public void OnResultNew(QueryResult result)
            {
                _parent.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                    delegate()
                    {
                        _parent.SearchingStateGrid.Visibility = Visibility.Visible;
                    })
                );
            }
            public void OnResultUpdate(QueryResult result)
            {
                if (_currentQueryResult != result)
                {
                    _currentQueryResult = result;                        
                }

                _parent.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                    delegate()
                    {
                        UpdateUIResults(result);
                    })
                );
            }

            private void UpdateUIResults(QueryResult result)
            {
                if (result != null && result.Items != null)
                {
                    ClearAllResults();
                    foreach (IQueryResultItem r in result.Items)
                    {
                        if (r != null)
                        {
                            IQueryResultControl control = FindQueryResultControl(r);
                            if (control != null)
                            {
                                control.SetResult(r);
                            }
                        }
                    }
                }
            }

            public void OnResultDeprecated(QueryResult result)
            {
            }
           
            public void OnResultCompleted(QueryResult result)
            {
                if (_currentQueryResult != result)
                {
                    _currentQueryResult = result;
                }

                _parent.Dispatcher.Invoke(DispatcherPriority.Normal, new Action( delegate() 
                    {
                        if (result.IsEmpty())
                        {
                            _parent.InputTextBox.Text = "哎呦，没有结果:(";
                        }
                        else
                        {
                            UpdateUIResults(result);
                        }

                        _parent.SearchingStateGrid.Visibility = Visibility.Hidden;
                    })
                );
            }


            #region private

            private IQueryResultControl FindQueryResultControl(IQueryResultItem resultItem)
            {
                IQueryResultControl control = null;
                string typeKey = null;
                switch(resultItem.ResultType) {
                    case QueryResultItemType.SearchEngineResult:
                        typeKey = (resultItem as SearchEngineResult).SearchEngineType.ToString();
                        break;
                    case QueryResultItemType.DictResult:
                        typeKey = (resultItem as DictResult).DictType.ToString();
                        break;
                    case QueryResultItemType.SuggestionResult:
                        typeKey = (resultItem as SuggestionResult).SuggestionType.ToString();
                        break;
                    default:
                        throw new NotSupportedException("Not Support ResultType:" + resultItem.ResultType);
                }
                
                if (_uiDict.ContainsKey(typeKey)) 
                {
                    control = _uiDict[typeKey];
                }
                return control;
            }

            public void ClearAllResults()
            {
                foreach (IQueryResultControl control in _uiDict.Values)
                {
                    control.ClearResult();
                }
            }
            #endregion private
        }

        #endregion QueryResultHandler

        #region HistoryWindow
        private void ShowHistoryWindowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowHistoryWindow(this.Top, this.Left);
        }

        public void ShowHistoryWindow(double top, double left)
        {
            if (_historyWindow != null && !_historyWindow.IsClosed) 
            {
                // do nothing
                return;
            }

            _historyWindow = new HistoryWindow();
            _historyWindow.Left = left;
            _historyWindow.Top = top;
            _historyWindow.Pipeline = _pipeline;
            _historyWindow.QueryResultHandler = _resultHandler;
            _historyWindow.QueryResultRecordManager = _queryResultRecordManager;
            _historyWindow.ChangeText = new ChangeString(ChangeInputText);
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(
                delegate()
                {
                    _historyWindow.LoadHistoryRecord();
                    _historyWindow.Show();
                })
            );
        }
        #endregion

        #region CallBack

        public void ChangeInputText(string text)
        {
            InputTextBox.Text = text;
        }

        private void SearchImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DoDirectQuery(InputTextBox.Text, InputQueryType.Text);
            }
        }

        private void Window_Loaded ( object sender, RoutedEventArgs e )
        {

        }
        #endregion
    }
}
