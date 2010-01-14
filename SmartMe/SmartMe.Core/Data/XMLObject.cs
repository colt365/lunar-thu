using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Data
{
    /// <summary>
    /// XML对象，用于序列化
    /// </summary>
    public class XMLObject
    {
        #region fields
        private string _xmlString;
        #endregion

        #region constructors
        public XMLObject(string xmlString)
        {
            if (xmlString != null)
            {
                _xmlString = xmlString;
            }
            else
            {
                _xmlString = string.Empty;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// 输出到字符串
        /// </summary>
        /// <returns>XML对应的字符串</returns>
        public override String ToString()
        {
            return _xmlString;
        }
        #endregion
    }
}
