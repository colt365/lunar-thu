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
        void appendRecord(QueryResult result, DateTime date);
        void modifyRecord(QueryResult result, DateTime date);
        List<QueryResult> getResultList(DateTime beginDate, DateTime endDate);
        #endregion
    }
}
