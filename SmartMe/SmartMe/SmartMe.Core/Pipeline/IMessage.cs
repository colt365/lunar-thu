using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Pipeline
{
    public enum MessageType
    {
        InputQuery,
        QueryResult
    }

    /// <summary>
    /// 传递的消息
    /// </summary>
    public interface IMessage
    {
        #region properties
        MessageType Type { get; }
        #endregion

        #region methods
        /// <summary>
        /// 转换消息的内容文本
        /// </summary>
        /// <returns>内容文本</returns>
        String ToString();
        #endregion
    }
}
