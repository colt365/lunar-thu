using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

namespace SmartMe.Web.Search
{
	class GoogleSearchEngine: ISearchEngine
	{
		#region ISearchEngine Members
		

		public SmartMe.Core.Data.SearchEngineResult Search(SmartMe.Core.Data.InputQuery query)
		{
			if(query == null || query.QueryType!= SmartMe.Core.Data.InputQueryType.Text)
			{
				return null;
			}
			string url = "http://www.google.cn/search?q=" + HttpUtility.UrlEncode(query.Text, Encoding.UTF8);

			string html=SmartMe.Web.Crawl.Crawler.Crawl(url,Encoding.UTF8);
			SmartMe.Web.Parse.GoogleParser parser= new SmartMe.Web.Parse.GoogleParser();
			return parser.Parse(html,Encoding.UTF8);
		}

		#endregion
	}
}
