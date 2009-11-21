using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Communication
{
    /// <summary>
    /// XML对象，用于序列化
    /// </summary>
    public interface IXMLObject
    {
        #region methods
        /// <summary>
        /// 向一个文件里存入XML对象
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <exception cref="System.IO.IOException">写入失败</exception>
        void SaveToFile(String fileName);

        /// <summary>
        /// 从一个文件中读取XML对象
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <exception cref="System.IO.IOException">读入失败</exception>
        void LoadFromFile(String fileName);

        /// <summary>
        /// 输出到字符串
        /// </summary>
        /// <returns>XML对应的字符串</returns>
        String ToString();
        #endregion
    }
}
