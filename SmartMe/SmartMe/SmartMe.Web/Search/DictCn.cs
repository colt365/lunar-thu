using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;
using System.Net;
using System.Web;

namespace SmartMe.Web.Search
{
    public class DictCn
    {
        #region ISearchEngine Members
        public DictResult Search ( InputQuery query )
        {

            SmartMe.Core.Data.DictResult emptyResult = new SmartMe.Core.Data.DictResult();
            emptyResult.DictionaryType = SmartMe.Core.Data.DictionaryType.Dict_cn;
            string url = "http://www.dict.cn/search?q=" + HttpUtility.UrlEncode( query.Text, Encoding.GetEncoding( "gb2312" ) );
            emptyResult.SearchUrl = url;
            if ( query == null || query.QueryType != SmartMe.Core.Data.InputQueryType.Text || query.Text == null || query.Text == "" )
            {
                return emptyResult;
            }

            string html = SmartMe.Web.Crawl.Crawler.Crawl( url, Encoding.GetEncoding( "gb2312" ) );
            if ( html == null || html == "" )
            {
                return emptyResult;
            }
            SmartMe.Web.Parse.DictCnParser parser = new SmartMe.Web.Parse.DictCnParser();
            SmartMe.Core.Data.DictResult result = parser.Parse( html, Encoding.GetEncoding( "gb2312" ) );
            result.SearchUrl = url;
            return result;
        }
        #endregion
    }
}
