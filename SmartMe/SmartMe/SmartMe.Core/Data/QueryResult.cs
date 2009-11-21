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
		
		string _title;
		string _url;
		string _description;
		string _cacheUrl;
		string _similarUrl;
		string _source;
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
