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
using System.ComponentModel;

namespace SmartMe.Windows
{
    /// <summary>
    /// Interaction logic for DetailedInfoWindow.xaml
    /// </summary>
    public partial class DetailedInfoWindow : Window
    {
        private bool _isClosed = false;
        public bool IsClosed
        {
            get
            {
                return _isClosed;
            }
        }

	    private string _linkedUrl = string.Empty;
        public string LinkedUrl
        {
            get
            {
                return _linkedUrl;
            }
            set
            {
                _linkedUrl = (value != null) ? value : string.Empty;
            }
        }
        
        public string Title
        {
            get
            {
                return TitleTextBlock.Text;
            }
            set
            {
                TitleTextBlock.Text = (value != null) ? value : string.Empty;
            }
        }

        public string Desciprtion
        {
            get
            {
                return DescriptionTextBlock.Text;
            }
            set
            {
                DescriptionTextBlock.Text = (value != null) ? value : string.Empty;
            }
        }
	
        public DetailedInfoWindow()
        {
            InitializeComponent();
        }

        private void OpenLinkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (LinkedUrl != string.Empty)
            {
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(
                    delegate()
                    {
                        Externel.Shell shell = new Externel.Shell();
                        shell.DoOpenWebBrowser(LinkedUrl);
                    })
                );
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _isClosed = true;
            base.OnClosed(e);
        }
    }
}
