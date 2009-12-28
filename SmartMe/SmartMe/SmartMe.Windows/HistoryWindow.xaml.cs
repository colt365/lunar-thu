using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using SmartMe.Core.Record;
using SmartMe.Core.Data;
using SmartMe.Core.Pipeline;

namespace SmartMe.Windows
{
	/// <summary>
	/// Interaction logic for HistoryWindow.xaml
	/// </summary>
	public partial class HistoryWindow : Window
    {
        #region fields

        #endregion

        #region constructor

        public HistoryWindow()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
            InitializeHistoryManager();
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

        public Pipeline Pipeline
        {
            get;
            set;
        }

        #endregion

        #region 读进历史记录

        internal void InitializeHistoryManager()
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
            TreeViewItem todayRecordItem = GetRootItem(DateTime.Today, DateTime.Today);
            todayRecordItem.Header = "今天";
            HistoryTreeView.Items.Add(todayRecordItem);

            // Yesterday
            DateTime yesterday = DateTime.Today - new TimeSpan(1, 0, 0, 0);
            TreeViewItem yesterdayRecordItem = GetRootItem(yesterday, yesterday);
            yesterdayRecordItem.Header = "昨天";
            HistoryTreeView.Items.Add(yesterdayRecordItem);

            // 7 days
            DateTime sevenDaysAgo = DateTime.Today - new TimeSpan(7, 0, 0, 0);
            DateTime twoDaysAgo = DateTime.Today - new TimeSpan(2, 0, 0, 0);
            TreeViewItem sevenDaysRecordItem = GetRootItem(sevenDaysAgo, twoDaysAgo);
            sevenDaysRecordItem.Header = "7天内";
            HistoryTreeView.Items.Add(sevenDaysRecordItem);

            // 30 days
            DateTime eightDaysAgo = DateTime.Today - new TimeSpan(8, 0, 0, 0);
            DateTime thirtyDaysAgo = DateTime.Today - new TimeSpan(30, 0, 0, 0);
            TreeViewItem thirtyDaysRecordItem = GetRootItem(thirtyDaysAgo, eightDaysAgo);
            thirtyDaysRecordItem.Header = "30天内";
            HistoryTreeView.Items.Add(thirtyDaysRecordItem);
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
                QueryResultRecordManager.getResultList(startDate, endDate);
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
            //TreeViewItem resultItem = new TreeViewItem();
            // To-do: Show the result
            InputQuery resultItem = result.Query;
            
            return resultItem;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // 取消关闭窗口，改为隐藏
            //e.Cancel = true;   // BUG:  cancel close window event, but resources are unreleased when exit program!
                                 //   FIXED! TT 09/12/28 21:59

            this.Hide();
        }

        #endregion


        #region 控制历史记录

        private void HistoryTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            InputQuery newQuery = e.NewValue as InputQuery;
            if (newQuery == null)
            {
                return;
            }
            Pipeline.OnInputTextReady(newQuery);
        }

        #endregion
    }
}
