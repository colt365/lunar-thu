using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Data
{
    public enum QueryResultItemType
    {
        SearchEngineResult
    }

    public interface IQueryResultItem
    {
        #region properties
        public QueryResultItemType Type { get; }
        #endregion
    }
}
