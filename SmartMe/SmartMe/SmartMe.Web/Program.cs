using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Web.Test;

namespace SmartMe.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            //WebResourceManagerTester.Test();
            GoogleSearchEngineTest.Test();
			BaiduSearchEngineTest.Test();
			SogouSearchEngineTest.Test();
        }
    }
}
