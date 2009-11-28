using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Data;
using SmartMe.Web.Search;

namespace SmartMe.Web.Test
{
    public class BaiduSearchEngineTest
    {
        public static void Test()
        {
            ISearchEngine engine = new BaiduSearchEngine();
            InputQuery query = new InputQuery("旅游");
            query.QueryType = InputQueryType.Text;
            IQueryResultItem item = engine.Search(query);
            Console.WriteLine(item);
        }
    }
}
