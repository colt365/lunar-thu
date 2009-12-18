using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Data;
using SmartMe.Web.Search;

namespace SmartMe.Web.Test
{
    public class GoogleSuggestionTest
    {
        public static void Test ( )
        {
            GoogleSuggestion engine = new GoogleSuggestion();
            InputQuery query = new InputQuery( "baim" );
            query.QueryType = InputQueryType.Text;
            SuggestionResult item = engine.Search( query );
            Console.WriteLine( item );
        }

    }
}
