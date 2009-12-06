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
        #endregion

        public MainWindow()
		{
			this.InitializeComponent();

			// Insert code required on object creation below this point.
            CreateListeners();
		}
        private void CreateListeners()
        {
            _pipeline = new Pipeline();
            _resultHandler = new QueryResultHandler(this);

            _webResourceManager = new WebResourceManager(_pipeline, _resultHandler);
            _pipeline.InputTextSubscriberManager.AddSubscriber(_webResourceManager);

            _webResourceManager.AddSearchEngine(new GoogleSearchEngine());
            _webResourceManager.AddSearchEngine(new BaiduSearchEngine());
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
        private void DoQuery(string text, InputQueryType queryType)
        {
            switch (queryType)
            {
                case InputQueryType.Text:
                {
                    InputQuery query = new InputQuery(text);
                    query.QueryType = InputQueryType.Text;
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
                //
                //启动外部程序
                //
                process = System.Diagnostics.Process.Start(info);
            }
            catch (ArgumentNullException e)
            {
                // MessageBox.Show(string.Format("错误：{0}", e.Message));
                return;
            }
            catch (Win32Exception e)
            {
                // MessageBox.Show(string.Format("错误：{0}", e.Message));
                return;
            }
            catch (ObjectDisposedException e)
            {
                // MessageBox.Show(string.Format("错误：{0}", e.Message));
                return;
            }
            catch (InvalidOperationException e)
            {
                // MessageBox.Show(string.Format("错误：{0}", e.Message));
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
            /*
            bool ignoreCase = true;
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentUICulture;
            if (!defaultPage.StartsWith("http://", ignoreCase, culture))
            {
                string url = defaultPage.TrimStart(new char[] { ' ' });
                defaultPage = "http://" + defaultPage;
            }
            */
            //MessageBox.Show("Uri:" + uriBuilder.Uri.ToString());
            info.UseShellExecute = true;
            
            //设置外部程序名 www.baidu.com
            info.FileName = uriBuilder.Uri.ToString();

            //设置外部程序的启动参数（命令行参数) 
            info.Arguments = "";

            //MessageBox.Show(info.FileName + " " + info.Arguments);

            //设置外部程序工作目录为  C:\
            info.WorkingDirectory = ".";

            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(
                    delegate()
                    {
                        this.DoShellCall(info);
                    })
            );
            

            ////打印出外部程序的开始执行时间
            //Console.WriteLine("外部程序的开始执行时间：{0}",  Proc.StartTime);

            ////等待3秒钟
            //Proc.WaitForExit(3000);

            ////如果这个外部程序没有结束运行则对其强行终止
            //if(Proc.HasExited  ==  false)
            //{
            //    Console.WriteLine("由主程序强行终止外部程序的运行！");
            //    Proc.Kill();
            //}
            //else
            //{
            //    Console.WriteLine("由外部程序正常退出！");
            //}
            //Console.WriteLine("外部程序的结束运行时间：{0}",  Proc.ExitTime);
            //Console.WriteLine("外部程序在结束运行时的返回值：{0}",  Proc.ExitCode);
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
			/*
            if (e.Data.GetDataPresent("UniformResourceLocator", true))
            {
                sb.AppendLine("UniformResourceLocator:" + e.Data.GetData(Type.GetType("UniformResourceLocator", false, true)));
            }
            if (e.Data.GetDataPresent("UniformResourceLocatorW", true))
            {
                sb.AppendLine("UniformResourceLocatorW:" + e.Data.GetData(Type.GetType("UniformResourceLocatorW", false, true)));
            }
			
            if (e.Data.GetDataPresent("UniformText", true))
            {
                sb.AppendLine("UniformText:" + e.Data.GetData(Type.GetType("UniformText", false, true)));
            }
            if (e.Data.GetDataPresent("FileName", true))
            {
                sb.AppendLine("FileName:" + e.Data.GetData(Type.GetType("FileName", false, true)));
            }
            if (e.Data.GetDataPresent("FileContents", true))
            {
                sb.AppendLine("FileContents:" + e.Data.GetData(Type.GetType("FileContents", false, true)));
            }
			*/
            
            ResultTextBox.Text += sb.ToString();
		}
        private void Window_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            // TODO: Add event handler implementation here.
            ResultTextBox.IsEnabled = false;
            ResultTextBox.Text = "Window_DragEnter: ResultTextBox.IsEnabled: false";

            InputTextBox.IsEnabled = false;
        }
        private void Window_DragOver(object sender, System.Windows.DragEventArgs e)
        {
            ResultTextBox.IsEnabled = false;
            ResultTextBox.Text = "Window_DragOver: ResultTextBox.IsEnabled: false";

            InputTextBox.IsEnabled = false;
        }
		private void Window_DragLeave(object sender, System.Windows.DragEventArgs e)
		{
			// TODO: Add event handler implementation here.
			ResultTextBox.IsEnabled = true;
            InputTextBox.IsEnabled = true;
		}
        #endregion 鼠标拖拽

        #region 搜索栏
		private void InputTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
			if (e.Key == Key.Return)
			{
				string text = InputTextBox.Text;
				text = text.Trim(new char[] {' ', '\t', '\r', '\n'});
				DoQuery(text, InputQueryType.Text);
			}
        	// MessageBox.Show("[" + e.Key + "]"); // TODO: Add event handler implementation here.
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

        #region 结果栏
		private void GoogleOutputListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	int index = GoogleOutputListBox.SelectedIndex;
            string uri = _resultHandler.GetUri(sender, index);
            if (uri != null)
            {
                DoOpenWebBrowser(uri);
            }
        }
        private void BaiduOutputListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int index = BaiduOutputListBox.SelectedIndex;
            string uri = _resultHandler.GetUri(sender, index);
            if (uri != null)
            {
                DoOpenWebBrowser(uri);
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

            public string GetUri(object sender, int itemIndex)
            {
                string uri = null;
                if (sender != null)
                {
                    SearchEngineResult searchEngineResult = null;
                    bool hasFound = FindSearchEngineResult(sender, out searchEngineResult);
                    if (hasFound) 
                    {
                        if (0 <= itemIndex && itemIndex < searchEngineResult.Results.Count)
                        {
                            uri = string.Format("{0}", searchEngineResult.Results[itemIndex].Url);
                        }
                    }
                }
                return uri;
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
                            engineName = "谷歌结果";
                            break;
                        }
                    case SearchEngineType.Baidu:
                        {
                            hasFound = true;
                            listBox = _parent.BaiduOutputListBox;
                            tabItem = _parent.BaiduTabItem;
                            engineName = "百度结果";
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

        private void InputTextBox_LostKeyboardFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }
        #endregion for Debug
	}
}