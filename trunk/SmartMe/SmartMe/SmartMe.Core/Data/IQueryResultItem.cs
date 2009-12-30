using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Data
{
    public enum QueryResultItemType
    {
        SearchEngineResult,
        DictionaryResult,
        SuggestionResult
    }

    public class IQueryResultItem : IMessage
    {
        #region
        internal QueryResultItemType _type;
        MessageType _messageType= MessageType.QueryResultItem;
        #endregion

        #region properties
        public QueryResultItemType ResultType 
        { 
            get
        {
            return _type;
        }
            set
            {
                _type=value;
            }
        }
        #endregion
    
#region IMessage Members

public MessageType  MessageType
{
    get
    {
        return _messageType;
    }
    set{
        _messageType = value;
    }
}

#endregion
}
}
