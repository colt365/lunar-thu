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
    public class WikipediaSearchEngine:ISearch
    {
        #region ISearch Members

        public IQueryResultItem Search(InputQuery query)
        {
            SearchEngineResult emptyResult = new SearchEngineResult();
            emptyResult.SearchEngineType = SearchEngineType.Wikipedia;
          
            string url = "http://en.wikipedia.org/w/index.php?title=Special:Search&search=" + HttpUtility.UrlEncode(query.Text, Encoding.UTF8) + "&fulltext=Search";
            emptyResult.SearchUrl = url;
            if (query == null || query.QueryType != SmartMe.Core.Data.InputQueryType.Text || string.IsNullOrEmpty(query.Text))
            {
                return emptyResult;
            }

            string html = Crawler.Crawl(url, Encoding.UTF8);
            if ( string.IsNullOrEmpty(html))
            {
                return emptyResult;
            }
            WikipediaParser parser = new WikipediaParser();
            SearchEngineResult result = parser.Parse(html, Encoding.UTF8);
            result.SearchUrl = url;
            return result;
        }

        #endregion
    }
}
