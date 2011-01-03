using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using SmartMe.Core.Pipeline;
using System.Xml.Serialization;

namespace SmartMe.Core.Data
{
    /// <summary>
    /// 搜索返回的结果
    /// </summary>
    public class QueryResult : Message
    {
		#region fields
        private InputQuery _query;
        private List<SearchEngineResult> _searchEngineResultItems = new List<SearchEngineResult>();
        private List<SuggestionResult> _suggestionResultItems = new List<SuggestionResult>();
        private List<DictResult> _dictResultItems = new List<DictResult>();
        private List<IQueryResultItem> _items = new List<IQueryResultItem>();
        #endregion
        
        #region constructor

        public QueryResult()
        {
            // do nothing
        }

        public QueryResult(InputQuery query)
        {
            _query = query;
        }

        #endregion

        #region properties

        public InputQuery Query
        {
            get
            {
                return _query;
            }
            set 
            {
                _query = value;
            }
        }

        [XmlIgnore]
        public List<IQueryResultItem> Items
        {
            get {
                _items.Clear();
                foreach ( IQueryResultItem item in _searchEngineResultItems )
                {
                    _items.Add( item );
                }
                
                foreach ( IQueryResultItem item in _suggestionResultItems )
                {
                    _items.Add( item );
                }

                
                foreach ( IQueryResultItem item in _dictResultItems )
                {
                    _items.Add( item );
                }

                return _items;
            }
        }

        public List<SearchEngineResult> SearchEngineResultItems
        {
            get
            {
                return _searchEngineResultItems;
            }
        }

        [XmlIgnore]
        public List<SuggestionResult> SuggestionResultItems
        {
            get
            {
                return _suggestionResultItems;
            }
        }


        public List<DictResult> DictResultItems
        {
            get
            {
                return _dictResultItems;
            }
        }

        public DateTime LastUpdateTime
        {
            get;
            set;
        }


        #region IMessage Members

        public override MessageType MessageType
        {
            get
            {
                return MessageType.QueryResult;
            }
        }

        #endregion

        #endregion

        #region methods

        public bool IsEmpty()
        {
            
            foreach (SearchEngineResult r in _searchEngineResultItems)
            {
                if(r.Items.Count!=0)
                {
                    return false;
                }
            }
            foreach (DictResult  r in _dictResultItems)
            {
                if (r.Word!=string.Empty)
                {
                    return false;
                }
            }
            foreach (SuggestionResult r in _suggestionResultItems)
            {
                if (r.Items.Count!=0)
                {
                    return false;
                }
            }
            return true;
           
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(_query.ToString());
            stringBuilder.Append( "\n" + _searchEngineResultItems.Count );
            foreach ( IQueryResultItem item in _searchEngineResultItems )
            {
                stringBuilder.Append("\n").Append(item);
            }

            stringBuilder.Append( "\n" + _suggestionResultItems.Count );
            foreach ( IQueryResultItem item in _suggestionResultItems )
            {
                stringBuilder.Append( "\n" ).Append( item );
            }

            stringBuilder.Append( "\n" + _dictResultItems.Count );
            foreach ( IQueryResultItem item in _dictResultItems )
            {
                stringBuilder.Append( "\n" ).Append( item );
            }


            return stringBuilder.ToString();
        }

        public override bool Equals(object obj)
        {
            QueryResult result = obj as QueryResult;
            if (result != null)
            {
                return (result.ToString() == this.ToString());
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #endregion
    }
}
