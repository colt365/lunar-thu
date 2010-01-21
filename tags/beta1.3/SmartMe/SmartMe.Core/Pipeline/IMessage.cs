using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Data;

namespace SmartMe.Core.Pipeline
{
    /// <summary>
    /// 消息的类型
    /// </summary>
    public enum MessageType
    {
        InputQuery,
        QueryResultItem,
        QueryResult
    }

    /// <summary>
    /// 传递的消息
    /// </summary>
    public interface IMessage
    {
        #region properties
        MessageType MessageType { get; }
        #endregion

        #region methods
        #endregion
    }
}
