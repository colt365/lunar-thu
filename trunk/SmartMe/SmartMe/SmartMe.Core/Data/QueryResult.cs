﻿using System;
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
    public class QueryResult : IMessage
    {
		#region fields
        InputQuery _query;
        ObservableCollection<SearchEngineResult> _items = new ObservableCollection<SearchEngineResult>();
		#endregion
        
        #region constructor
        public QueryResult(InputQuery query)
        {
            _query = query;
        }
        #endregion

        #region properties
        public InputQuery Query
        {
            get { return _query; }
            set { _query = value; }
        }

        public ObservableCollection<SearchEngineResult> Items
        {
            get { return _items; }
        }
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

        #region IMessage Members
        public MessageType Type
        {
            get 
            {
                return MessageType.QueryResult;
            }
        }
        #endregion
        #endregion
    }
}
