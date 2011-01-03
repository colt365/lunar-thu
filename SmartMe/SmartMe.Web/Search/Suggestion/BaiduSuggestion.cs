using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using SmartMe.Core.Data;
using SmartMe.Web.Properties;

namespace SmartMe.Web.Search
{
    public class BaiduSuggestion : ISearch
    {
        #region  members

        public IQueryResultItem Search ( InputQuery query )
        {
            SuggestionResult finalResult = new SuggestionResult();
            finalResult.SuggestionType = SuggestionType.Baidu;
            Encoding gb2312 = Encoding.GetEncoding( "gb2312" );
            Encoding utf8 = Encoding.UTF8;

            string queryFormat = Settings.Default.BaiduSuggestionQueryFormat;
            string url = string.Format(queryFormat, HttpUtility.UrlEncode(query.Text, gb2312));

            finalResult.SearchUrl = url;

            if ( query == null || query.QueryType != SmartMe.Core.Data.InputQueryType.Text || string.IsNullOrEmpty(query.Text) )
            {
                return finalResult;
            }

            string html = SmartMe.Web.Crawl.Crawler.Crawl( url, gb2312 );
            if ( string.IsNullOrEmpty(html) )
            {
                return finalResult;
            }

            Regex p = new Regex( "\\[.+\\]" );
            string jsonResult = p.Match( html ).Value;
            if ( jsonResult == null || jsonResult == string.Empty )
            {
                return finalResult;
            }
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer( typeof( object ) );

            object rawResults = jsonSerializer.ReadObject( new MemoryStream( Encoding.Convert(gb2312,utf8,gb2312.GetBytes(jsonResult)) ) );

            if(rawResults!=null)
            {
                object[] preResults = rawResults as object[];
                if ( preResults!=null && preResults.Length==2)
                {
                    object[] results = preResults[1] as object[];
                    if ( results != null && results.Length > 0 )
                    {
                        for ( int i = 0; i < results.Length; ++i )
                        {
                            SuggestionResult.ResultItem item = new SuggestionResult.ResultItem();
                            string temp = results[i] as string;
                            if ( temp != null )
                            {
                                item.Suggestion = temp;
                                //Console.WriteLine( temp );
                                finalResult.Items.Add( item );
                            }
                        }
                    }
                }
            }
            return finalResult;

        }

        #endregion
    }
}
