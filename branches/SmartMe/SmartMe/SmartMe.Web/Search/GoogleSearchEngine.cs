using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using SmartMe.Core.Data;
using SmartMe.Web.Crawl;
using SmartMe.Web.Parse;

namespace SmartMe.Web.Search
{
	public class GoogleSearchEngine: ISearch
	{
		#region ISearch Members


        public IQueryResultItem Search ( InputQuery query )
		{
            SearchEngineResult emptyResult = new SearchEngineResult();
            emptyResult.SearchEngineType = SearchEngineType.Google;
            string url = "http://www.google.com/search?q=" + HttpUtility.UrlEncode(query.Text, Encoding.UTF8);
            emptyResult.SearchUrl=url;
			if(query == null || query.QueryType!= InputQueryType.Text|| string.IsNullOrEmpty(query.Text))
			{
                return emptyResult;
			}
			
			string html=SmartMe.Web.Crawl.Crawler.Crawl(url,Encoding.UTF8);
			if ( string.IsNullOrEmpty(html) )
			{
                return emptyResult;
			}
			GoogleParser parser= new GoogleParser();
			SearchEngineResult result=parser.Parse(html,Encoding.UTF8);
            result.SearchUrl = url;
            return result;
		}

		#endregion
	}
}
