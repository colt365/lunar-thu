using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;

namespace SmartMe.Output
{
    public interface IQueryResultHandler
    {
        void OnResultNew(QueryResult result);
        void OnResultUpdate(QueryResult result);
        void OnResultDeprecated(QueryResult result);
    }
}
