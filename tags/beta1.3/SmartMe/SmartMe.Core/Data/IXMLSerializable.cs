using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Data
{
    /// <summary>
    /// 可以被XML序列化的对象
    /// </summary>
    public interface IXMLSerializable
    {
        #region methods
        /// <summary>
        /// 输出到XML对象
        /// </summary>
        /// <returns>XML对象</returns>
        XMLObject ToXMLObject();

        /// <summary>
        /// 从XML对象读入
        /// </summary>
        /// <param name="xmlObject">读入的XML对象</param>
        void FromXMLObject(XMLObject xmlObject);
        #endregion
    }
}
