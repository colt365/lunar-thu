using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Data
{
    /// <summary>
    /// XML对象，用于序列化
    /// </summary>
    public interface IXMLObject
    {
        #region methods
        /// <summary>
        /// 输出到字符串
        /// </summary>
        /// <returns>XML对应的字符串</returns>
        String ToString();
        #endregion
    }
}
