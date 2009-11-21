using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Pipeline
{
    /// <summary>
    /// ISubScriber代表可以接收别人消息并进行处理的接口
    /// </summary>
    public interface ISubScriber
    {
        #region methods
        /// <summary>
        /// 处理收到的相应消息
        /// </summary>
        /// <param name="message"></param>
        void Handle(IMessage message);
        #endregion
    }
}
