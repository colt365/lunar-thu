using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

namespace SmartMe.Web.Search
{
	public class BaiduSearchEngine: ISearchEngine
	{
		#region ISearchEngine Members

		public SmartMe.Core.Data.SearchEngineResult Search(SmartMe.Core.Data.InputQuery query)
		{
			if (query == null || query.QueryType != SmartMe.Core.Data.InputQueryType.Text)
			{
				return null;
			}
			string url = "http://www.baidu.com/s?wd=" + HttpUtility.UrlEncode(query.Text, Encoding.GetEncoding("gb2312"));

			string html = SmartMe.Web.Crawl.Crawler.Crawl(url, Encoding.GetEncoding("gb2312"));
			SmartMe.Web.Parse.BaiduParser parser = new SmartMe.Web.Parse.BaiduParser();
			return parser.Parse(html, Encoding.GetEncoding("gb2312"));
		}

		#endregion
	}
}
