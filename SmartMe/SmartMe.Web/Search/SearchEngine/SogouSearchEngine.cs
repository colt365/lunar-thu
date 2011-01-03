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
    public class SogouSearchEngine : SearchEngine
	{
        public SogouSearchEngine()
        {
            this.SearchEngineType = SearchEngineType.Sogou;
            this.Encoding = Encoding.GetEncoding(Settings.Default.SogouSearchEngineEncoding);
            this.QueryFormat = Settings.Default.SogouSearchEngineQueryFormat;
            this.Parser = new SogouParser();
        }
	}
}
