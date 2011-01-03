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
	public class SogouSearchEngine:ISearch
	{
		#region ISearch Members

        public IQueryResultItem Search ( InputQuery query )
		{
            SearchEngineResult emptyResult = new SearchEngineResult();
            emptyResult.SearchEngineType = SearchEngineType.Sougou;
            string url = "http://www.sogou.com/web?query=" + HttpUtility.UrlEncode(query.Text, Encoding.GetEncoding("gb2312"));
            emptyResult.SearchUrl=url;
            if (query == null || query.QueryType != SmartMe.Core.Data.InputQueryType.Text || string.IsNullOrEmpty(query.Text))
			{
                return emptyResult;
			}
			
			string html = Crawler.Crawl(url, Encoding.GetEncoding("gb2312"));
			if ( string.IsNullOrEmpty(html) )
			{
                return emptyResult;
			}
			SogouParser parser = new SogouParser();
			SearchEngineResult result=parser.Parse(html, Encoding.GetEncoding("gb2312"));
            result.SearchUrl=url;
            return result;
		}

		#endregion
	}
}
