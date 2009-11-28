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
        private List<string> _searchResults;
        public List<string> SearchResults
        {
            get { return _searchResults; }
            set { _searchResults = value; }
        }
		private string _bindingString;
        public string BindingString
        {
            get { return _bindingString; }
            set { _bindingString = value; }
        }

        WebResourceManager _webResourceManager = null;
        Pipeline _pipeline = null;
        IQueryResultHandler _resultHandler = null;
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
            _resultHandler = new QueryResultHandler(this.OutputListBox);

            _webResourceManager = new WebResourceManager(_pipeline, _resultHandler);
            _pipeline.InputTextSubscriberManager.AddSubscriber(_webResourceManager);

            _webResourceManager.AddSearchEngine(new GoogleSearchEngine());
            _webResourceManager.AddSearchEngine(new BaiduSearchEngine());
        }
        
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
                InputQuery query = new InputQuery(text);
                query.QueryType = InputQueryType.Text;
                InputTextBox.Text = "Query:" + text;
                _pipeline.OnInputTextReady(query);
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

		private void Window_DragLeave(object sender, System.Windows.DragEventArgs e)
		{
			// TODO: Add event handler implementation here.
			ResultTextBox.IsEnabled = true;
		}

		private void Window_DragEnter(object sender, System.Windows.DragEventArgs e)
		{
			// TODO: Add event handler implementation here.
			ResultTextBox.IsEnabled = false;
			ResultTextBox.Text = "ResultTextBox.IsEnabled: false";
		}


        class QueryResultHandler : IQueryResultHandler
        {
            private ListBox _outputListBox = null;
            public QueryResultHandler(ListBox outputListBox)
            {
                _outputListBox = outputListBox;
            }

            #region IQueryResultHandler 成员
            public void OnResultNew(QueryResult result)
            {
                _outputListBox.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                    delegate()
                    {
                        _outputListBox.Items.Clear();
                    })
                );
            }

            public void OnResultUpdate(QueryResult result)
            {
                _outputListBox.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                    delegate()
                    {
                        _outputListBox.Items.Clear();
                        foreach (SearchEngineResult searchItem in result.Items)
                        {
                            foreach (SearchEngineResult.ResultItem resultItem in searchItem.Results)
                            {
                                _outputListBox.Items.Add(new ListBoxItem() { Content = resultItem.ToString() });
                            }
                        }
                        _outputListBox.InvalidateArrange();
                        _outputListBox.UpdateLayout();
                    })
                );
            }

            public void OnResultDeprecated(QueryResult result)
            {
                MessageBox.Show("OnResultDeprecated" + result.ToString());
            }

            public void OnResultCompleted(QueryResult result)
            {
                MessageBox.Show("OnResultCompleted" + result.ToString());
            }
            #endregion
        }
	}
}