using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Controls;
using System.Windows.Navigation;

using SmartMe.Core.Data;
using SmartMe.Web.Search;
using System.Threading;

namespace SmartMe.Web.Externel
{
    /// <summary>
    /// 从外部脚本进行处理的引擎
    /// </summary>
    public class ExternelSearchEngine : ISearch
    {
        #region fields

        private WebBrowser _webBrowser;

        private string _urlPattern;

        private ExternelParser _parser;

        private List<string> _libraryPath = new List<string>();

        private Encoding _encoding = Encoding.UTF8;

        private bool isBrowserNavigating = false;

        /// <summary>
        /// 上一次搜索结果
        /// </summary>
        private SearchEngineResult _result = new SearchEngineResult();

        #endregion

        #region constructors

        /// <summary>
        /// 创建一个从外部script进行解析的引擎
        /// </summary>
        /// <param name="webBrowser">执行脚本的浏览器</param>
        /// <param name="urlPattern">读取的脚本，形如"http://www.google.cn/search?q="</param>
        /// <param name="scriptPath">脚本所在的本地位置</param>
        public ExternelSearchEngine(WebBrowser webBrowser,
                                    string urlPattern,
                                    string scriptPath)
        {
            _parser = new ExternelParser(webBrowser, scriptPath);
            WebBrowser = webBrowser;
            UrlPattern = urlPattern;
        }

        #endregion

        #region properties

        /// <summary>
        /// 执行脚本的浏览器
        /// </summary>
        public WebBrowser WebBrowser
        {
            get
            {
                return _webBrowser;
            }
            set
            {
                _webBrowser = value;
                Parser.WebBrowser = value;
                _webBrowser.BeginInit();
                _webBrowser.LoadCompleted +=
                    new LoadCompletedEventHandler(WebBrowserLoadCompleted);
                _webBrowser.EndInit();
            }
        }

        /// <summary>
        /// Url模型，形如"http://www.google.cn/search?q="
        /// </summary>
        public string UrlPattern
        {
            get
            {
                return _urlPattern;
            }
            set
            {
                _urlPattern = value;
            }
        }

        /// <summary>
        /// 脚本所在的路径
        /// </summary>
        public string ScriptPath
        {
            get
            {
                return Parser.ScriptPath;
            }
            set
            {
                Parser.ScriptPath = value;
            }
        }

        /// <summary>
        /// 解析器
        /// </summary>
        public ExternelParser Parser
        {
            get 
            { 
                return _parser;
            }
            set 
            {
                _parser = value;
            }
        }

        /// <summary>
        /// 编码方式，默认UTF8
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                _encoding = value;
            }
        }

        /// <summary>
        /// 脚本库路径
        /// </summary>
        public List<string> LibraryPath
        {
            get
            { 
                return _libraryPath;
            }
            set
            {
                _libraryPath = value;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// 读入并执行一个库文件
        /// </summary>
        /// <param name="libPath">库路径</param>
        /// <returns>是否正确读入并执行</returns>
        private bool LoadLibrary(string libPath)
        {
            string content = ScriptManager.LoadScript(ScriptPath);
            bool result = false;
            if (content != "")
            {
                try
                {
                    WebBrowser.InvokeScript("eval", new String[] { content });
                    result = true;
                }
                catch (Exception)
                {
                    // currently do nothing
                }
            }
            return result;
        }

        /// <summary>
        /// 读完一个网页后进行解析的回调函数
        /// </summary>
        /// <param name="sender">产生发送消息者</param>
        /// <param name="e">消息事件</param>
        private void WebBrowserLoadCompleted(object sender, NavigationEventArgs e)
        {
            foreach (string str in _libraryPath)
            {
                LoadLibrary(str);
            }
            _result = Parser.Parse(null, _encoding);
            isBrowserNavigating = false;
        }

        #region ISearch Members

        public IQueryResultItem Search ( InputQuery query )
        {
            string queryString = HttpUtility.UrlEncode(query.Text, _encoding);
            string urlString = string.Format(UrlPattern, queryString);
            string urlAddress = urlString.Trim(new char[] { ' ', '\t', '\n', '\r' });
            this.WebBrowser.Navigate(new Uri(urlAddress));
            isBrowserNavigating = true;

            while (isBrowserNavigating)
            {
                Thread.Sleep(500);
            }
            return _result;
        }

        #endregion

        #endregion
    }
}
