using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;

namespace SmartMe.Input
{
    public interface IInputQueryProcessor
    {
        #region methods
        /// <summary>
        /// 获得输入的分类
        /// </summary>
        /// <param name="rawText">输入的文本</param>
        /// <returns>分类好的查询结果</returns>
        InputQuery GetQuery(string rawText);
        #endregion
    }
}
