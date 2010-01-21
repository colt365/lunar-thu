using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Data;
using SmartMe.Web.Search;

namespace SmartMe.Web.Test
{
	public class SogouSearchEngineTest
	{
		public static void Test()
		{
			ISearch engine = new SogouSearchEngine();
			InputQuery query = new InputQuery("");
			query.QueryType = InputQueryType.Text;
			IQueryResultItem item = engine.Search(query);
			Console.WriteLine(item);
		}
	}
}
