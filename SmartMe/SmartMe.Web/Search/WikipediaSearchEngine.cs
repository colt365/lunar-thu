using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

namespace SmartMe.Web.Search
{
    public class WikipediaSearchEngine:ISearch
    {
        #region ISearch Members


        public SmartMe.Core.Data.IQueryResultItem Search(SmartMe.Core.Data.InputQuery query)
        {
            SmartMe.Core.Data.SearchEngineResult emptyResult = new SmartMe.Core.Data.SearchEngineResult();
            emptyResult.SearchEngineType = SmartMe.Core.Data.SearchEngineType.Wikipedia;
          
            string url = "http://en.wikipedia.org/w/index.php?title=Special:Search&search=" + HttpUtility.UrlEncode(query.Text, Encoding.UTF8) + "&fulltext=Search";
            emptyResult.SearchUrl = url;
            if (query == null || query.QueryType != SmartMe.Core.Data.InputQueryType.Text || query.Text == null || query.Text == "")
            {
                return emptyResult;
            }

            string html = SmartMe.Web.Crawl.Crawler.Crawl(url, Encoding.UTF8);
            if (html == null || html == "")
            {
                return emptyResult;
            }
            SmartMe.Web.Parse.WikipediaParser parser = new SmartMe.Web.Parse.WikipediaParser();
            SmartMe.Core.Data.SearchEngineResult result = parser.Parse(html, Encoding.UTF8);
            result.SearchUrl = url;
            return result;
        }

        #endregion
    }
}
