using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;
using SmartMe.Core.Pipeline;
using SmartMe.Web.Search;
using System.Diagnostics;
using System.Threading;

namespace SmartMe.Web.Test
{
    public class WebResourceManagerTester
    {
        public static void Test()
        {
            Pipeline pipeline = new Pipeline();
            pipeline.QueryResultItemSubscriberManager.AddSubscriber(new ResultItemSubscriber(500));
            WebResourceManager manager = new WebResourceManager(pipeline, new QueryResultHandler());
            InputQuery query = new InputQuery("LinTian");
            Console.WriteLine(query.Text);
            manager.SearchEngineList.Add(new SearchEngine(2000));
            manager.SearchEngineList.Add(new SearchEngine(1000));
            manager.Handle(query);
            Thread.Sleep(5000);
        }

        public class SearchEngine : ISearchEngine
        {
            private int _waitTime;
            private static int _count = 0;
            public SearchEngine(int waitTime)
            {
                _waitTime = waitTime;
            }
            #region ISearchEngine Members

            public SearchEngineResult Search(InputQuery query)
            {
                SearchEngineResult result = new SearchEngineResult();
                SearchEngineResult.ResultItem item1 = new SearchEngineResult.ResultItem();
                item1.Title = "INFO";
                item1.Source = "TSINGUA";
                item1.CacheUrl = "http://aaa.www.com/";
                item1.Description = query + " " + _count + " Done.";
                item1.SimilarUrl = "http://similar.www.com/";
                item1.Url = "http://info.tsinghua.edu.cn";
                result.Results.Add(item1);
                _count++;
                Thread.Sleep(_waitTime);
                return result;
            }

            #endregion
        }

        public class QueryResultHandler : IQueryResultHandler
        {
            #region IQueryResultHandler Members

            public void OnResultNew(QueryResult result)
            {
                Console.WriteLine("New Result!");
            }

            public void OnResultUpdate(QueryResult result)
            {
                Console.WriteLine(result.ToString() + "\nUpdate Result!");
            }

            public void OnResultDeprecated(QueryResult result)
            {
                Console.WriteLine("Deprecate Result!");
            }

            #endregion
        }

        public class ResultItemSubscriber : ISubScriber
        {
            private int _waitTime;
            public ResultItemSubscriber(int waitTime)
            {
                _waitTime = waitTime;
            }
            #region ISubScriber Members

            public void Handle(IMessage message)
            {
                Thread.Sleep(_waitTime);
                Debug.Assert(message.Type == MessageType.QueryResultItem);
                Console.WriteLine(message);
            }

            #endregion
        }
    }
}
