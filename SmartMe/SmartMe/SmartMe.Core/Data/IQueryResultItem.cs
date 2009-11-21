using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Data
{
    public enum QueryResultItemType
    {
        SearchEngineResult
    }

    public interface IQueryResultItem : IMessage
    {
        #region properties
        QueryResultItemType ResultType { get; }
        #endregion
    }
}
