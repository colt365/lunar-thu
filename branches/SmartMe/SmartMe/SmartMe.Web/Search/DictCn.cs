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
    public class DictCn:ISearch
    {
        #region ISearch Members
        public IQueryResultItem Search ( InputQuery query )
        {
            SmartMe.Core.Data.DictResult emptyResult = new SmartMe.Core.Data.DictResult();
            emptyResult.DictType = DictType.Dict_cn;
            string url = "http://dict.cn/search?q=" + HttpUtility.UrlEncode( query.Text, Encoding.GetEncoding( "gb2312" ) );
            emptyResult.SearchUrl = url;
            if ( query == null || query.QueryType != InputQueryType.Text || string.IsNullOrEmpty(query.Text))
            {
                return emptyResult;
            }

            string html = Crawler.Crawl( url, Encoding.GetEncoding( "gb2312" ) );
            if ( string.IsNullOrEmpty(html) )
            {
                return emptyResult;
            }
            DictCnParser parser = new DictCnParser();
            DictResult result = parser.Parse( html, Encoding.GetEncoding( "gb2312" ) );
            result.SearchUrl = url;
            return result;
        }
        #endregion
    }
}
