using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Data
{
    /// <summary>
    /// 搜索返回的结果
    /// </summary>
    public class QueryResult : Message
    {
		#region fields
        private InputQuery _query;
        private List<SearchEngineResult> _items = new List<SearchEngineResult>();
		#endregion
        
        #region constructor

        public QueryResult()
        {
            // do nothing
        }

        public QueryResult(InputQuery query)
        {
            _query = query;
        }

        #endregion

        #region properties

        public InputQuery Query
        {
            get
            {
                return _query;
            }
            set 
            {
                _query = value;
            }
        }

        public List<SearchEngineResult> Items
        {
            get { return _items; }
        }


        #region IMessage Members

        public override MessageType MessageType
        {
            get
            {
                return MessageType.QueryResult;
            }
        }

        #endregion

        #endregion

        #region methods

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(_query.ToString());
            stringBuilder.Append("\n" + Items.Count);
            foreach (IQueryResultItem item in _items)
            {
                stringBuilder.Append("\n").Append(item);
            }
            return stringBuilder.ToString();
        }

        #endregion
    }
}
