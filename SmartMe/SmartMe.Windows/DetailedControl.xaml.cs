using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using SmartMe.Windows.Externel;

namespace SmartMe.Windows
{
	/// <summary>
	/// Interaction logic for DetailedControl.xaml
	/// </summary>
	public partial class DetailedControl : UserControl
	{
        protected string _link = string.Empty;
        public string Link {
            get { return _link; }
            set { _link = value; }
        }

        protected string _title = string.Empty;
        public string Title {
            get { return _title; }
            set {
                _title = value;
                TitleTextBox.Text = _title; 
            }
        }

        protected string _description = string.Empty;
        public string Description
        {
            get {
                return _description;
            }
            set
            {
                _description = value;
                DescriptionRichTextBox.Document.Blocks.Clear();
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(_description);
                DescriptionRichTextBox.Document.Blocks.Add(paragraph);
            }
        }

		public DetailedControl()
		{
			this.InitializeComponent();
		}

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.Visibility = Visibility.Hidden;
        }


        private void onMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
        }
        private void OpenLinkButton_Click(object sender, RoutedEventArgs e)
        {
            string uri = Link;
            if ( !string.IsNullOrEmpty(uri) )
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background,
                    new Action(delegate() {
                        Shell shell = new Shell();
                        shell.DoOpenWebBrowser(uri);
                    })
                );
            }
        }
	}
}