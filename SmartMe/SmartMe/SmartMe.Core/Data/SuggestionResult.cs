using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Data
{
    public enum SuggestionType
    {
        Google,
        Baidu,
        Other
    }

    [Serializable]
    public class SuggestionResult:IQueryResultItem
    {
        public SuggestionResult()
        {
            _type = QueryResultItemType.SuggestionResult;
        }

        #region nested
        public class ResultItem
        {
            #region fields
            private string _suggestion = string.Empty;

            private string _number = string.Empty;

            private string _index = string.Empty;

           
            #endregion

            #region properties
            public string Suggestion
            {
                get
                {
                    return _suggestion;
                }
                set
                {
                    _suggestion = value;
                }
            }

            public string Number
            {
                get
                {
                    return _number;
                }
                set
                {
                    _number = value;
                }
            }

            public string Index
            {
                get
                {
                    return _index;
                }
                set
                {
                    _index = value;
                }
            }

   
            #endregion

            #region methods
            public override string ToString ( )
            {
                return "Suggestion:" + _suggestion + "\n" +
                       "Number:" + _number + "\n" +
                       "Index:" + _index ;
            }
            #endregion
        }
        #endregion

        

        #region IMessage Members

        public SmartMe.Core.Pipeline.MessageType MessageType
        {
            get
            {
                return  SmartMe.Core.Pipeline.MessageType.QueryResult;
            }
        }

        #endregion


        #region fields
        private List<ResultItem> _results = new List<ResultItem>();
        private string _searchUrl = string.Empty;
        private SuggestionType _suggestionType = SuggestionType.Other;
        
        #endregion


        #region properties
       

      public SuggestionType SuggestionType
      {
          get
          {
              return _suggestionType;
          }
          set
          {
              _suggestionType = value;
          }
      }

        

        public List<ResultItem> Results
        {
            get
            {
                return _results;
            }
            set
            {
                _results = value;
            }
        }
        public string SearchUrl
        {
            get
            {
                return _searchUrl;
            }
            set
            {
                _searchUrl = value;
            }
        }

        #endregion


        #region methods
        public override string ToString ( )
        {
            StringBuilder stringBuilder = new StringBuilder( Results.Count.ToString() );
            foreach ( ResultItem item in Results )
            {
                stringBuilder.Append( "\n" + item.ToString() );
            }
            return stringBuilder.ToString();
        }
        #endregion

    }
}
