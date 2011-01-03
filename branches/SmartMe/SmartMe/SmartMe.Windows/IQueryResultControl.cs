using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Data;

namespace SmartMe.Windows
{
    public interface IQueryResultControl
    {
        string BaseHeader { get; set; }
        string SubHeader { get; set; }
        string Header { get; }
        
        void SetResult(IQueryResultItem queryResult);
        void ClearResult();
    }

    public delegate void DisplayResultHandler(object sender, DisplayResultEventArgs e);
    public class DisplayResultEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}
