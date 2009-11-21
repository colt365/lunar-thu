using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Web.Search
{
	class SearchEngine:ISearchEngine
	{

		#region ISearchEngine Members

		public SmartMe.Core.Data.SearchEngineResult Search(SmartMe.Core.Data.InputQuery query)
		{
			if (query.QueryType != SmartMe.Core.Data.InputQueryType.Text)
			{
				return null;
			}

			return null;
		}

		#endregion
	}
}
