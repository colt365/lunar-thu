using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Data
{
    public class SearchEngineResult : IQueryResultItem
    {
        #region fields
        private List<ResultItem> _results = new List<ResultItem>();
        #endregion

        #region nested
        public struct ResultItem
        {
            #region fields
            string Title;
            string Url;
            string Description;
            string CacheUrl;
            string SimilarUrl;
            string Source;
            #endregion
        }
        #endregion

        #region properties
        QueryResultItemType IQueryResultItem.Type
        {
            get 
            {
                return QueryResultItemType.SearchEngineResult;
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
        #endregion
    }
}
