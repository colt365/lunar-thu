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
			if (query == null || query.QueryType != SmartMe.Core.Data.InputQueryType.Text)
			{
				return null;
			}
			string url = "http://www.sogou.com/web?query=" + HttpUtility.UrlEncode(query.Text, Encoding.GetEncoding("gb2312"));
			
			string html = SmartMe.Web.Crawl.Crawler.Crawl(url, Encoding.GetEncoding("gb2312"));
			if (html == null)
			{
				return null;
			}
			SmartMe.Web.Parse.SogouParser parser = new SmartMe.Web.Parse.SogouParser();
			return parser.Parse(html, Encoding.GetEncoding("gb2312"));
		}

		#endregion
	}
}
