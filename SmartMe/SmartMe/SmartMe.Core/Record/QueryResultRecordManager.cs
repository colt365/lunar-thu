using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SmartMe.Core.Data;
using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Record
{
    public class QueryResultRecordManager : IRecordManager, ISubScriber
    {
        #region fields

        private IRecordFileManager _fileManager =
            new RecordFileManager(typeof(List<QueryResult>));

        private string _recordPath;

        private List<QueryResult> _resultList =
            new List<QueryResult>();

        #endregion

        #region constructors

        /// <summary>
        /// 创立一个管理搜索引擎结果的管理器
        /// </summary>
        /// <param name="recordPath">记录根路径</param>
        /// <param name="obsoletedTimeDuration">超期时间</param>
        public QueryResultRecordManager(
            string recordPath, TimeSpan obsoletedTimeDuration)
        {
            ObsoletedTimeDuration = obsoletedTimeDuration;
            RecordPath = recordPath;
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
        public List<QueryResult> ResultList
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

        public void AppendRecord(QueryResult result, DateTime date)
        {
            List<QueryResult> resultList = GetResultList(date);
            resultList.Add(result);
            SetResultList(date, resultList);
        }

        public void ModifyRecord(QueryResult result, DateTime date, bool appendIfNotExist)
        {
            List<QueryResult> resultList = GetResultList(date);
            foreach (QueryResult existResult in resultList)
            {
                if (result.Query.Equals(existResult.Query))
                {
                    int i = resultList.IndexOf(existResult);
                    //resultList.RemoveAt(i);
                    //resultList.Insert(i, result);
                    if (existResult.Items.Count <= result.Items.Count)
                    {
                        existResult.Items.Clear();
                        existResult.Items.AddRange(result.Items);
                        SetResultList(date, resultList);
                    }
                    return;
                }
            }
            if (appendIfNotExist)
            {
                AppendRecord(result, date);
            }
        }

        public List<QueryResult> GetResultList(DateTime beginDate, DateTime endDate)
        {
            List<QueryResult> resultList = new List<QueryResult>();
            for (DateTime date = beginDate; date <= endDate; date = date.AddDays(1))
            {
                List<QueryResult> results = GetResultList(date);
                resultList.AddRange(results);
            }
            return resultList;
        }

        /// <summary>
        /// 移除指定时间内的结果项
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">终结日期</param>
        public void RemoveResultList(DateTime beginDate, DateTime endDate)
        {
            for (DateTime date = beginDate; date <= endDate; date = date.AddDays(1))
            {
                RemoveResultList(date);
            }            
        }

        /// <summary>
        /// 移除指定天份的结果项
        /// </summary>
        /// <param name="date">指定的天份</param>
        public void RemoveResultList(DateTime date)
        {
            string path = GetDirectoryPath(date);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// 移除一条历史记录
        /// </summary>
        /// <param name="result">历史记录</param>
        /// <param name="date">日期</param>
        public void RemoveResult(QueryResult result, DateTime date)
        {
            List<QueryResult> resultList = GetResultList(date);
            resultList.Remove(result);
            SetResultList(date, resultList);
        }

        /// <summary>
        /// 移除所有记录
        /// </summary>
        public void RemoveAllResultList()
        {
            string[] allRecordDirectories = Directory.GetDirectories(RecordPath);
            foreach (string directory in allRecordDirectories)
            {
                string dateString = Path.GetFileName(directory);
                DateTime date = GetDate(dateString);
                if (date == DateTime.MaxValue)
                {
                    continue;
                }
                Directory.Delete(directory, true);
            }
        }

        /// <summary>
        /// 移除某天之前的记录
        /// </summary>
        /// <param name="dueDate">保留下来的第一天记录</param>
        public void RemoveResultListFromDate(DateTime dueDate)
        {
            string[] allRecordDirectories = Directory.GetDirectories(RecordPath);
            foreach (string directory in allRecordDirectories)
            {
                string dateString = Path.GetFileName(directory);
                DateTime date = GetDate(dateString);
                if (date == DateTime.MaxValue)
                {
                    continue;
                }
                if (date >= dueDate)
                {
                    continue;
                }
                Directory.Delete(directory, true);
            }
        }

        public void SetResultList(DateTime date, List<QueryResult> resultList)
        {
            string directoryPath = GetDirectoryPath(date);
            string filePath = GetFilePath(date);

            lock (_fileManager)
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                _fileManager.SaveToFile(resultList, filePath);
            }
        }

        public List<QueryResult> GetResultList(DateTime date)
        {
            string filePath = GetFilePath(date);
            List<QueryResult> resultList = null;
            lock (_fileManager)
            {
                resultList = _fileManager.ReadFromFile(filePath) as List<QueryResult>;
            }
            if (resultList == null)
            {
                return new List<QueryResult>();
            }
            else
            {
                return resultList;
            }
        }

        private string GetFilePath(DateTime date)
        {
            return GetDirectoryPath(date) + "\\default.xml";
        }

        private string GetDirectoryPath(DateTime date)
        {
            string dateName = date.Year + "-" + date.Month + "-" + date.Day;
            return RecordPath + "\\" + dateName;
        }

        public static DateTime GetDate(string dateTimeString)
        {
            DateTime date = DateTime.MaxValue;
            string[] numbers = dateTimeString.Split(new char[] { '-' });
            if (numbers.Length != 3)
            {
                return date;
            }
            int year = int.Parse(numbers[0]);
            int month = int.Parse(numbers[1]);
            int day = int.Parse(numbers[2]);
            date = new DateTime(year, month, day);
            return date;
        }

        public void UpdateRecord(QueryResult result)
        {
            ModifyRecord(result, DateTime.Today, true);
        }

        #endregion

        #region ISubScriber Members

        public void Handle(IMessage message)
        {
            if (message.MessageType == MessageType.QueryResult)
            {
                QueryResult result = message as QueryResult;
                if (result == null)
                {
                    return;
                }
                UpdateRecord(result);
            }
        }

        #endregion

        #endregion
    }
}
