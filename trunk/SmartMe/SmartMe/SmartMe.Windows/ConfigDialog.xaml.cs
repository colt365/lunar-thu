using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartMe.Windows
{
	/// <summary>
	/// Interaction logic for ConfigDialog.xaml
	/// </summary>
	public partial class ConfigDialog : Window
	{
		public ConfigDialog()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

        public string TimeSpanText
        {
            get
            {
                return TimeSpanTextBlock.Text;
            }
        }

        public bool Ok
        {
            get;
            set;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Ok = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Ok = false;
            this.Close();
        }
	}

 
}