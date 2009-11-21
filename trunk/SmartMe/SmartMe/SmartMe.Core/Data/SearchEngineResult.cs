using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Data
{
    public class SearchEngineResult : IQueryResultItem
    {
        #region fields
        private List<ResultItem> _results = new List<ResultItem>();
        #endregion

        #region nested
        public class ResultItem
        {
            #region fields
            string _title;            

            string _url;

            string _description;

            string _cacheUrl;

            string _similarUrl;

            string _source;
            #endregion

            #region properties
            public string Title
            {
                get { return _title; }
                set { _title = value; }
            }

            public string Url
            {
                get { return _url; }
                set { _url = value; }
            }

            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }

            public string CacheUrl
            {
                get { return _cacheUrl; }
                set { _cacheUrl = value; }
            }

            public string SimilarUrl
            {
                get { return _similarUrl; }
                set { _similarUrl = value; }
            }

            public string Source
            {
                get { return _source; }
                set { _source = value; }
            }
            #endregion

            #region methods
            public override string ToString()
            {
                return "Title:" + _title + "\n" +
                       "Url:" + _url + "\n" +
                       "Description:" + _description + "\n" +
                       "CacheUrl:" + _cacheUrl + "\n" +
                       "SimilarUrl:" + _similarUrl + "\n" +
                       "Source:" + _source;
            }
            #endregion
        }
        #endregion

        #region properties
        public QueryResultItemType ResultType
        {
            get 
            {
                return QueryResultItemType.SearchEngineResult;
            }
        }

        public MessageType Type
        {
            get
            {
                return MessageType.QueryResultItem;
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
        #endregion

        #region methods
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(Results.Count.ToString());
            foreach (ResultItem item in Results)
            {
                stringBuilder.Append("\n" + item.ToString());
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}
