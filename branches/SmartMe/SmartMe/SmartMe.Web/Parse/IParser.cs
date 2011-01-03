using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Majestic12;
using SmartMe.Core.Data;

namespace SmartMe.Web.Parse
{
    public interface ISearchEngineParser
	{
		/// <summary>
		/// 解析网页
		/// </summary>
		/// <returns>返回解析的结果</returns>
        SearchEngineResult Parse(string html, Encoding encoding);
	}

    public interface IDictParser
    {
        /// <summary>
        /// 解析网页
        /// </summary>
        /// <returns>返回解析的结果</returns>
        DictResult Parse(string html, Encoding encoding);
    }

    public interface ISuggestionParser
    {
        /// <summary>
        /// 解析网页
        /// </summary>
        /// <returns>返回解析的结果</returns>
        SuggestionResult Parse(string html, Encoding encoding);
    }
}
