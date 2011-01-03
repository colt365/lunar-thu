using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using SmartMe.Core.Data;
using SmartMe.Web.Crawl;
using SmartMe.Web.Parse;
using SmartMe.Web.Properties;

namespace SmartMe.Web.Search
{ 
    public class WikipediaSearchEngine : SearchEngine
    {
        public WikipediaSearchEngine()
        {
            this.SearchEngineType = SearchEngineType.Wikipedia;
            this.Encoding = Encoding.GetEncoding(Settings.Default.WikipediaSearchEngineEncoding);
            this.QueryFormat = Settings.Default.WikipediaSearchEngineQueryFormat;
            this.Parser = new WikipediaParser();
        }
    }
}
