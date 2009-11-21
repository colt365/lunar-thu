using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;
using Majestic12;


namespace SmartMe.Web.Search
{
    /// <summary>
    /// 搜索引擎的抽象
    /// </summary>
    public interface ISearchEngine
    {
        /// <summary>
        /// 搜索文本
        /// </summary>
        /// <param name="query">搜索的文本</param>
        /// <returns>返回结果</returns>
        public SearchEngineResult Search(InputQuery query);
    }

}
