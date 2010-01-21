using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace SmartMe.Windows
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static SmartMe.Log.Logger _logger = null;
        public static SmartMe.Log.Logger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new SmartMe.Log.Logger();
                }
                return _logger;
            }
            private set { _logger = value; }
        }
    }
}
