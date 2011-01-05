using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

using SmartMe.Log;

namespace SmartMe.Windows
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static Logger _logger = null;
        public static Logger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new Logger();
                }
                return _logger;
            }
            private set { _logger = value; }
        }

        public App()
        {
            // Handle UI Thread Exception:
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            // Handle Non-UI Thread Exception:
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMsg = "窗体线程异常: ";
            try 
            {
                Exception ex = e.Exception;
                errorMsg = errorMsg + Environment.NewLine
                                + ex.Message + Environment.NewLine
                                + "Sender: " + sender + Environment.NewLine
                                + ex.StackTrace;
                App.Logger.Error(errorMsg);
            }
            catch 
            {
                errorMsg = errorMsg + "不可恢复，应用程序将退出！";
                App.Logger.Fatal(errorMsg);
            }
            finally
            {
                MessageBox.Show(errorMsg);
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string errorMsg = "非窗体线程异常: ";
            try
            {
                Exception ex = e.ExceptionObject as Exception;
                errorMsg = errorMsg + Environment.NewLine
                             + ex.Message + Environment.NewLine
                             + "Sender: " + sender + Environment.NewLine
                             + ex.StackTrace;
                App.Logger.Error(errorMsg);
            }
            catch
            {
                errorMsg = errorMsg + "不可恢复，应用程序将退出！";
                App.Logger.Fatal(errorMsg);
            }
            finally
            {
                MessageBox.Show(errorMsg);
            }
        }
    }
}
