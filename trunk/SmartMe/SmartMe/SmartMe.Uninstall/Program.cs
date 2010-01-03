using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Uninstall
{
    class Program
    {
        static void Main(string[] args)
        {
            string systemRoot = System.Environment.SystemDirectory;
            string msiexcPath = System.IO.Path.Combine(systemRoot, "msiexec");
            string productionID = "{51F468DC-35D1-4119-81A0-E2F12813CD87}";
            string arguments = string.Format("/x {0} /qr", productionID);
            System.Diagnostics.Process.Start(msiexcPath, arguments);
        }
    }
}
