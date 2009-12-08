using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

namespace SmartMe.Web.Search
{
	public class GoogleSearchEngine: ISearchEngine
	{
		#region ISearchEngine Members
		

		public SmartMe.Core.Data.SearchEngineResult Search(SmartMe.Core.Data.InputQuery query)
		{
            SmartMe.Core.Data.SearchEngineResult emptyResult = new SmartMe.Core.Data.SearchEngineResult();
            emptyResult.SearchEngineType = SmartMe.Core.Data.SearchEngineType.Google;
            string url = "http://www.google.cn/search?q=" + HttpUtility.UrlEncode(query.Text, Encoding.UTF8);
            emptyResult.SearchUrl=url;
			if(query == null || query.QueryType!= SmartMe.Core.Data.InputQueryType.Text|| query.Text==null || query.Text=="")
			{
                return emptyResult;
			}
			
			string html=SmartMe.Web.Crawl.Crawler.Crawl(url,Encoding.UTF8);
			if(html==null || html=="")
			{
                return emptyResult;
			}
			SmartMe.Web.Parse.GoogleParser parser= new SmartMe.Web.Parse.GoogleParser();
			SmartMe.Core.Data.SearchEngineResult result=parser.Parse(html,Encoding.UTF8);
            result.SearchUrl = url;
            return result;
		}

		#endregion
	}
}
