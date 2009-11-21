using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;

namespace SmartMe.Core.Pipeline
{
    /// <summary>
    /// 这个流水线把不同的部分组合到一起
    /// </summary>
    public class Pipeline : IPipeline
    {
        #region fields
        private ISubScriberManager _inputTextSubscriberManager = new SubscriberManager();
        private ISubScriberManager _queryResultSubscriberManager = new SubscriberManager();
        private List<Pipe> _inputTextPipes = new List<Pipe>();
        private List<Pipe> _queryResultPipes = new List<Pipe>();
        #endregion

        #region constructors
        public Pipeline()
        {
        }
        #endregion

        #region properties
        /// <summary>
        /// 订阅了输入消息的集合
        /// </summary>
        public ISubScriberManager InputTextSubscriberManager
        {
            get
            {
                return _inputTextSubscriberManager;
            }
        }

        /// <summary>
        /// 订阅了搜索结果的集合
        /// </summary>
        public ISubScriberManager QueryResultSubscriberManager
        {
            get
            {
                return _queryResultSubscriberManager;
            }
        }
        #endregion

        #region methods
        public void OnInputTextReady(InputQuery text)
        {
            List<Pipe> newPipes = _inputTextSubscriberManager.NotifyAll(text);
            // Assert(newPipes != null)
            _inputTextPipes.AddRange(newPipes);
        }

        public void OnQueryResultNew(QueryResult result)
        {
            List<Pipe> newPipes = _queryResultSubscriberManager.NotifyAll(result);
            // Assert(newPipes != null)
            _queryResultPipes.AddRange(newPipes);
        }

        public void OnQueryResultUpdate(QueryResult result)
        {
            List<Pipe> newPipes = _queryResultSubscriberManager.NotifyAll(result);
            // Assert(newPipes != null)
            _queryResultPipes.AddRange(newPipes);
        }

        public void OnInputTextCanceled(InputQuery text)
        {
            foreach (Pipe pipe in _inputTextPipes)
            {
                if (pipe.Message == text)
                {
                    pipe.CancelMessage();
                }
            }
        }

        public void OnQueryResultCanceled(QueryResult result)
        {
            foreach (Pipe pipe in _queryResultPipes)
            {
                if (pipe.Message == result)
                {
                    pipe.CancelMessage();
                }
            }
        }
        #endregion
    }
}
