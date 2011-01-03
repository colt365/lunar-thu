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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using SmartMe.Core.Data;
using SmartMe.Windows.Externel;

namespace SmartMe.Windows
{
	/// <summary>
    /// Interaction logic for SearchEngineResultControl.xaml
	/// </summary>
	public partial class SearchEngineResultControl : UserControl, INotifyPropertyChanged, IQueryResultControl
    {
        #region properties

        #region INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion INotifyPropertyChanged interface

        public event DisplayResultHandler DisplayResult;
        private void OnDisplayResult(string title, string description, string url)
        {
            DisplayResultEventArgs e = new DisplayResultEventArgs() { Title = title, Description = description, Url = url };
            if (DisplayResult != null)
            {
                DisplayResult(this, e);
            }
        }
        #region IQueryResultControl interface

        public static readonly DependencyProperty BaseHeaderProperty =
            DependencyProperty.Register("BaseHeader", typeof(string), typeof(SearchEngineResultControl),
                new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnBaseHeaderPropertyChange)));
        public string BaseHeader
        {
            get { return (string)GetValue(BaseHeaderProperty); }
            set { SetValue(BaseHeaderProperty, value); }
        }
        private static void OnBaseHeaderPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateHeaderProperty(d);
        }

        public static readonly DependencyProperty SubHeaderProperty =
            DependencyProperty.Register("SubHeader", typeof(string), typeof(SearchEngineResultControl),
                new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnSubHeaderPropertyChange)));
        public string SubHeader
        {
            get { return (string)GetValue(SubHeaderProperty); }
            set { SetValue(SubHeaderProperty, value); }
        }
        private static void OnSubHeaderPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateHeaderProperty(d);
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(SearchEngineResultControl),
                new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnHeaderPropertyChange)));
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            internal set { SetValue(HeaderProperty, value); }
        }
        private static void OnHeaderPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateHeaderProperty(d);
        }
        private static void UpdateHeaderProperty(DependencyObject d)
        {
            SearchEngineResultControl control = d as SearchEngineResultControl;
            control.Header = string.IsNullOrEmpty(control.SubHeader) ?
                                     string.Format("{0}", control.BaseHeader) :
                                     string.Format("{0}({1})", control.BaseHeader, control.SubHeader);
        }

        public void SetResult(IQueryResultItem queryResult)
        {
            SearchEngineResult = queryResult as SearchEngineResult;
            if (SearchEngineResult.Items != null) {
                SubHeader = string.Format("{0}", SearchEngineResult.Items.Count);
            }
        }
        public void ClearResult()
        {
            this.OutputListBox.SelectedIndex = -1;
            this.SearchEngineResult = null;
        }
        
        #endregion IQueryResultControl interface

        protected SearchEngineResult _searchEngineResult = null;
        public SearchEngineResult SearchEngineResult
        {
            get { return _searchEngineResult; }
            set { _searchEngineResult = value; NotifyPropertyChanged("SearchEngineResult"); }
        }

        #endregion properties

        public SearchEngineResultControl()
		{
			this.InitializeComponent();
		}
		
        private void OutputListBox_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            int count = OutputListBox.Items.Count;
            int selectedIndex = OutputListBox.SelectedIndex;
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
                OutputListBox.SelectedIndex = selectedIndex;
            }
        }

        private void OutputListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int index = OutputListBox.SelectedIndex;
                OpenSearchEngineResult(sender, index);
            }
        }

        private void OutputListBox_SelectionChanged(object sender,
                                        System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DisplayResult == null) { return; }
            
            SearchEngineResult result = _searchEngineResult;
            int index = OutputListBox.SelectedIndex;
            if (result != null) 
            {
                if ( 0 <= index && index < result.Items.Count) 
                {
                    string title = string.Format("{0}", result.Items[index].Title);
                    string url = string.Format("{0}", result.Items[index].Url);
                    string description = string.Format("{0}", result.Items[index].Description);
                    OnDisplayResult(title, description, url);
                }
            }
        }

        private void OutputListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null)
            {
                listBox.SelectedIndex = -1;
            }
        }

        private void OpenSearchEngineResult(object sender, int index)
        {
            SearchEngineResult result = _searchEngineResult;
            if (result != null)
            {
                if (0 <= index && index < result.Items.Count)
                {
                    string itemUrl = string.Format("{0}", result.Items[index].Url);
                    Shell shell = new Shell();
                    shell.DoOpenWebBrowser(itemUrl);
                }
                else if (index == -1)
                {
                    string searchUrl = string.Format("{0}", result.SearchUrl);
                    Shell shell = new Shell();
                    shell.DoOpenWebBrowser(searchUrl);
                }
            }
        }
    }
}