using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Input
{
    /// <summary>
    /// 查询文本获得器
    /// </summary>
    public interface ITextFetcher
    {
        #region methods
        /// <summary>
        /// 得到输入的文字
        /// </summary>
        /// <returns> 输入的文字 </returns>
        public InputText fetchText();        
        #endregion
    }
}
