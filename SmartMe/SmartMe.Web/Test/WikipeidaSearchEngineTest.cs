using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Data;
using SmartMe.Web.Search;

namespace SmartMe.Web.Test
{
    class WikipeidaSearchEngineTest
    {
        public static void Test()
        {
            ISearch engine = new WikipediaSearchEngine();
            InputQuery query = new InputQuery("王菲");
            query.QueryType = InputQueryType.Text;
            IQueryResultItem item = engine.Search(query);
            Console.WriteLine(item);
        }
    }
}
