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
        void OnInputTextReady(InputQuery text);

        /// <summary>
        /// 准备好了新的查询项
        /// </summary>
        /// <param name="item"></param>
        void OnQueryResultItemReady(IQueryResultItem item);

        /// <summary>
        /// 取消了输入数据
        /// </summary>
        /// <param name="text"></param>
        void OnInputTextCanceled(InputQuery text);

        /// <summary>
        /// 取消了查询结果
        /// </summary>
        /// <param name="result"></param>
        void OnQueryResultCanceled(QueryResult result);
        #endregion


    }
}
