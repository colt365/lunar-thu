using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Majestic12;

namespace SmartMe.Web.Parse
{
    public interface IParser
	{
		/// <summary>
		/// 解析网页
		/// </summary>
		/// <returns>返回解析的结果</returns>
		SmartMe.Core.Data.SearchEngineResult Parse(string html, Encoding encoding);
	}
}
