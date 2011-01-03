using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;

using SmartMe.Core.Data;
using SmartMe.Web.Parse;

namespace SmartMe.Web.Externel
{
    public class ExternelParser : ISearchEngineParser
    {
        #region fields
        WebBrowser _webBrowser;
        
        private string _libPath;
        #endregion

        #region constructors
        public ExternelParser(WebBrowser browser, string scriptPath)
        {
            WebBrowser = browser;
            ScriptPath = scriptPath;
        }
        #endregion

        #region properties
        public string ScriptPath
        {
            get
            {
                return _libPath;
            }
            set
            {
                _libPath = value;
            }
        }

        public WebBrowser WebBrowser
        {
            get
            { 
                return _webBrowser;
            }
            set 
            { 
                _webBrowser = value;
            }
        }
        #endregion

        #region methods



        #region IParser Members

        public SearchEngineResult Parse(string html, Encoding encoding)
        {
            string content = ScriptManager.LoadScript(ScriptPath);
            SearchEngineResult result;
            if (content != "")
            {
                try
                {
                    object o = WebBrowser.InvokeScript("eval", new String[] { content });
                    result = SearchEngineResult.GetSearchEngineResultFromXMLObject(new XMLObject(o.ToString()));
                    return result;
                }
                catch (Exception)
                {
                    // currently do nothing
                    result = new SearchEngineResult();
                }
            }
            else
            {
                result = new SearchEngineResult();
            }
            return result;
        }

        #endregion

        #endregion
    }
}
