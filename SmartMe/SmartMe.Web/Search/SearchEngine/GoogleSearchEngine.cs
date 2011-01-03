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
    public class GoogleSearchEngine : SearchEngine
	{
        public GoogleSearchEngine()
        {
            this.SearchEngineType = SearchEngineType.Google;
            this.Encoding = Encoding.GetEncoding(Settings.Default.GoogleSearchEngineEncoding);
            this.QueryFormat = Settings.Default.GoogleSearchEngineQueryFormat;
            this.Parser = new GoogleParser();
        }
	}
}
