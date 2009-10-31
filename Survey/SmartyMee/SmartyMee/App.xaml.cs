using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using SmartyMee.Kernel.Log;

namespace SmartyMee
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static Logger Log = new Kernel.Log.Logger();
    }
}
