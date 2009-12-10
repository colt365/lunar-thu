﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;

using SmartMe.Core;
using SmartMe.Core.Pipeline;
using SmartMe.Core.Data;
using SmartMe.Web;
using SmartMe.Web.Search;


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
        Pipeline _pipeline = null;
        QueryResultHandler _resultHandler = null;

        // For DelayedQuery Method
        private DateTime _lastInputTime = DateTime.Now;
        private Object _lastInputTimeLock = new Object();
		private string _lastQueryText = "";


        // Detailed Window
        private DetailedInfoWindow _detailedInfoWindow = new DetailedInfoWindow();
        #endregion

        public MainWindow()
		{
			this.InitializeComponent();

			// Insert code required on object creation below this point.
            CreateListeners();
		}

        ~MainWindow()
        {
            _detailedInfoWindow.Close();
        }

        private void CreateListeners()
        {
            _pipeline = new Pipeline();
            _resultHandler = new QueryResultHandler(this);

            _webResourceManager = new WebResourceManager(_pipeline, _resultHandler);
            _pipeline.InputTextSubscriberManager.AddSubscriber(_webResourceManager);

            _webResourceManager.AddSearchEngine(new GoogleSearchEngine());
            _webResourceManager.AddSearchEngine(new BaiduSearchEngine());
            _webResourceManager.AddSearchEngine(new SogouSearchEngine());
            _webResourceManager.AddSearchEngine(new WikipediaSearchEngine());
        }

        
        #region Hidden
        private void GrabButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			//ShowScreenWindowAction action = new ShowScreenWindowAction();
            ScreenWindow screenWindow = new ScreenWindow();
            bool? isActived = screenWindow.ShowDialog();
            
            if (screenWindow.IsScreenRegionSelected)
            {
                Point p0 = screenWindow.P0;
                Point p1 = screenWindow.P1;
                this.ResultTextBox.Text = string.Format("SelectionRegion: ({0}, {1}), ({2}, {3})", new object[] { p0.X, p0.Y, p1.X, p1.Y });
            }
            else
            {
                this.ResultTextBox.Text = string.Format("No SelectionRegion");
            }
            screenWindow.Close();
        }
        #endregion Hidden
        #region DetailedInfoWindow
        private Point GetDetailedInfoScreenPosition(MouseEventArgs e)
        {
            Point mouseWindowPosition = e.GetPosition(this);
            double mainWindowLeft = this.Left;
            double mainWindowTop = this.Top;

            Point detailedInfoScreenPostion = new Point();
            detailedInfoScreenPostion.X = mainWindowLeft - _detailedInfoWindow.Width;
            detailedInfoScreenPostion.Y = mainWindowTop + mouseWindowPosition.Y;

            return detailedInfoScreenPostion;
        }

        private void ToggleDetailedInfoWindow(MouseEventArgs e)
        {
            Point p = GetDetailedInfoScreenPosition(e);
            if (_detailedInfoWindow.Visibility == Visibility.Visible)
            {
                HideDetailedInfoWindow();
            }
            else
            {
                ShowDetailedInfoWindow((int)p.X, (int)p.Y);
            }
        }

        private void ShowDetailedInfoWindow(int left, int top)
        {
            _detailedInfoWindow.Left = left;
            _detailedInfoWindow.Top = top;
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(
                delegate()
                {
                    _detailedInfoWindow.Show();
                })
            );
        }

        private void HideDetailedInfoWindow()
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(
                delegate()
                {
                    _detailedInfoWindow.Hide();
                })
            );
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
                DoQuery(text, queryType);
            }
        }
        private void DoQuery(string text, InputQueryType queryType)
        {
            lock (_lastInputTimeLock)
            {
                _lastInputTime = DateTime.Now;
            }
            if (text == string.Empty)
            {
                return;
            }
			if (text == _lastQueryText)
			{
				return;
			}

            switch (queryType)
            {
                case InputQueryType.Text:
                {
                    InputQuery query = new InputQuery(text);
                    query.QueryType = InputQueryType.Text;
					
					_lastQueryText = text;
                    AddQueryHistory(text);

                    this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(
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
                    DoOpenWebBrowser(text);
                    break;
                }
                case InputQueryType.FileName:
                {
                    break;
                }
            }
        }
        private void DoShellCall(object o)
        {
            System.Diagnostics.ProcessStartInfo info = (System.Diagnostics.ProcessStartInfo)o;
            System.Diagnostics.Process process = null;
            try
            {
                process = System.Diagnostics.Process.Start(info);
            }
            catch (ArgumentNullException e)
            {
                return;
            }
            catch (Win32Exception e)
            {
                return;
            }
            catch (ObjectDisposedException e)
            {
                return;
            }
            catch (InvalidOperationException e)
            {
                return;
            }
            catch (Exception e)
            {
                return;
            }
        }
        private void DoOpenWebBrowser(string defaultPage)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            UriBuilder uriBuilder = new UriBuilder(defaultPage);
            info.UseShellExecute = true;
            info.FileName = uriBuilder.Uri.ToString();
            info.Arguments = "";
            info.WorkingDirectory = ".";

            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(
                    delegate()
                    {
                        this.DoShellCall(info);
                    })
            );
        }
        #endregion Functional

        #region 鼠标拖拽
        private void Window_Drop(object sender, System.Windows.DragEventArgs e)
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
            ResultTextBox.Text = sb.ToString();
			sb = new StringBuilder();

            if (e.Data.GetDataPresent("Text", true))
            {
                sb.AppendLine("Text:" + e.Data.GetData("Text", true));
                string text = e.Data.GetData("Text", true).ToString();
                InputTextBox.Text = text;
                DoQuery(InputTextBox.Text, InputQueryType.Text);
				InputTextBox.IsEnabled = true;
            }
			if (e.Data.GetDataPresent("text/html", true))
            {
                sb.AppendLine("text/html:" + e.Data.GetData("text/html", true));
            }
            if (e.Data.GetDataPresent("text/x-moz-url", true))
            {
                sb.AppendLine("text/x-moz-url:" + e.Data.GetData("text/x-moz-url", true));
            }
            if (e.Data.GetDataPresent("text/html", true))
            {
                sb.AppendLine("text/html:" + e.Data.GetData("text/html", true));
            }
            if (e.Data.GetDataPresent("HTML Format", true))
            {
                sb.AppendLine("HTML Format:" + e.Data.GetData("HTML Format", true));
            }
            if (e.Data.GetDataPresent("UniformText", true))
            {
                sb.AppendLine("UniformText:" + e.Data.GetData("UniformText", true));
            }
			if (e.Data.GetDataPresent("FileName", true))
            {
                sb.AppendLine("FileName:" + e.Data.GetData("FileName", true));
            }
			if (e.Data.GetDataPresent("FileNameW", true))
            {
                sb.AppendLine("FileNameW:" + e.Data.GetData("FileNameW", true));
            }
            if (e.Data.GetDataPresent("FileName", true))
            {
                sb.AppendLine("FileName:" + string.Join("; ", (String[])e.Data.GetData("FileName", true)));
            }
            if (e.Data.GetDataPresent("FileNameW", true))
            {
                sb.AppendLine("FileNameW:" + string.Join("; ", (String[])e.Data.GetData("FileNameW", true)));
            }
            if (e.Data.GetDataPresent("UniformResourceLocator", true))
            {
                sb.AppendLine("UniformResourceLocator:" + e.Data.GetData("UniformResourceLocator", true));
            }
            if (e.Data.GetDataPresent("UniformResourceLocatorW", true))
            {
                sb.AppendLine("UniformResourceLocatorW:" + e.Data.GetData("UniformResourceLocatorW", true));
            }
			
            
            ResultTextBox.Text += sb.ToString();
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
            ResultTextBox.IsEnabled = false;
            InputTextBox.IsEnabled = false;
		}
        private void EnableKeyBoardInput()
        {
            ResultTextBox.IsEnabled = true;
            InputTextBox.IsEnabled = true;
        }
        #endregion 鼠标拖拽

        #region 搜索栏
		private void InputTextBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
        	string text = InputTextBox.Text;
            string trimedText = text.Trim(new char[] { ' ', '\t', '\r', '\n' });

            if (e.Key == Key.Return)
            {
                DoQuery(trimedText, InputQueryType.Text);
            }
            else // DoWaitedQuery
            {
                int milliSecondsTimedOut = 500; // 500 毫秒
                ThreadStart threadStart = delegate {
                    this.DoDelayedQuery(trimedText, InputQueryType.Text, milliSecondsTimedOut);
                };
                Thread thread = new Thread(threadStart);
                thread.Start();
            }
        }
        private void InputTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			if (InputTextBox.Text == "")
			{
				InputTextBox.Text = "搜索栏";
				InputTextBox.Opacity = 0.5;
			}
		}
		private void InputTextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			if (InputTextBox.Text == "搜索栏")
			{
				InputTextBox.Text = "";
			}
			InputTextBox.Opacity = 1.0;
        }
        #endregion 搜索栏

        #region 打开 Detailed Window
		private void GoogleOutputListBox_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	int index = GoogleOutputListBox.SelectedIndex;
            SearchEngineResult result = _resultHandler.GetSearchEngineResult(sender);
            if (result != null)
            {
                if (0 <= index && index < result.Results.Count)
                {
                    string title = string.Format("{0}", result.Results[index].Title);
                    string uri = string.Format("{0}", result.Results[index].Url);
                    string description = string.Format("{0}", result.Results[index].Description);
                    string cachedUri = string.Format("{0}", result.Results[index].CacheUrl);
                    string similarUri = string.Format("{0}", result.Results[index].SimilarUrl);
                    _detailedInfoWindow.TitleTextBlock.Text = title;
                    _detailedInfoWindow.DescriptionTextBlock.Text = description;
                    ToggleDetailedInfoWindow(e);
                }
            }
        }
        #endregion

        #region 结果栏
        private void GoogleOutputListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int index = GoogleOutputListBox.SelectedIndex;
                SearchEngineResult result = _resultHandler.GetSearchEngineResult(sender);
                if (result != null)
                {
                    if (0 <= index && index < result.Results.Count)
                    {
                        string uri = string.Format("{0}", result.Results[index].Url);
                        DoOpenWebBrowser(uri);
                    }
                }
            }
        }
		
        private void BaiduOutputListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int index = BaiduOutputListBox.SelectedIndex;
                SearchEngineResult result = _resultHandler.GetSearchEngineResult(sender);
                if (result != null)
                {
                    if (0 <= index && index < result.Results.Count)
                    {
                        string uri = string.Format("{0}", result.Results[index].Url);
                        DoOpenWebBrowser(uri);
                    }
                }
            }
        }
		private void SougouOutputListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int index = SougouOutputListBox.SelectedIndex;
                SearchEngineResult result = _resultHandler.GetSearchEngineResult(sender);
                if (result != null)
                {
                    if (0 <= index && index < result.Results.Count)
                    {
                        string uri = string.Format("{0}", result.Results[index].Url);
                        DoOpenWebBrowser(uri);
                    }
                }
            }
        }

        private void WikipediaOutputListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int index = WikipediaOutputListBox.SelectedIndex;
                SearchEngineResult result = _resultHandler.GetSearchEngineResult(sender);
                if (result != null)
                {
                    if (0 <= index && index < result.Results.Count)
                    {
                        string uri = string.Format("{0}", result.Results[index].Url);
                        DoOpenWebBrowser(uri);
                    }
                }
            }
        }   
        #endregion 结果栏

        #region QueryResultHandler
        class QueryResultHandler : IQueryResultHandler
        {
            private QueryResult _currentQueryResult = null;
            public SmartMe.Core.Data.QueryResult CurrentQueryResult
            {
                get { return _currentQueryResult; }
                set { _currentQueryResult = value; }
            }

            private MainWindow _parent;

            public QueryResultHandler(MainWindow parent)
            {
                _parent = parent;
            }

            public void OnResultNew(QueryResult result)
            {
                //_currentQueryResult = result;
                //_parent.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                //    delegate()
                //    {
                //    })
                //);
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
                        if (result != null && result.Items != null)
                        {
                            foreach (SearchEngineResult searchEngineItem in result.Items)
                            {
                                if (searchEngineItem != null && searchEngineItem.Results != null) // TODO: Bug ASSERT(searchEngineItem != null)
                                {
                                    ListBox listBox = null; 
                                    TabItem tabItem = null;
                                    string engineName = null;
                                    bool hasFound = FindUIElements(searchEngineItem, out listBox, out tabItem, out engineName);
                                    
                                    if (hasFound)
                                    {
                                        listBox.Items.Clear();
                                        foreach (SearchEngineResult.ResultItem resultItem in searchEngineItem.Results)
                                        {
                                            listBox.Items.Add(new ListBoxItem() { Content = resultItem.Title });
                                        }
                                        tabItem.Header = string.Format("{0}({1})", engineName, searchEngineItem.Results.Count);
                                        
                                        listBox.InvalidateArrange();
                                        tabItem.InvalidateArrange();
                                    }
                                }
                            }
                        }
                    })
                );
            }
            public void OnResultDeprecated(QueryResult result)
            {
                //if (_currentQueryResult == result)
                //{
                //    _parent.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                //        delegate()
                //        {
                //            _parent.GoogleOutputListBox.Items.Clear();
                //            _parent.BaiduOutputListBox.Items.Clear();
                //        })
                //    );
                //    _currentQueryResult = null;
                //}

                //MessageBox.Show("OnResultDeprecated" + result.ToString());
            }
            public void OnResultCompleted(QueryResult result)
            {
                //if (_currentQueryResult != result)
                //{
                //    _currentQueryResult = result;
                //}
            }

            public SearchEngineResult GetSearchEngineResult(object sender)
            {
                SearchEngineResult searchEngineResult = null;
                if (sender != null)
                {
                    bool hasFound = FindSearchEngineResult(sender, out searchEngineResult);
                }
                return searchEngineResult;
            }

            #region private
            private bool FindSearchEngineResult(object sender, out SearchEngineResult searchEngineResult)
            {
                bool hasFound = false;
                SearchEngineResult result = null;
                if (sender is ListBox)
                {
                    ListBox sourceListBox = sender as ListBox;
                    if (sourceListBox == _parent.GoogleOutputListBox)
                    {
                        hasFound = FindSearchEngineResult(_currentQueryResult, SearchEngineType.Google, out result);
                    }
                    else if (sourceListBox == _parent.BaiduOutputListBox)
                    {
                        hasFound = FindSearchEngineResult(_currentQueryResult, SearchEngineType.Baidu, out result);
                    }
                    else if (sourceListBox == _parent.SougouOutputListBox)
                    {
                        hasFound = FindSearchEngineResult(_currentQueryResult, SearchEngineType.Sougou, out result);
                    }
                    else if (sourceListBox == _parent.WikipediaOutputListBox)
                    {
                        hasFound = FindSearchEngineResult(_currentQueryResult, SearchEngineType.Wikipedia, out result);
                    }
                }
                searchEngineResult = result;
                return hasFound;
            }
            private bool FindSearchEngineResult(QueryResult queryResult, SearchEngineType targetType, out SearchEngineResult searchEngineResult)
            {
                bool hasFound = false;
                searchEngineResult = null;
                if (queryResult != null)
                {
                    if (queryResult.Items != null)
                    {
                        foreach (SearchEngineResult resultItem in queryResult.Items)
                        {
                            if (resultItem != null && resultItem.SearchEngineType == targetType)
                            {
                                searchEngineResult = resultItem;
                                hasFound = true;
                                break;
                            }
                        }
                    }
                }
                return hasFound;
            }
            private bool FindUIElements(SearchEngineResult searchEngineResult, out ListBox listBox, out TabItem tabItem, out string engineName)
            {
                bool hasFound = false;
                switch (searchEngineResult.SearchEngineType)
                {
                    case SearchEngineType.Google:
                        {
                            hasFound = true;
                            listBox = _parent.GoogleOutputListBox;
                            tabItem = _parent.GoogleTabItem;
                            engineName = "谷歌";
                            break;
                        }
                    case SearchEngineType.Baidu:
                        {
                            hasFound = true;
                            listBox = _parent.BaiduOutputListBox;
                            tabItem = _parent.BaiduTabItem;
                            engineName = "百度";
                            break;
                        }
					case SearchEngineType.Sougou:
                        {
                            hasFound = true;
                            listBox = _parent.SougouOutputListBox;
                            tabItem = _parent.SougouTabItem;
                            engineName = "搜狗";
                            break;
                        }
                    case SearchEngineType.Wikipedia:
                        {
                            hasFound = true;
                            listBox = _parent.WikipediaOutputListBox;
                            tabItem = _parent.WikipediaTabItem;
                            engineName = "维基";
                            break;
                        }
                    default:
                        {
                            hasFound = false;
                            listBox = null;
                            tabItem = null;
                            engineName = "Unknown";
                            break;
                        }
                }
                return hasFound;
            }
            #endregion private
        }
        #endregion QueryResultHandler

        #region for Debug
        public enum Level
        {
            Normal,
            Warning,
            Error,
            Fatal
        }

        public void MessageDebug(object o)
        {
            MessageDebug(o, Level.Normal);
        }

        public void MessageDebug(object o, Level level)
        {
            string str = o.ToString();

            // TODO: unfinished 
            //  TT 09/12/5 
        }

        #endregion for Debug
	}
}