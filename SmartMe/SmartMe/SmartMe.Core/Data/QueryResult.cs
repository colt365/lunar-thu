using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Data
{

	
	public class ResultItem
	{
		#region fields
		
		string title;
		string url;
		string description;
		string cacheUrl;
		string similarUrl;
		string source;
		#endregion
	}

    /// <summary>
    /// 搜索返回的结果
    /// </summary>
    public class QueryResult : IMessage
    {

		#region fields
		string _query;
		List<ResultItem> _items;
		#endregion 
	
		

        #region methods
        #region IMessage Members
        string IMessage.ToString()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
