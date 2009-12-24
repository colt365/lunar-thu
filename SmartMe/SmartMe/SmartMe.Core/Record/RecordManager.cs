using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SmartMe.Core.Data;

namespace SmartMe.Core.Record
{
    public class SearchEngineResultRecordManager : IRecordManager
    {
        #region fields

        private IRecordFileManager _fileManager =
            new RecordFileManager(typeof(SearchEngineResult));

        private TimeSpan _obsoletedDuration;

        private string _recordPath;

        private List<SearchEngineResult> _resultList = new List<SearchEngineResult>();

        #endregion

        #region constructors

        /// <summary>
        /// 创立一个管理搜索引擎结果的管理器
        /// </summary>
        /// <param name="recordPath">记录根路径</param>
        /// <param name="obsoletedTimeDuration">超期时间</param>
        public SearchEngineResultRecordManager(
            string recordPath, TimeSpan obsoletedTimeDuration)
        {
            ObsoletedTimeDuration = obsoletedTimeDuration;
            RecordPath = recordPath;
            //LoadQuery();
            //CleanObsoletedRecord();
        }

        #endregion

        #region properties

        /// <summary>
        /// 存储所有输入的文件位置
        /// </summary>
        public string RecordPath
        {
            get
            {
                return _recordPath;
            }
            set
            {
                _recordPath = value;
            }
        }

        /// <summary>
        /// 所有查询结果
        /// </summary>
        public List<SearchEngineResult> ResultList
        {
            get
            {
                return _resultList;
            }
            set
            {
                _resultList = value;
            }
        }

        /// <summary>
        /// 条目最长保留的时间
        /// </summary>
        public TimeSpan ObsoletedTimeDuration
        {
            get;
            set;
        }

        #endregion

        #region methods
        #region IRecordManager Members

        public void appendRecord(QueryResult result, DateTime date)
        {
            List<QueryResult> resultList = getResultList(date);
            resultList.Add(result);
            setResultList(date, resultList);
        }

        public void modifyRecord(QueryResult result, DateTime date)
        {
            List<QueryResult> resultList = getResultList(date);
            foreach (QueryResult existResult in resultList)
            {
                if (result.Query.Equals(result.Query))
                {
                    int i = resultList.IndexOf(existResult);
                    resultList.RemoveAt(i);
                    resultList.Insert(i, result);
                    break;
                }
            }
        }

        public List<QueryResult> getAllRecords(DateTime beginDate, DateTime endDate)
        {
            List<QueryResult> resultList = new List<QueryResult>();
            for (DateTime date = beginDate; date < endDate; date = date.AddDays(1))
            {
                List<QueryResult> results = getResultList(date);
                resultList.AddRange(results);
            }
            return resultList;
        }

        private void setResultList(DateTime date, List<QueryResult> resultList)
        {
            string filePath = getFilePath(date);
            _fileManager.SaveToFile(resultList, filePath);
            throw new NotImplementedException();
        }

        private List<QueryResult> getResultList(DateTime date)
        {
            string filePath = getFilePath(date);
            List<QueryResult> resultList = _fileManager.ReadFromFile(filePath) as List<QueryResult>;
            if (resultList == null)
            {
                return new List<QueryResult>();
            }
            else
            {
                return resultList;
            }
            throw new NotImplementedException();
        }

        private string getFilePath(DateTime date)
        {
            string dateName = date.Year + "-" + date.Month + "-" + date.Day;
            return "data\\" + dateName + "\\default.xml";
        }

        #endregion
        #endregion
    }
}
