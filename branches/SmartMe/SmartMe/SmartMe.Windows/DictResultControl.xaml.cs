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
	/// Interaction logic for DictResultControl.xaml
	/// </summary>
    public partial class DictResultControl : UserControl, INotifyPropertyChanged, IQueryResultControl
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

        #region IQueryResultControl interface

        public static readonly DependencyProperty BaseHeaderProperty =
            DependencyProperty.Register("BaseHeader", typeof(string), typeof(DictResultControl),
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
            DependencyProperty.Register("SubHeader", typeof(string), typeof(DictResultControl),
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
            DependencyProperty.Register("Header", typeof(string), typeof(DictResultControl),
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
            DictResultControl control = d as DictResultControl;
            control.Header = string.IsNullOrEmpty(control.SubHeader) ?
                                     string.Format("{0}", control.BaseHeader) :
                                     string.Format("{0}({1})", control.BaseHeader, control.SubHeader);
        }

        public void SetResult(IQueryResultItem queryResult)
        {
            DictResult = queryResult as DictResult;
            SetDictResult(DictResult);
        }
        public void ClearResult()
        {
            this.OutputListBox.SelectedIndex = -1;
            this.DictResult = null;
        }

        #endregion IQueryResultControl interface

        protected DictResult _dictResult = null;
        public DictResult DictResult
        {
            get { return _dictResult; }
            set { _dictResult = value; NotifyPropertyChanged("DictResult"); }
        }
        #endregion properties

		public DictResultControl()
		{
			this.InitializeComponent();
		}

        private void OutputListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            int count = OutputListBox.Items.Count;
            int selectedIndex = OutputListBox.SelectedIndex;
            if (0 <= selectedIndex && selectedIndex < count)
            {
                if (e.Delta < 0)
                {
                    selectedIndex = Math.Min(selectedIndex + 2, count);
                }
                else
                {
                    selectedIndex = Math.Max(selectedIndex - 2, 0);
                }
                OutputListBox.SelectedIndex = selectedIndex;
            }
        }

        private void OutputListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int index = OutputListBox.SelectedIndex;
                DictResult result = DictResult;
                if (result != null && ((index & 1) == 0))
                {
                    string uri = string.Format("{0}", result.SearchUrl);
                    Shell shell = new Shell();
                    shell.DoOpenWebBrowser(uri);
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

        private void SetDictResult(DictResult dictItem)
        {
            if (dictItem != null && !string.IsNullOrEmpty(dictItem.Word))
            {
                ListBox listBox = OutputListBox;

                listBox.Items.Clear();
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

                this.SubHeader = string.Format("{0}", 1);
            }
            else
            {
                this.SubHeader = string.Format("{0}", 0);
            }
        }
	}
}