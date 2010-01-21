using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Record
{
    public interface IRecordFileManager
    {
        /// <summary>
        /// 存到文件中，覆写原有信息
        /// </summary>
        /// <param name="record">存入的记录</param>
        /// <param name="filePath">文件路径</param>
        void SaveToFile(Object record, string filePath);

        /// <summary>
        /// 从文件中读取
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>读取的记录，不考虑文件内容是否正确</returns>
        Object ReadFromFile(string filePath);

        /// <summary>
        /// 产生XML字符串
        /// </summary>
        /// <param name="record">存入的记录</param>
        /// <returns>XML字符串</returns>
        string SaveToXmlString(Object record);

        /// <summary>
        /// 从XML字符串中读取
        /// </summary>
        /// <param name="xmlString">XML字符串</param>
        /// <returns>读取的记录</returns>
        Object ReadFromXmlString(string xmlString);
    }
}
