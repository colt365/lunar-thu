using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

namespace SmartMe.Web.Search
{
	class SogouSearchEngine:ISearchEngine
	{
		#region ISearchEngine Members

		public SmartMe.Core.Data.SearchEngineResult Search(SmartMe.Core.Data.InputQuery query)
		{
            SmartMe.Core.Data.SearchEngineResult emptyResult = new SmartMe.Core.Data.SearchEngineResult();
            emptyResult.SearchEngineType = SmartMe.Core.Data.SearchEngineType.Sougou;
            string url = "http://www.sogou.com/web?query=" + HttpUtility.UrlEncode(query.Text, Encoding.GetEncoding("gb2312"));
            emptyResult.SearchUrl=url;
            if (query == null || query.QueryType != SmartMe.Core.Data.InputQueryType.Text || query.Text == null || query.Text == "")
			{
                return emptyResult;
			}
			
			
			string html = SmartMe.Web.Crawl.Crawler.Crawl(url, Encoding.GetEncoding("gb2312"));
			if (html == null||html=="")
			{
                return emptyResult;
			}
			SmartMe.Web.Parse.SogouParser parser = new SmartMe.Web.Parse.SogouParser();
			SmartMe.Core.Data.SearchEngineResult result=parser.Parse(html, Encoding.GetEncoding("gb2312"));
            result.SearchUrl=url;
            return result;
		}

		#endregion
	}
}
