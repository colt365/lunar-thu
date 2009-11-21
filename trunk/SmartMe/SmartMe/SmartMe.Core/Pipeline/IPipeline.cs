using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;

namespace SmartMe.Core.Pipeline
{
    /// <summary>
    /// 这个流水线把不同的部分组合到一起
    /// </summary>
    public interface IPipeline
    {
        #region methods
        /// <summary>
        /// 准备好了输入数据
        /// </summary>
        /// <param name="text"></param>
        void onInputTextReady(InputText text);

        /// <summary>
        /// 准备好了查询结果
        /// </summary>
        /// <param name="text"></param>
        void onQueryResultReady(QueryResult result);

        /// <summary>
        /// 取消了输入数据
        /// </summary>
        /// <param name="text"></param>
        void onInputTextCanceled(InputText text);

        /// <summary>
        /// 取消了查询结果
        /// </summary>
        /// <param name="result"></param>
        void onQueryResultReade(QueryResult result);
        #endregion


    }
}
