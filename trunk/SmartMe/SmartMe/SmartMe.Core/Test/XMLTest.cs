using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Data;

namespace SmartMe.Core.Test
{
    public class XMLTest
    {
        public static void Test()
        {
            SearchEngineResult result = GetSearchResult();
            Console.WriteLine(result);
            XMLObject xmlObject = result.ToXMLObject();
            Console.WriteLine(xmlObject.ToString());
            XMLObject newXmlObject = new XMLObject(xmlObject.ToString());
            Console.WriteLine(newXmlObject.ToString());
            SearchEngineResult newResult = SearchEngineResult.GetSearchEngineResultFromXMLObject(newXmlObject);
            Console.WriteLine(newResult);
            Console.WriteLine(result.ToString() == newResult.ToString());
        }

        private static SearchEngineResult GetSearchResult()
        {
            SearchEngineResult result = new SearchEngineResult();
            result.SearchEngineType = SearchEngineType.Other;
            SearchEngineResult.ResultItem item1 = new SearchEngineResult.ResultItem();
            item1.Title = "INFO";
            item1.CacheUrl = "http://aaa.www.com/";
            item1.Description = "LinTian" + " " + 1 + " Done.";
            item1.SimilarUrl = "http://similar.www.com/";
            item1.Url = "http://info.tsinghua.edu.cn";
            result.Results.Add(item1);
            SearchEngineResult.ResultItem item2 = new SearchEngineResult.ResultItem();
            item2.Title = "INFO2";
            item2.CacheUrl = "http://aaa.www.com22222/";
            item2.Description = "LunaR" + " " + 2 + " Done2.";
            item2.SimilarUrl = "http://similar.www.com2222/";
            item2.Url = "http://info.tsinghua.edu.cn2";
            result.Results.Add(item2);
            return result;
        }
    }
}
