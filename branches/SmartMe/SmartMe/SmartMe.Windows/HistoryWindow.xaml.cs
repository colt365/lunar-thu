using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using SmartMe.Core.Record;
using SmartMe.Core.Data;
using SmartMe.Core.Pipeline;
using SmartMe.Web;

namespace SmartMe.Windows
{
    public delegate void ChangeString(string text);

	/// <summary>
	/// Interaction logic for HistoryWindow.xaml
	/// </summary>
	public partial class HistoryWindow : Window
    {
        #region fields

        private bool _isClosed = false;

        #endregion

        #region constructor

        public HistoryWindow()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
            //InitializeHistoryManager();
        }

        #endregion

        #region nested

        /// <summary>
        /// 一个辅助类，用来在TreeView中显示
        /// </summary>
        class DisplayQueryResult
        {
            #region constructors

            public DisplayQueryResult(QueryResult queryResult)
            {
                QueryResult = queryResult;
            }

            #endregion

            #region properties

            public QueryResult QueryResult
            {
                get;
                set;
            }

            #endregion

            #region methods

            public override string ToString()
            {
                return QueryResult.Query.ToString();
            }

            #endregion
        }

        #endregion

        #region properties

        /// <summary>
        /// 管理搜索引擎的历史记录
        /// </summary>
        public QueryResultRecordManager QueryResultRecordManager
        {
            get;
            set;
        }

        /// <summary>
        /// 通讯管道
        /// </summary>
        public Pipeline Pipeline
        {
            get;
            set;
        }

        /// <summary>
        /// 搜索引擎控制
        /// </summary>
        public IQueryResultHandler QueryResultHandler
        {
            get;
            set;
        }

        /// <summary>
        /// 改变文本
        /// </summary>
        public ChangeString ChangeText
        {
            get;
            set;
        }

        /// <summary>
        /// 窗体是否关闭
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return _isClosed;
            }
        }

        #endregion

        #region 读进历史记录

        private void InitializeHistoryManager()
        {
            QueryResultRecordManager =
                new QueryResultRecordManager(
                    "data", new TimeSpan(30, 0, 0, 0));
        }

        public void LoadHistoryRecord()
        {
            //HistoryTreeView = new TreeView();
            HistoryTreeView.Items.Clear();

            // Today
            TreeViewItem todayRecordItem = GetRootItem(
                DateTime.Today, DateTime.Today);
            todayRecordItem.Header = "今天";
            HistoryTreeView.Items.Add(todayRecordItem);

            // Yesterday
            DateTime yesterday = DateTime.Today - new TimeSpan(1, 0, 0, 0);
            TreeViewItem yesterdayRecordItem =
                GetRootItem(yesterday, yesterday);
            yesterdayRecordItem.Header = "昨天";
            HistoryTreeView.Items.Add(yesterdayRecordItem);

            // 7 days
            DateTime sevenDaysAgo = DateTime.Today - new TimeSpan(7, 0, 0, 0);
            DateTime twoDaysAgo = DateTime.Today - new TimeSpan(2, 0, 0, 0);
            TreeViewItem sevenDaysRecordItem =
                GetRootItem(sevenDaysAgo, twoDaysAgo);
            sevenDaysRecordItem.Header = "过去7天";
            HistoryTreeView.Items.Add(sevenDaysRecordItem);
        }

        /// <summary>
        /// 得到一段时间内的所有结果，包括开头和结尾
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>结果对应的一项</returns>
        private TreeViewItem GetRootItem(DateTime startDate, DateTime endDate)
        {
            TreeViewItem rootItem = new TreeViewItem();
            List<QueryResult> resultList =
                QueryResultRecordManager.GetResultList(startDate, endDate);
            foreach (QueryResult result in resultList)
            {
                object resultItem = GetResultItem(result);
                rootItem.Items.Add(resultItem);
            }
            return rootItem;
        }

        /// <summary>
        /// 得到一个搜索结果对应的TreeViewItem
        /// </summary>
        /// <param name="result">搜索结果</param>
        /// <returns>对应的一项</returns>
        private object GetResultItem(QueryResult result)
        {
            DisplayQueryResult resultItem = new DisplayQueryResult(result);
            return resultItem;
        }

        #endregion


        #region 控制历史记录

        /// <summary>
        /// 清除所有历史记录
        /// </summary>
        private void CleanHistoryRecord()
        {
            QueryResultRecordManager.RemoveAllResultList();
        }

        /// <summary>
        /// 清除某天之前（不包括这一天）的历史记录
        /// </summary>
        /// <param name="date">某一天</param>
        private void CleanHistoryRecord(DateTime date)
        {
            QueryResultRecordManager.RemoveResultListFromDate(date);
        }

        /// <summary>
        /// 显示历史记录
        /// </summary>
        /// <param name="result">记录</param>
        private void ShowHistoryResult(QueryResult result)
        {
            if (ChangeText != null)
            {
                ChangeText(result.Query.ToString());
            }
            QueryResultHandler.OnResultNew(result);
            QueryResultHandler.OnResultUpdate(result);
            QueryResultHandler.OnResultCompleted(result);
        }

        #endregion

        #region 响应消息

        private void HistoryTreeView_SelectedItemChanged(
            object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DisplayQueryResult displayQueryResult =
                e.NewValue as DisplayQueryResult;
            if (displayQueryResult == null)
            {
                return;
            }
            // 显示历史记录结果
            ShowHistoryResult(displayQueryResult.QueryResult);

            // 显示搜索引擎结果
            // Pipeline.OnInputTextReady(newQuery);
        }

        private void CleanAllRecordsMenuItem_Click(
            object sender, RoutedEventArgs e)
        {
            CleanHistoryRecord();
            LoadHistoryRecord();
        }

        private void CleanRecordsFromDateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // TO-DO: Add a dialog to let user choose instead of the magic number
            ConfigDialog dialog = new ConfigDialog();
            dialog.ShowDialog();
            if (dialog.Ok)
            {
                bool? cleanAllRecords = dialog.CleanAllRecordsCheckBox.IsChecked;
                if (cleanAllRecords == true)
                {
                    CleanHistoryRecord();
                    LoadHistoryRecord();
                }
                else
                {
                    string timeSpanText = dialog.TimeSpanText;
                    int day = int.Parse(timeSpanText);
                    TimeSpan timeSpan = new TimeSpan(day, 0, 0, 0);
                    CleanHistoryRecord(DateTime.Today - timeSpan);
                    LoadHistoryRecord();
                }
            }
        }

        private void RefreshMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadHistoryRecord();
        }

        #endregion

        private void RemoveRecordMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // TO-DO: Remove current record
            DisplayQueryResult displayQueryResult =
                HistoryTreeView.SelectedItem as DisplayQueryResult;

            // 选中的不是历史记录，则返回
            if (displayQueryResult == null)
            {
                return;
            }

            DateTime date = displayQueryResult.QueryResult.Time.Date;
            QueryResultRecordManager.RemoveResult(
                displayQueryResult.QueryResult, date);
            LoadHistoryRecord();
        }

        protected override void OnClosed(EventArgs e)
        {
            _isClosed = true;
            base.OnClosed(e);
        }
    }
}
