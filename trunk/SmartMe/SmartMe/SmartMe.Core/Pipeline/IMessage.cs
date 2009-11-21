using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Pipeline
{
    /// <summary>
    /// 传递的消息
    /// </summary>
    public interface IMessage
    {
        #region methods
        /// <summary>
        /// 转换消息的内容文本
        /// </summary>
        /// <returns>内容文本</returns>
        String ToString();
        #endregion
    }
}
