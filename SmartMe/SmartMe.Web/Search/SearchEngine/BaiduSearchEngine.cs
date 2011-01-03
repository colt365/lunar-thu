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
    public class BaiduSearchEngine : SearchEngine
	{
        public BaiduSearchEngine()
        {
            this.SearchEngineType = SearchEngineType.Baidu;
            this.Encoding = Encoding.GetEncoding(Settings.Default.BaiduSearchEngineEncoding);
            this.QueryFormat = Settings.Default.BaiduSearchEngineQueryFormat;
            this.Parser = new BaiduParser();
        }
	}
}
