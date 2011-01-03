﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using SmartMe.Core.Data;
using SmartMe.Web.Crawl;
using SmartMe.Web.Parse;
using System.Diagnostics;

namespace SmartMe.Web.Search
{
    public abstract class SearchEngine : ISearch
    {
        protected string _queryFormat = null;
        public string QueryFormat
        {
            get { return _queryFormat; }
            internal set { _queryFormat = value; }
        }
        protected Encoding _encoding = null;
        public Encoding Encoding
        {
            get { return _encoding; }
            internal set { _encoding = value; }
        }
        protected SearchEngineType _searchEngineType = SearchEngineType.Other;
        public SearchEngineType SearchEngineType
        {
            get { return _searchEngineType; }
            internal set { _searchEngineType = value; }
        }
        protected ISearchEngineParser _parser = null;
        public ISearchEngineParser Parser
        {
            get { return _parser; }
            internal set { _parser = value; }
        }


        protected string CreateSearchUrl(string text)
        {
            Debug.Assert(QueryFormat != null);
            Debug.Assert(Encoding != null);

            string queryText = HttpUtility.UrlEncode(text, Encoding);
            string url = string.Format(QueryFormat, queryText);
            return url;
        }

        public virtual IQueryResultItem Search(InputQuery query)
        {
            Debug.Assert(QueryFormat != null);
            Debug.Assert(Encoding != null);
            Debug.Assert(Parser != null);

            // Crawler:
            SearchEngineResult emptyResult = new SearchEngineResult();
            emptyResult.SearchEngineType = this.SearchEngineType;
            string url = CreateSearchUrl(query.Text);

            emptyResult.SearchUrl = url;
            if (query == null || string.IsNullOrEmpty(query.Text))
            {
                return emptyResult;
            }
            string html = Crawler.Crawl(url, Encoding);

            // Parse:
            if (string.IsNullOrEmpty(html))
            {
                return emptyResult;
            }
            ISearchEngineParser parser = Parser;
            SearchEngineResult result = parser.Parse(html, Encoding);
            result.SearchUrl = url;

            return result;
        }
    }
}
