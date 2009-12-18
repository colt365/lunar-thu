using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;
using SmartMe.Web.Search;
namespace SmartMe.Web.Test
{
    public class BaiduSuggestionTest
    {
        public static void Test ( )
        {
            BaiduSuggestion engine = new BaiduSuggestion();
            InputQuery query = new InputQuery( "baih" );
            query.QueryType = InputQueryType.Text;
            SuggestionResult item = engine.Search( query );
            Console.WriteLine( item );
        }

    }
}
