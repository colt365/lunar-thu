using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Data;
using SmartMe.Core.Pipeline;
using SmartMe.Core.Record;

using System.Threading;

namespace SmartMe.Core.Test
{
    public class QueryResultRecordManagerTest
    {
        public static void Test()
        {
            QueryResultRecordManager manager = new QueryResultRecordManager("data", new TimeSpan(0, 1, 0));

            Pipeline.Pipeline pipeline = new Pipeline.Pipeline();
            pipeline.QueryResultSubscriberManager.AddSubscriber(manager);

            QueryResult result = new QueryResult(new InputQuery("Bill Gates"));
            SearchEngineResult resultItem = new SearchEngineResult();
            resultItem.SearchEngineType = SearchEngineType.Google;
            resultItem.SearchUrl = "http://www.google.com/query.jsp";
            SearchEngineResult.ResultItem item = new SearchEngineResult.ResultItem();
            item.Title = "ddd";
            item.Url = "http://www.gfw.com/";
            item.SimilarUrl = "http://www.g.com/ddd";
            item.CacheUrl = "http://www.g.com/cache";
            item.Description = "Who cares?";
            resultItem.Results.Add(item);

            SuggestionResult resultItem1 = new SuggestionResult();
            resultItem1.SuggestionType = SuggestionType.Google;
            resultItem1.SearchUrl = "json";
            SuggestionResult.ResultItem item1 = new SuggestionResult.ResultItem();
            item1.Index = "1";
            item1.Number = "2";
            item1.Suggestion = "haha";
            resultItem1.Results.Add( item1 );

            result.SearchEngineResultItems.Add(resultItem);
            result.SuggestionResultItems.Add(resultItem1);

            pipeline.OnQueryResultReady(result);

            Thread.Sleep(1000);

            List<QueryResult> resultList = manager.GetResultList(DateTime.Today, DateTime.Today);
            foreach (QueryResult queryResult in resultList)
            {
                Console.WriteLine(queryResult);
            }
            Console.WriteLine("----------------------------------------------");

            manager.RemoveAllResultList();

            resultList = manager.GetResultList(DateTime.Today, DateTime.Today);
            foreach (QueryResult queryResult in resultList)
            {
                Console.WriteLine(queryResult);
            }

            Console.WriteLine("----------------------------------------------");

            result = new QueryResult(new InputQuery("Bill Gates"));
            resultItem = new SearchEngineResult();
            resultItem.SearchEngineType = SearchEngineType.Google;
            resultItem.SearchUrl = "http://www.google.com/query.jsp";
            item = new SearchEngineResult.ResultItem();
            item.Title = "ddd";
            item.Url = "http://www.gfw.com/";
            item.SimilarUrl = "http://www.g.com/ddd";
            item.CacheUrl = "http://www.g.com/cache";
            item.Description = "Who cares?";
            resultItem.Results.Add(item);

            result.Items.Add(resultItem);

            pipeline.OnQueryResultReady(result);

            Thread.Sleep(1000);

            manager.RemoveResultListFromDate(DateTime.Today);
            resultList = manager.GetResultList(DateTime.Today - new TimeSpan(1, 0, 0, 0), DateTime.Today);
            foreach (QueryResult queryResult in resultList)
            {
                Console.WriteLine(queryResult);
            }

            Console.WriteLine("----------------------------------------------");

            manager.RemoveResultListFromDate(DateTime.Today + new TimeSpan(1, 0, 0, 0));
            resultList = manager.GetResultList(DateTime.Today - new TimeSpan(1, 0, 0, 0), DateTime.Today);
            foreach (QueryResult queryResult in resultList)
            {
                Console.WriteLine(queryResult);
            }

            Console.WriteLine("----------------------------------------------");
        }
    }
}
