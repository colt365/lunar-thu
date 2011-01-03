using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using SmartMe.Core.Data;
using SmartMe.Web.Crawl;
using SmartMe.Web.Parse;
using SmartMe.Web.Properties;

namespace SmartMe.Web.Search
{
    public class DictCn : DictEngine
    {
        public DictCn()
        {
            this.DictType = DictType.Dict_cn;
            this.Encoding = Encoding.GetEncoding(Settings.Default.DictCnEncoding);
            this.QueryFormat = Settings.Default.DictCnQueryFormat;
            this.Parser = new DictCnParser();
        }
    }
}
