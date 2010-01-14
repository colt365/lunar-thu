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
            //       string productionIDBeta1_0 = "{51F468DC-35D1-4119-81A0-E2F12813CD87}"; 
            //       string productionIDBeta1_1 = "{705C251F-D7B1-4D94-A81D-1529B1EA10AF}";
            //       string productionIDBeta1_2 = "{982A5C87-A039-4C7E-8836-AFBD8B1BBB0D}";
            string productionIDBeta1_3 = "{1B1383F4-BD61-4BF3-90F0-BC8922D1768F}";
            string arguments = string.Format("/x {0} /qr", productionIDBeta1_3);
            System.Diagnostics.Process.Start(msiexcPath, arguments);
        }
    }
}
