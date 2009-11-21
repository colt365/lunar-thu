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
        private List<SearchEngineResult> _results = new List<SearchEngineResult>();

        /// <summary>
        /// 当前的搜索
        /// </summary>
        private InputQuery _currentQuery;

        private HandleDeprecateQueryOption _deprecateQueryOption = HandleDeprecateQueryOption.Drop;
        #endregion

        #region constructor
        /// <summary>
        /// 构造一个WebResourceManager，如果pipeline = null，抛出ArgumentNullException
        /// </summary>
        /// <param name="pipeline">流水线，不能是null</param>
        public WebResourceManager(Pipeline pipeline)
        {            
            if (pipeline == null)
            {
                throw new ArgumentNullException("pipeline");
            }
            _pipeline = pipeline;
        }
        #endregion

        #region nested
        private class SearchAndReturnPipe
        {
            #region fields
            private ISearchEngine _searchEngine;
            private InputQuery _inputQuery;
            private Pipeline _pipeline;
            private Thread _thread;
            #endregion

            #region constructor
            public SearchAndReturnPipe(ISearchEngine engine, InputQuery query, Pipeline pipeline)
            {
                // Assert(engine!= null && query != null)
                _searchEngine = engine;
                _inputQuery = query;
                _pipeline = pipeline;
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
                _pipeline.OnQueryResultItemReady(result);
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
                if (_currentQuery == query)
                {
                    return;
                }
                // Kill current queries
                HandleDeprecateMessage(_currentQuery);

                // Generate new queries
                _currentQuery = query;                
                foreach (ISearchEngine engine in _searchEngineList)
                {
                    SearchAndReturnPipe pipe = new SearchAndReturnPipe(engine, query, _pipeline);
                }
            }
        }
        #endregion
        #endregion
    }
}
