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

namespace WpfBrowser
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void OpenBrowserButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	if (this.OpenAddressTextBox.Text.StartsWith("http://"))
            {
                string addr = this.OpenAddressTextBox.Text.Trim(new char[] { ' ', '\t', '\n', '\r' });          
				this.WebBrowser.BeginInit();
				this.WebBrowser.Navigate(new Uri(addr));
				this.WebBrowser.EndInit();
				/*this.WebBrowser.Document = 
					"<html><head><script>" +
					"function test(message) { alert(message); }" +
					"</script></head><body><button " +
					"onclick=\"window.external.Test('called from script code')\">" +
					"call client code from script code</button>" +
					"</body></html>";*/
				this.WebBrowser.
				
            }
        }

        private void RunButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			string text = this.ScriptTextBox.Text;
			try 
			{
				string wrapperText = "(\"" + text.ToString() + "\")";
				this.WebBrowser.BeginInit();
				this.WebBrowser.EndInit();
				MessageBox.Show("begin: " + wrapperText);
        		object o = this.WebBrowser.InvokeScript("eval", new String[] { text });
				MessageBox.Show("result:" + o);
			}
			catch (Exception ex)
			{
				string msg = "Could not call script: " +
							ex.Message +
							"\n\nPlease click the 'Load HTML Document with Script' button to load." 
					       +"\n\n" + text;
				MessageBox.Show(msg);
			}
        }

    }
}
