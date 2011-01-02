using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SmartMe.Web.Externel
{
    /// <summary>
    /// 管理Javascript(或其他脚本)的控件
    /// </summary>
    internal class ScriptManager
    {
        #region methods

        /// <summary>
        /// 读取脚本
        /// </summary>
        /// <param name="scriptPath">脚本路径</param>
        /// <returns>读取的文字</returns>
        internal static string LoadScript(string scriptPath)
        {
            string content = "";
            using (StreamReader streamReader = new StreamReader(
                                    new FileStream(scriptPath, FileMode.Open)))
            {
                content = streamReader.ReadToEnd();
            }
            return content;
        }

        #endregion
    }
}
