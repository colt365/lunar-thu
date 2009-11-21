using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Majestic12;

namespace SmartMe.Web.Parse
{
	interface IParser
	{
		/// <summary>
		/// 解析网页
		/// </summary>
		/// <returns>返回解析的结果</returns>
		public SearchEngineResult Parse(string html, Encoding encoding);
	
	}
}
