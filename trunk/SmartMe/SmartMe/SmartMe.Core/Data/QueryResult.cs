using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		List<IQueryResultItem> _items;
		#endregion		

        #region methods
        #endregion

        #region IMessage Members

        public MessageType Type
        {
            get { return MessageType.QueryResult; }
        }

        #endregion
    }
}
