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
    public class GoogleSuggestion : ISearch
    {
        #region members

        public IQueryResultItem Search ( InputQuery query )
        {
            SuggestionResult finalResult = new SuggestionResult();
            finalResult.SuggestionType = SuggestionType.Google;

            string queryFormat = Settings.Default.GoogleSuggestionQueryFormat;
            string url = string.Format(queryFormat, HttpUtility.UrlEncode(query.Text, Encoding.UTF8));
            finalResult.SearchUrl=url;

            if ( query == null || query.QueryType != SmartMe.Core.Data.InputQueryType.Text || query.Text == null || query.Text == "" )
            {
                return finalResult;
            }

            string html = SmartMe.Web.Crawl.Crawler.Crawl( url, Encoding.UTF8 );
            if ( html == null || html == "" )
            {
                return finalResult;
            }

            Regex p = new Regex( "\\[.+\\]" );
            string jsonResult = p.Match( html ).Value;

            if(jsonResult==null || jsonResult==string.Empty)
            {
                return finalResult;
            }

            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer( typeof( object ) );

            object rawResults = jsonSerializer.ReadObject( new MemoryStream(Encoding.UTF8.GetBytes(jsonResult)) );           

            if (rawResults!=null)
            {
                object[] tmpResults = rawResults as object[];
                if ( tmpResults != null && tmpResults.Length == 2 )
                {
                    object[] results = tmpResults[1] as object[];
                    if ( results != null && results.Length > 0 )
                    {

                        for ( int i = 0; i < results.Length; ++i )
                        {
                            bool success = true;
                            SuggestionResult.ResultItem item = new SuggestionResult.ResultItem();
                            object[] triple = results[i] as object[];
                            if ( triple != null && triple.Length == 3 )
                            {
                                string temp = triple[0] as string;
                                if ( temp != null )
                                {
                                    item.Suggestion = temp;
                                }else
                                {
                                    success = false;
                                }
                                temp = triple[1] as string;
                                if ( temp != null )
                                {
                                    item.Number = temp;
                                }
                                else
                                {
                                    success = false;
                                }
                                temp = triple[2] as string;
                                if ( temp != null )
                                {
                                    item.Index = temp;
                                }
                                else
                                {
                                    success = false;
                                }
                                if ( success )
                                {
                                    finalResult.Items.Add( item );
                                }

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
