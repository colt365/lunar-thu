using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

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

    /// <summary>
    /// 从一个搜索引擎返回的结果
    /// </summary>
    [Serializable]
    public class SearchEngineResult : IQueryResultItem, IXMLSerializable
    {
        public SearchEngineResult()
        {
            _type = QueryResultItemType.SearchEngineResult;
        }

        #region fields
        private List<ResultItem> _items = new List<ResultItem>();
        private string _searchUrl = string.Empty;
        private SearchEngineType _searchEngine = SearchEngineType.Other;
        private QueryResultItemType _resultType;
        private MessageType _messageType;
        #endregion

        #region nested
        [Serializable]
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
        public new QueryResultItemType ResultType
        {
            get 
            {
                return QueryResultItemType.SearchEngineResult;
            }
            set
            {
                _resultType=value;
            }
        }

        public new MessageType MessageType
        {
            get
            {
                return MessageType.QueryResultItem;
            }
            set{
                _messageType=value;
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

        public List<ResultItem> Items
        {
            get 
            {
                return _items;
            }
            set 
            { 
                _items = value;
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
            StringBuilder stringBuilder = new StringBuilder(Items.Count.ToString());
            foreach (ResultItem item in Items)
            {
                stringBuilder.Append("\n" + item.ToString());
            }
            return stringBuilder.ToString();
        }

        public static SearchEngineResult GetSearchEngineResultFromXMLObject(XMLObject xmlObject)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SearchEngineResult));
            StringReader xmlStringReader = new StringReader(xmlObject.ToString());
            SearchEngineResult result = xmlSerializer.Deserialize(xmlStringReader) as SearchEngineResult;
            xmlStringReader.Close();
            if (result == null)
            {
                result = new SearchEngineResult();
            }
            return result;
        }

        #region IXMLSerializable Members

        public XMLObject ToXMLObject()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SearchEngineResult));
            StringBuilder xmlString = new StringBuilder();
            StringWriter xmlStringWriter = new StringWriter(xmlString);
            xmlSerializer.Serialize(xmlStringWriter, this);
            xmlStringWriter.Close();
            return new XMLObject(xmlString.ToString());
        }

        public void FromXMLObject(XMLObject xmlObject)
        {
            SearchEngineResult result = GetSearchEngineResultFromXMLObject(xmlObject);
            this._searchEngine = result._searchEngine;
            this._searchUrl = result._searchUrl;
            this._items = result._items;
        }

        #endregion

        #endregion
    }
}
