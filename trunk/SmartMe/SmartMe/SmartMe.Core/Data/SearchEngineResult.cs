using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Data
{
    /// <summary>
    /// 搜索引擎类型
    /// </summary>
    public enum SearchEngineType
    {
        Google,
        Baidu,
        Sougou,
        Wikipedia,
        Other
    }

    public class SearchEngineResult : IQueryResultItem
    {
        #region fields
        private List<ResultItem> _results = new List<ResultItem>();
        private string _searchUrl;
        private SearchEngineType _searchEngine;
        #endregion

        #region nested
        public class ResultItem
        {
            #region fields
            private string _title="";            

            private string _url="";

            private string _description="";

            private string _cacheUrl="";

            private string _similarUrl="";
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
            #endregion

            #region methods
            public override string ToString()
            {
                return "Title:" + _title + "\n" +
                       "Url:" + _url + "\n" +
                       "Description:" + _description + "\n" +
                       "CacheUrl:" + _cacheUrl + "\n" +
                       "SimilarUrl:" + _similarUrl;
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

        public MessageType MessageType
        {
            get
            {
                return MessageType.QueryResultItem;
            }
        }

        public SearchEngineType SearchEngineType
        {
            get
            {
                return _searchEngine;
            }
            set
            {
                _searchEngine = value;
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
                _searchUrl=value;
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
