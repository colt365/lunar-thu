using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Data;

namespace SmartMe.Core.Record
{
    public interface IRecordManager
    {
        #region methods

        /// <summary>
        /// 增加一条历史记录
        /// </summary>
        /// <param name="result">增加的记录</param>
        /// <param name="date">增加的日期</param>
        void AppendRecord(QueryResult result, DateTime date);

        /// <summary>
        /// 修改一条历史记录
        /// </summary>
        /// <param name="result">修改的记录</param>
        /// <param name="date">修改的日期</param>
        /// <param name="appendIfNotExist">如果不存在，是否添加</param>
        void ModifyRecord(QueryResult result, DateTime date, bool appendIfNotExist);

        /// <summary>
        /// 得到一段时间的历史记录
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>得到的历史记录</returns>
        List<QueryResult> GetResultList(DateTime beginDate, DateTime endDate);

        #endregion
    }
}
