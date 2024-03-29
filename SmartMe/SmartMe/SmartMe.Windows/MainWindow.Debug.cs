﻿using System;
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
        #region for Debug
        public enum Level
        {
            Normal,
            Warning,
            Error,
            Fatal
        }

        public void MessageDebug(object o)
        {
            MessageDebug(o, Level.Normal);
        }

        public void MessageDebug(object o, Level level)
        {
            string str = o.ToString();
            App.Logger.Message(str);
        }
        #endregion for Debug

       
    }
}
