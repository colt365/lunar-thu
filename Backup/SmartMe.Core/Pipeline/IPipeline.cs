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
        /// <param name="text">输入</param>
        void OnInputTextReady(InputQuery text);

        /// <summary>
        /// 准备好了新的查询项
        /// </summary>
        /// <param name="item">查询项</param>
        void OnQueryResultItemReady(IQueryResultItem item);

        /// <summary>
        /// 取消了输入数据
        /// </summary>
        /// <param name="text">输入</param>
        void OnInputTextCanceled(InputQuery text);

        /// <summary>
        /// 取消了查询结果
        /// </summary>
        /// <param name="result">结果</param>
        void OnQueryResultCanceled(QueryResult result);

        /// <summary>
        /// 准备好查询结果
        /// </summary>
        /// <param name="result">结果</param>
        void OnQueryResultReady(QueryResult result);

        /// <summary>
        /// 取消了查询项
        /// </summary>
        /// <param name="item">查询项</param>
        void OnQueryResultItemCanceled(IQueryResultItem item);

        #endregion


    }
}
