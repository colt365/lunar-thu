using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

using SmartMe.Core.Data;
using SmartMe.Web.Externel;

namespace SmartMe.Web.Test
{
    public class ExternelSearchEngineTest
    {
        private static WebBrowser _webBrowser = new WebBrowser();

        private const string _googleUrlPattern = "http://www.google.cn/search?q={0}";

        private const string _scriptPath = "google.template.js";

        private static string[] _libPath = null;

        private static string _queryString = "北京";

        public static void Test()
        {
            _libPath = new string[] {
                "jquery-1.3.2.min.js",
                "SmartMe-Buildin-Script.js"
            };
            ExternelSearchEngine engine = new ExternelSearchEngine(_webBrowser, _googleUrlPattern, _scriptPath);
            engine.Encoding = Encoding.UTF8;
            engine.LibraryPath.AddRange(_libPath);

            InputQuery query = new InputQuery(_queryString, InputQueryType.Text);
            SearchEngineResult result = engine.Search(query) as SearchEngineResult;
            Console.WriteLine(result);
        }
    }
}
