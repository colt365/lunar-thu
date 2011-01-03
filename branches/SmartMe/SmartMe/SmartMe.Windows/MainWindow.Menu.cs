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
using SmartMe.Core.Record;
using SmartMe.Web;
using SmartMe.Web.Search;
using SmartMe.Windows.Externel;


namespace SmartMe.Windows
{
    partial class MainWindow : Window
    {
        #region MinimizeToIcon

        private void MinimizeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void CloseMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Hide();
            if (_notifyIcon != null)
            {
                _notifyIcon.ShowBalloonTip(500);
            }
        }

        #endregion MinimizeToIcon
    }
}
