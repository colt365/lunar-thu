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
            InputQuery query = new InputQuery("Ba");
            Console.WriteLine(query.Text);
            //manager.SearchEngineList.Add(new SearchEngine(2000));
           // manager.SearchEngineList.Add(new SearchEngine(1000));
            manager.SearchEngineList.Add(new GoogleSearchEngine());
            manager.SearchEngineList.Add(new BaiduSearchEngine());
            manager.SearchEngineList.Add( new GoogleSuggestion() );
            manager.SearchEngineList.Add(new DictCn());
            manager.Handle(query);
            Thread.Sleep(8000);
        }

        public class SearchEngine : ISearch
        {
            private int _waitTime;
            private static int _count = 0;
            public SearchEngine(int waitTime)
            {
                _waitTime = waitTime;
            }
            #region ISearch Members

            public IQueryResultItem Search ( InputQuery query )
            {
                SearchEngineResult result = new SearchEngineResult();
                result.SearchEngineType = SearchEngineType.Other;
                SearchEngineResult.ResultItem item1 = new SearchEngineResult.ResultItem();
                item1.Title = "INFO";
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

            public void OnResultCompleted(QueryResult result)
            {

                Console.WriteLine("Complete Result!");
                Console.WriteLine( result.Items.Count );
                foreach ( IQueryResultItem item in result.Items)
                {
                    switch(item.ResultType)
                    {
                        case QueryResultItemType.SearchEngineResult:
                            var searchEngineItem = item as SearchEngineResult;
                            if ( searchEngineItem != null && searchEngineItem.Results != null ) // TODO: Bug ASSERT(searchEngineItem != null)
                            {
                                Console.WriteLine( searchEngineItem.ToString() );
                            }
                            break;
                        case QueryResultItemType.DictionaryResult:
                            var dictItem = item as DictResult;
                            if ( dictItem != null && dictItem.SearchUrl != null ) // TODO: Bug ASSERT(searchEngineItem != null)
                            {
                                Console.WriteLine( dictItem.ToString() );
                            }
                            break;
                        case QueryResultItemType.SuggestionResult:
                            var suggestionItem = item as SuggestionResult;
                            if ( suggestionItem != null && suggestionItem.Results != null ) // TODO: Bug ASSERT(searchEngineItem != null)
                            {
                                Console.WriteLine( suggestionItem.ToString() );
                            }
                            break;
                        default:
                            break;

                    }
                }
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
                Debug.Assert(message.MessageType == MessageType.QueryResultItem);
                Console.WriteLine(message);
            }

            #endregion
        }
    }
}
