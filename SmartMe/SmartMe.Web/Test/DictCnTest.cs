using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;
using SmartMe.Web.Search;
namespace SmartMe.Web.Test
{
    public class DictCnTest
    {
         public static void Test()
         {
             DictCn engine = new DictCn();
             InputQuery query = new InputQuery( "time" );
             query.QueryType = InputQueryType.Text;
             IQueryResultItem item = engine.Search( query );
             Console.WriteLine( item );
         }
    }
}
