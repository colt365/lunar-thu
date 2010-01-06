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
            
            // History ProductionID List:
            //       string productionIDBeta100 = "{51F468DC-35D1-4119-81A0-E2F12813CD87}"; 

            string productionIDBeta101 = "{705C251F-D7B1-4D94-A81D-1529B1EA10AF}";

            string arguments = string.Format("/x {0} /qr", productionIDBeta101);
            System.Diagnostics.Process.Start(msiexcPath, arguments);
        }
    }
}
