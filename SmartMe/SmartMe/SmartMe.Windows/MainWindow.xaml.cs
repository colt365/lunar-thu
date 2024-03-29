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

        // Detailed Window
        private DetailedInfoWindow _detailedInfoWindow = null;
        private string _selectedItemUri = string.Empty;

        // History Window
        private HistoryWindow _historyWindow = null;
        
		// NotifyIcon
        private System.Windows.Forms.NotifyIcon notifyIcon;

        #endregion

        #region construction

        public MainWindow(System.Windows.Forms.NotifyIcon notifyIcon)
		{
			this.InitializeComponent();
            this.notifyIcon = notifyIcon;
         
			// Insert code required on object creation below this point.
            CreateListeners();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_detailedInfoWindow != null)
            {
                _detailedInfoWindow.Close();
            }
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
            //_webResourceManager.AddSearchEngine( new GoogleSuggestion() );
            _webResourceManager.AddSearchEngine( new DictCn() );

            InputQueryObsoletedTime = _defaultInputQueryObsoletedTime;
            //_inputQueryRecordManager = new InputQueryRecordManager(
            //    "data\\query.xml", InputQueryObsoletedTime);
            _pipeline.InputTextSubscriberManager.AddSubscriber(_inputQueryRecordManager);

            _queryResultRecordManager =
                new QueryResultRecordManager(
                    "data", new TimeSpan(30, 0, 0, 0));
            _pipeline.QueryResultSubscriberManager.AddSubscriber(_queryResultRecordManager);            

        }
        /*
                private void InitNotifyIcon(){
                    trayContextMenu = new System.Windows.Forms.ContextMenu();
                    trayExitMenuItem = new System.Windows.Forms.MenuItem();
                    trayViewHistorymenuItem = new System.Windows.Forms.MenuItem();

                    trayContextMenu.MenuItems.AddRange(
                                new System.Windows.Forms.MenuItem[] { trayViewHistorymenuItem, trayExitMenuItem });

                    trayViewHistorymenuItem.Index = 0;
                    trayViewHistorymenuItem.Text = "查看历史记录(&H)";
                    trayViewHistorymenuItem.Click += new EventHandler(TrayViewHistoryMenuItem_Click);

                    trayExitMenuItem.Index = 1;
                    trayExitMenuItem.Text = "退出(&E)";
                    trayExitMenuItem.Click += new EventHandler(TrayExitMenuItem_Click);

                    notifyIcon = new System.Windows.Forms.NotifyIcon();
                    notifyIcon.BalloonTipText = "SmartMe已最小化到托盘，双击此处恢复窗口";
                    notifyIcon.BalloonTipTitle = "SmartMe";
                    notifyIcon.Text = "SmartMe";
                    notifyIcon.Icon = new System.Drawing.Icon("icon.ico");
                    notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
                    notifyIcon.ContextMenu = this.trayContextMenu;
                    ShowTrayIcon(true);	// Always show the icon
                }
                */

        #endregion

        

        #region DetailedInfoWindow
        private void ShowDetailedGrid(string title, string description, string url)
        {
            //Title
            DetailedTitleTextBox.Text = title;

            //Description
            DetailedDescriptionRichTextBox.Document.Blocks.Clear();
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(description);
            DetailedDescriptionRichTextBox.Document.Blocks.Add(paragraph);

            //Url
            _selectedItemUri = url;

            //Show Detail Grid
            DetailedGrid.Visibility = Visibility.Visible;
        }

        private void HideDetailedGrid()
        {
            DetailedGrid.Visibility = Visibility.Hidden;
            _selectedItemUri = string.Empty;
        }

        #endregion DetailedInfoWindow

        #region open Detailed Window
        private void GoogleOutputListBox_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            int count = GoogleOutputListBox.Items.Count;
            int selectedIndex = GoogleOutputListBox.SelectedIndex;
            if (0 <= selectedIndex && selectedIndex < count)
            {
                if (e.Delta < 0)
                {
                    selectedIndex = Math.Min(selectedIndex + 1, count);
                }
                else
                {
                    selectedIndex = Math.Max(selectedIndex - 1, 0);
                }
                GoogleOutputListBox.SelectedIndex = selectedIndex;
            }
        }
        private void BaiduOutputListBox_PreviewMouseWheel ( object sender, System.Windows.Input.MouseWheelEventArgs e )
        {
            int count = BaiduOutputListBox.Items.Count;
            int selectedIndex = BaiduOutputListBox.SelectedIndex;
            if ( 0 <= selectedIndex && selectedIndex < count )
            {
                if ( e.Delta < 0 )
                {
                    selectedIndex = Math.Min( selectedIndex + 1, count );
                }
                else
                {
                    selectedIndex = Math.Max( selectedIndex - 1, 0 );
                }
                BaiduOutputListBox.SelectedIndex = selectedIndex;
            }
        }
        private void SougouOutputListBox_PreviewMouseWheel ( object sender, System.Windows.Input.MouseWheelEventArgs e )
        {
            int count = SougouOutputListBox.Items.Count;
            int selectedIndex = SougouOutputListBox.SelectedIndex;
            if ( 0 <= selectedIndex && selectedIndex < count )
            {
                if ( e.Delta < 0 )
                {
                    selectedIndex = Math.Min( selectedIndex + 1, count );
                }
                else
                {
                    selectedIndex = Math.Max( selectedIndex - 1, 0 );
                }
                SougouOutputListBox.SelectedIndex = selectedIndex;
            }
        }
        private void WikipediaOutputListBox_PreviewMouseWheel ( object sender, System.Windows.Input.MouseWheelEventArgs e )
        {
            int count = WikipediaOutputListBox.Items.Count;
            int selectedIndex = WikipediaOutputListBox.SelectedIndex;
            if ( 0 <= selectedIndex && selectedIndex < count )
            {
                if ( e.Delta < 0 )
                {
                    selectedIndex = Math.Min( selectedIndex + 1, count );
                }
                else
                {
                    selectedIndex = Math.Max( selectedIndex - 1, 0 );
                }
                WikipediaOutputListBox.SelectedIndex = selectedIndex;
            }
        }
        /*
        private void GoogleOutputListBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int index = GoogleOutputListBox.SelectedIndex;
            if (index >= 0)
            {
                MessageBox.Show("index:" + index);
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
                        Point p = GetDetailedInfoScreenPosition(e);
                        ShowDetailedInfoWindow((int)p.X, (int)p.Y);
                    }
                }
            }
            else
            {
                HideDetailedInfoWindow();
            }
        }
        */

        private void GoogleOutputListBox_SelectionChanged(object sender, 
                                        System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = GoogleOutputListBox.SelectedIndex;
            DispalySearchEngineResultDetailedGrid(sender, index);
        }


        private void BaiduOutputListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = BaiduOutputListBox.SelectedIndex;
            DispalySearchEngineResultDetailedGrid(sender, index);
        }

        private void SougouOutputListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = SougouOutputListBox.SelectedIndex;
            DispalySearchEngineResultDetailedGrid(sender, index);
        }

        private void WikipediaOutputListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = WikipediaOutputListBox.SelectedIndex;
            DispalySearchEngineResultDetailedGrid(sender, index);
        }  

        private void DispalySearchEngineResultDetailedGrid(object resultListBox, int index)
        {
            if (index >= 0)
            {
                SearchEngineResult result = _resultHandler.GetSearchEngineResult(resultListBox);
                if (result != null)
                {
                    if (0 <= index && index < result.Results.Count)
                    {
                        string title = string.Format("{0}", result.Results[index].Title);
                        string uri = string.Format("{0}", result.Results[index].Url);
                        string description = string.Format("{0}", result.Results[index].Description);
                        ShowDetailedGrid(title, description, uri);
                    }
                }
            }
            else
            {
                HideDetailedGrid();
            }
        }

        private void ShowDetailWindow_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            if (GoogleTabItem.IsSelected)
            {
                index = GoogleOutputListBox.SelectedIndex;
                DispalySearchEngineResultDetailedGrid(GoogleOutputListBox, index);
            }
            if (BaiduTabItem.IsSelected)
            {
                index = GoogleOutputListBox.SelectedIndex;
                DispalySearchEngineResultDetailedGrid(BaiduOutputListBox, index);
            }
            if (SougouTabItem.IsSelected)
            {
                index = GoogleOutputListBox.SelectedIndex;
                DispalySearchEngineResultDetailedGrid(SougouOutputListBox, index);
            }
            if (WikipediaTabItem.IsSelected)
            {
                index = GoogleOutputListBox.SelectedIndex;
                DispalySearchEngineResultDetailedGrid(WikipediaOutputListBox, index);
            }
        }

        //private void GoogleOutputListBox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    HideDetailedInfoWindow();
        //}
        /*
        private void GoogleOutputListBox_PreviewMouseRightButtonDown(object sender, 
                                            System.Windows.Input.MouseButtonEventArgs e)
        {
            int index = GoogleOutputListBox.SelectedIndex;
            if (index >= 0)
            {
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
                        // ToggleDetailedInfoWindow(e);
                    }
                }
            }
        }
        */

        #endregion

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
            ResultTextBox.Text = sb.ToString();
			sb = new StringBuilder();

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

        #region Result Control
        private void GoogleOutputListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int index = GoogleOutputListBox.SelectedIndex;
                OpenSearchEngineResult(sender, index);
            }
        }
		
        private void BaiduOutputListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int index = BaiduOutputListBox.SelectedIndex;
                OpenSearchEngineResult(sender, index);
            }
        }
		private void SougouOutputListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int index = SougouOutputListBox.SelectedIndex;
                OpenSearchEngineResult(sender, index);
            }
        }

        private void WikipediaOutputListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int index = WikipediaOutputListBox.SelectedIndex;
                OpenSearchEngineResult(sender, index);
            }
        }

        private void OpenSearchEngineResult(object sender, int index)
        {
            SearchEngineResult result = _resultHandler.GetSearchEngineResult(sender);
            if (result != null)
            {
                if (0 <= index && index < result.Results.Count)
                {
                    string uri = string.Format("{0}", result.Results[index].Url);
                    Shell shell = new Shell();
                    shell.DoOpenWebBrowser(uri);
                }
                else if (index == -1)
                {
                    string uri = string.Format("{0}", result.SearchUrl);
                    Shell shell = new Shell();
                    shell.DoOpenWebBrowser(uri);
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
                        if (result != null && result.Items != null)
                        {
                            try
                            {
                                _parent.ClearAllListBoxes();
                                foreach (IQueryResultItem queryResultItem in result.Items)
                                {
                                    #region dealwithqueryitem
                                    switch (queryResultItem.ResultType)
                                    {
                                        case QueryResultItemType.SearchEngineResult:
                                            var searchEngineItem = queryResultItem as SearchEngineResult;
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
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = resultItem.Title
                                                        });
                                                    }
                                                    tabItem.Header = string.Format("{0}({1})", engineName, searchEngineItem.Results.Count);

                                                    listBox.InvalidateArrange();
                                                    tabItem.InvalidateArrange();
                                                }
                                            }

                                            break;

                                        case QueryResultItemType.SuggestionResult:
                                            //SuggestionResult suggestionItem= queryResultItem as SuggestionResult;
                                            //_parent._suggestionWindow.Show( suggestionItem );


                                            break;

                                        case QueryResultItemType.DictionaryResult:
                                            var dictItem = queryResultItem as DictResult;
                                            if (dictItem != null && dictItem.Word != null) // TODO: Bug ASSERT(searchEngineItem != null)
                                            {

                                                ListBox listBox = null;
                                                TabItem tabItem = null;
                                                string dictname = null;
                                                bool hasFound = FindUIElements(dictItem, out listBox, out tabItem, out dictname);

                                                if (hasFound && dictItem.Word != string.Empty)
                                                {
                                                    listBox.Items.Clear();
                                                    /*foreach ( SearchEngineResult.ResultItem resultItem in searchEngineItem.Results )
                                                    {
                                                        listBox.Items.Add( new ListBoxItem()
                                                        {
                                                            Content = resultItem.Title
                                                        } );
                                                    }*/
                                                    listBox.Items.Add(new ListBoxItem()
                                                    {
                                                        Content = dictItem.Word

                                                    });
                                                    if (dictItem.Pronunciation != string.Empty)
                                                    {
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = " "
                                                        });
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = dictItem.Pronunciation
                                                        });
                                                    }
                                                    if (dictItem.Variations != string.Empty)
                                                    {
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = " "
                                                        });
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = dictItem.Variations
                                                        });
                                                    }
                                                    if (dictItem.EnglishExplanations != string.Empty)
                                                    {
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = " "
                                                        });
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = dictItem.EnglishExplanations
                                                        });
                                                    }
                                                    if (dictItem.ChineseExplanations != string.Empty)
                                                    {
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = " "
                                                        });
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = dictItem.ChineseExplanations
                                                        });
                                                    }
                                                    if (dictItem.Examples != string.Empty)
                                                    {
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = " "
                                                        });
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = dictItem.Examples
                                                        });
                                                    }
                                                    if (dictItem.FromEncyclopedia != string.Empty)
                                                    {
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = " "
                                                        });
                                                        listBox.Items.Add(new ListBoxItem()
                                                        {
                                                            Content = dictItem.FromEncyclopedia
                                                        });
                                                    }


                                                    tabItem.Header = string.Format("{0}({1})", dictname, 1);

                                                    listBox.InvalidateArrange();
                                                    tabItem.InvalidateArrange();
                                                }
                                                else
                                                {
                                                    tabItem.Header = string.Format("{0}({1})", dictname, 0);

                                                    listBox.InvalidateArrange();
                                                    tabItem.InvalidateArrange();
                                                }
                                            }


                                            break;

                                        default:
                                            break;
                                    }
                                    #endregion
                                }
                            }
                            catch (Exception)
                            {
                                // do nothing
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
                _parent.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                        delegate()
                        {
                            if (result.IsEmpty())
                            {
                                _parent.InputTextBox.Text = "哎呦，没有结果:(";
                            }
                            
                            _parent.SearchingStateGrid.Visibility = Visibility.Hidden;

                        })
                    );
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
            public DictResult GetDictcnResult ( object sender )
            {
                DictResult dictResult = null;
                if ( sender != null )
                {
                    bool hasFound = FindDictcnResult( sender, out dictResult );
                }
                return dictResult;
            }

            #region private

            private bool FindDictcnResult ( object sender, out DictResult dictResult )
            {
                bool hasFound = false;
                DictResult result = null;
                if ( sender is ListBox )
                {
                    ListBox sourceListBox = sender as ListBox;
                    hasFound = FindDictcnResult( _currentQueryResult, DictionaryType.Dict_cn, out result );
                }
                dictResult = result;
                return hasFound;
            }

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

            private bool FindDictcnResult ( QueryResult queryResult, DictionaryType targetType, out DictResult dictResult )
            {
                bool hasFound = false;
                dictResult = null;
                if ( queryResult != null )
                {
                    if ( queryResult.DictResultItems != null )
                    {

                        foreach ( DictResult resultItem in queryResult.DictResultItems )
                        {
                            if ( resultItem != null && resultItem.DictionaryType == targetType )
                            {
                                dictResult = resultItem;
                                hasFound = true;
                                break;
                            }
                        }
                    }
                }
                return hasFound;
            }

            private bool FindSearchEngineResult(QueryResult queryResult, SearchEngineType targetType, out SearchEngineResult searchEngineResult)
            {
                bool hasFound = false;
                searchEngineResult = null;
                if (queryResult != null)
                {
    
                    if ( queryResult.SearchEngineResultItems != null )
                    {

                        foreach ( SearchEngineResult resultItem in queryResult.SearchEngineResultItems )
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
            private bool FindUIElements ( DictResult dictResult, out ListBox listBox, out TabItem tabItem, out string engineName )
            {
                bool hasFound = false;
                switch ( dictResult.DictionaryType )
                {
                    case DictionaryType.Dict_cn:
                        {
                            hasFound = true;
                            listBox = _parent.DictcnOutputListBox;
                            tabItem = _parent.DictcnTabItem;
                            engineName = "Dictcn";
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

        public void ClearAllListBoxes()
        {
            ClearListBox(GoogleOutputListBox, GoogleTabItem, "谷歌");
            ClearListBox(BaiduOutputListBox, BaiduTabItem, "百度");
            ClearListBox(SougouOutputListBox, SougouTabItem, "搜狗");
            ClearListBox(WikipediaOutputListBox, WikipediaTabItem, "维基");
            ClearListBox(DictcnOutputListBox, DictcnTabItem, "电子辞典");
        }

        private void ClearListBox(ListBox box, TabItem tab, string name)
        {
            box.Items.Clear();
            box.SelectedIndex = -1;
            tab.Header = name;
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

        private void DetailedOpenLinkButton_Click(object sender, RoutedEventArgs e)
        {
            string uri = string.Format("{0}", _selectedItemUri);
            Shell shell = new Shell();
            if (_selectedItemUri != null && _selectedItemUri != string.Empty)
            {
                shell.DoOpenWebBrowser(uri);
            }
        }     

        private void DetailedCloseImageButton_ImageFailed ( object sender, ExceptionRoutedEventArgs e )
        {
            
        }

        private void onMouseLeftButtonDown ( object sender, MouseButtonEventArgs e )
        {
            DetailedGrid.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded ( object sender, RoutedEventArgs e )
        {

        }

        private void DictcnOutputListBox_PreviewMouseWheel ( object sender, MouseWheelEventArgs e )
        {
            int count = DictcnOutputListBox.Items.Count;
            int selectedIndex = DictcnOutputListBox.SelectedIndex;
            if ( 0 <= selectedIndex && selectedIndex < count )
            {
                if ( e.Delta < 0 )
                {
                    selectedIndex = Math.Min( selectedIndex + 2, count );
                }
                else
                {
                    selectedIndex = Math.Max( selectedIndex - 2, 0 );
                }
                DictcnOutputListBox.SelectedIndex = selectedIndex;
            }
        }

        private void DictcnOutputListBox_MouseDoubleClick ( object sender, MouseButtonEventArgs e )
        {
            if ( e.LeftButton == MouseButtonState.Pressed )
            {
                int index = DictcnOutputListBox.SelectedIndex;
                DictResult result = _resultHandler.GetDictcnResult(sender);
                //SearchEngineResult result = _resultHandler.GetSearchEngineResult( sender );
                if ( result != null && ((index & 1 )==0))
                {
                    string uri = string.Format( "{0}", result.SearchUrl );
                    Shell shell = new Shell();
                    shell.DoOpenWebBrowser( uri );
                }
            }
        }

        private void DictcnOutputListBox_SelectionChanged ( object sender, SelectionChangedEventArgs e )
        {

        }

        private void OutputListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox == null)
            {
                // do nothing
            }
            listBox.SelectedIndex = -1;
        }


        
        #endregion
    }
}
