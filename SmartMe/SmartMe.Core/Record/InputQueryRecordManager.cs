using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Data;
using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Record
{
    /// <summary>
    /// 这个类监听InputQuery，并将其记录以及读取记录
    /// 将这个类加到Pipeline中，监听其InputQuery一层（和WebResourceManager并列）
    /// </summary>
    public class InputQueryRecordManager : ISubScriber
    {
        #region fields

        private IRecordFileManager _recordFileManager =
            new RecordFileManager(typeof(List<InputQuery>));

        private string _recordPath;

        private List<InputQuery> _queryList;

        private TimeSpan _obsoletedTimeDuration = TimeSpan.MaxValue;

        #endregion

        #region constructors

        public InputQueryRecordManager(string recordPath, TimeSpan obsoletedTimeDuration)
        {
            ObsoletedTimeDuration = obsoletedTimeDuration;
            RecordPath = recordPath;
            LoadQuery();
            CleanObsoletedRecord();
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
        /// 所有查询词
        /// </summary>
        public List<InputQuery> QueryList
        {
            get
            {
                return _queryList;
            }
            set
            {
                _queryList = value;
            }
        }

        /// <summary>
        /// 条目最长保留的时间
        /// </summary>
        public TimeSpan ObsoletedTimeDuration
        {
            get
            {
                return _obsoletedTimeDuration;
            }
            set
            {
                _obsoletedTimeDuration = value;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// 新增一个查询项
        /// </summary>
        /// <param name="query">查询项</param>
        private void AddInputQuery(InputQuery query)
        {
            if (query == null)
            {
                //TO-DO: throw exception?
                return;
            }

            lock (QueryList)
            {
                QueryList.Add(query);
            }
            // TO-DO: Do not save anyway, add some saving policies
            SaveQuery();
        }

        /// <summary>
        /// 将所有查询结果存入文件
        /// </summary>
        /// <returns>存入是否成功</returns>
        public bool SaveQuery()
        {
            try
            {
                lock (_recordFileManager)
                {
                    Console.WriteLine(_recordFileManager.SaveToXmlString(QueryList));
                    _recordFileManager.SaveToFile(QueryList, RecordPath);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// 从文件中读入所有查询结果，覆盖掉已有的结果
        /// </summary>
        /// <returns>读取是否成功</returns>
        public bool LoadQuery()
        {
            try
            {
                List<InputQuery> queryList = _recordFileManager.ReadFromFile(RecordPath)
                    as List<InputQuery>;
                if (queryList != null)
                {
                    QueryList = queryList;
                    return true;
                }
                else
                {
                    QueryList = new List<InputQuery>();
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                QueryList = new List<InputQuery>();
                return false;
            }
        }

        /// <summary>
        /// 删除过期的数据
        /// </summary>
        public void CleanObsoletedRecord()
        {
            for (int i = QueryList.Count - 1; i > -1; i--)
            {
                if (DateTime.Now - QueryList[i].Time > ObsoletedTimeDuration)
                {
                    QueryList.RemoveAt(i);
                }
            }
        }

        #region ISubScriber Members

        public void Handle(IMessage message)
        {
            InputQuery query = message as InputQuery;
            AddInputQuery(query);
        }

        #endregion

        #endregion
    }
}
