using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SmartMe.Core.Data;

namespace SmartMe.Core.Record
{
    public class RecordManager : IRecordManager
    {
        #region fields
        private IRecordFileManager _fileManager;
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
