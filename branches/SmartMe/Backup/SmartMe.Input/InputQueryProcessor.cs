using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;

namespace SmartMe.Input
{
    public class InputQueryProcessor : IInputQueryProcessor
    {
        #region methods
        #region IInputQueryProcessor Members

        public InputQuery GetQuery(string rawText)
        {
            InputQuery query = new InputQuery(rawText);
            return query;
        }

        #endregion
        #endregion
    }
}
