using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;
using SmartMe.Core.Pipeline;
using SmartMe.Web.Search;
using System.Threading;

namespace SmartMe.Web
{
    public enum HandleDeprecateQueryOption
    {
        Drop,
        Reserve
    }

    public class WebResourceManager : ISubScriber
    {
        #region fields
        /// <summary>
        /// 搜索引擎列表
        /// </summary>
        private List<ISearchEngine> _searchEngineList = new List<ISearchEngine>();

        /// <summary>
        /// 流水线
        /// </summary>
        private Pipeline _pipeline;

        /// <summary>
        /// 搜索返回的结果
        /// </summary>
        private QueryResult _result;

        /// <summary>
        /// 处理并显示搜索结果的处理函数
        /// </summary>
        private IQueryResultHandler _handler;

        /// <summary>
        /// 是否取消过期的查询
        /// </summary>
        private HandleDeprecateQueryOption _deprecateQueryOption = HandleDeprecateQueryOption.Reserve;
        #endregion

        #region constructor
        /// <summary>
        /// 构造一个WebResourceManager，如果pipeline = null，抛出ArgumentNullException
        /// </summary>
        /// <param name="pipeline">流水线，不能是null</param>
        /// <param name="handler">处理搜索结果并显示的模块，不能是null</param>
        public WebResourceManager(Pipeline pipeline, IQueryResultHandler handler)
        {            
            if (pipeline == null)
            {
                throw new ArgumentNullException("pipeline");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            _pipeline = pipeline;
            _handler = handler;
        }
        #endregion

        #region nested
        private class SearchAndReturnPipe
        {
            #region fields
            private ISearchEngine _searchEngine;
            private InputQuery _inputQuery;
            private WebResourceManager _parent;
            private Thread _thread;
            #endregion

            #region constructor
            public SearchAndReturnPipe(WebResourceManager parent,
                ISearchEngine engine, InputQuery query, Pipeline pipeline)
            {
                // Assert(engine!= null && query != null)
                _parent = parent;
                _searchEngine = engine;
                _inputQuery = query;
                _thread = new Thread(new ThreadStart(SearchAndReturn));
                _thread.Start();
            }
            #endregion

            #region methods
            public void CancelQuery()
            {
                if (_thread != null)
                {
                    _thread.Abort();
                    _thread = null;
                }
            }

            private void SearchAndReturn()
            {
                SearchEngineResult result = _searchEngine.Search(_inputQuery);
                _parent.OnSearchResultDone(result, _inputQuery);
            }
            #endregion
        }
        #endregion

        #region properties
        public HandleDeprecateQueryOption DeprecateQueryOption
        {
            get { return _deprecateQueryOption; }
            set { _deprecateQueryOption = value; }
        }

        public List<ISearchEngine> SearchEngineList
        {
            get { return _searchEngineList; }
            set { _searchEngineList = value; }
        }
        #endregion

        #region methods

        public bool AddSearchEngine(ISearchEngine searchEngine)
        {
            if (searchEngine == null)
            {
                return false;
            }
            _searchEngineList.Add(searchEngine);
            return true;
        }

        public bool RemoveSearchEngine(ISearchEngine searchEngine)
        {
            return _searchEngineList.Remove(searchEngine);
        }

        public bool RemoveSearchEngineAt(int position)
        {
            if (position < 0 || position >= _searchEngineList.Count)
            {
                return false;
            }
            _searchEngineList.RemoveAt(position);
            return true;
        }

        private void OnSearchResultDone(SearchEngineResult result, InputQuery query)
        {
            if (query == _result.Query)
            {
                lock (_result)
                {
                    _result.Items.Add(result);
                    _handler.OnResultUpdate(_result);
                    if (_result.Items.Count == _searchEngineList.Count)
                    {
                        _handler.OnResultCompleted(_result);
                    }

                }
            }
        }

        private void HandleDeprecateMessage(InputQuery message)
        {
            if (_deprecateQueryOption == HandleDeprecateQueryOption.Drop)
            {
                _pipeline.OnInputTextCanceled(message);
            }
        }

        #region ISubScriber Members
        public void Handle(IMessage message)
        {
            if (message.Type == MessageType.InputQuery)
            {
                InputQuery query = (InputQuery)message;
                if (_result != null && _result.Query == query)
                {
                    return;
                }
                // Kill current queries
                if (_result != null)
                {
                    HandleDeprecateMessage(_result.Query);
                }

                // Notice the handler
                _result = new QueryResult(query);
                _handler.OnResultNew(_result);

                // Generate new queries               
                foreach (ISearchEngine engine in _searchEngineList)
                {
                    SearchAndReturnPipe pipe = new SearchAndReturnPipe(this, engine, query, _pipeline);
                }
            }
        }
        #endregion
        #endregion
    }
}
