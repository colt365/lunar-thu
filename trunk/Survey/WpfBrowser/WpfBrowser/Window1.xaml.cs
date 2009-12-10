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
using System.Security;

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
            this.WebBrowser.BeginInit();
            this.WebBrowser.LoadCompleted += new LoadCompletedEventHandler(this.WebBrowser_LoadCompleted);
            this.WebBrowser.EndInit();
        }

        private void OpenBrowserButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	if (this.OpenAddressTextBox.Text.StartsWith("http://"))
            {
                string addr = this.OpenAddressTextBox.Text.Trim(new char[] { ' ', '\t', '\n', '\r' });          
				this.WebBrowser.Navigate(new Uri(addr));
				/*
                 this.WebBrowser.Document = 
					"<html><head><script>" +
					"function test(message) { alert(message); }" +
					"</script></head><body><button " +
					"onclick=\"window.external.Test('called from script code')\">" +
					"call client code from script code</button>" +
					"</body></html>";
                 */
            }
        }

        private void WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            MessageBox.Show("WebBrowser_LoadCompleted");

            string libPath = @"SmartMe-Buildin-Script.js";
            string content = "";
            string error = "";
            bool isOK = FileLoader.LoadScript(libPath, out content, out error);
            if (isOK)
            {
                MessageBox.Show("Code is: \r\n" + content);
                try
                {
                    MessageBox.Show("[ Embedding Script ]");
                    object o = this.WebBrowser.InvokeScript("eval", new String[] { content });
                    MessageBox.Show("[ Script Embedded ]");
                    MessageBox.Show("[Application]Result:" + o);
                }
                catch (Exception ex)
                {
                    string msg = "Could not call script: " +
                                ex.Message;
                    MessageBox.Show(msg);
                }
            }
            else
            {
                MessageBox.Show(error);
            }
        }


        private void RunButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			string text = this.ScriptTextBox.Text;
			try
			{
                MessageBox.Show("begin: " + text);
        		var o = this.WebBrowser.InvokeScript("eval", new String[] {  text });
                if (o == null)
                {
                    MessageBox.Show("[Application] Result is null");
                }
                else
                {
                    MessageBox.Show("[Application] Get Result:" + "\r\n" 
                                    + "GetType(): "+ o.GetType() + "\r\n"
                                    + "ToString(): " + o.ToString() + "\r\n"
                        );
                    try
                    {
                        Object obj = o as Object;
                        if (obj != null)
                        {
                            MessageBox.Show("o as Object: " + obj);
                        }
                        else 
                        {
                            MessageBox.Show("o as Object failure");
                        }
                    }
                    catch (System.Exception)
                    {
                    }
                    try
                    {
                        Array array = o as Array;
                        if (array != null)
                        {
                            MessageBox.Show("o as Array: " + array);
                        }
                        else
                        {
                            MessageBox.Show("o as Array failure");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        
                    }
                    try
                    {
                    }
                    catch (System.Exception ex)
                    {
                     
                    }

                }
			}
			catch (Exception ex)
			{
				string msg = "Could not call script: " + ex.Message;
				MessageBox.Show(msg);
			}
        }
    }
}
