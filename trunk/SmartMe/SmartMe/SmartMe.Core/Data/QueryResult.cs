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
        #region methods
        #region IMessage Members
        string IMessage.ToString()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
