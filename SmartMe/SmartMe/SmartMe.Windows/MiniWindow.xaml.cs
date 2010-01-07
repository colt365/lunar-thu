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
using System.Threading;
using SmartMe.Core.Data;

namespace SmartMe.Windows
{
	/// <summary>
	/// Interaction logic for MiniWindow.xaml
	/// </summary>
	public partial class MiniWindow : Window
	{
        // NotifyIcon
        static public System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenu trayContextMenu;
        private System.Windows.Forms.MenuItem trayExitMenuItem;
        private System.Windows.Forms.MenuItem trayMiniMenuItem;
        private System.Windows.Forms.MenuItem trayViewHistorymenuItem;

        private MainWindow _mainWindow = new MainWindow(notifyIcon);

        public MiniWindow()
		{
			this.InitializeComponent();

            InitNotifyIcon();
			// Insert code required on object creation below this point.
            HideMainWindow();

            App.Logger.Message("SmartMe Start!");
		}

        #region MinimizeToIcon
        // private bool firstShowTip = true;
        private void InitNotifyIcon()
        {
            trayContextMenu = new System.Windows.Forms.ContextMenu();
            trayExitMenuItem = new System.Windows.Forms.MenuItem();
            trayMiniMenuItem = new System.Windows.Forms.MenuItem();
            trayViewHistorymenuItem = new System.Windows.Forms.MenuItem();

            trayContextMenu.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { trayViewHistorymenuItem, trayMiniMenuItem, trayExitMenuItem });

            trayViewHistorymenuItem.Index = 0;
            trayViewHistorymenuItem.Text = "查看历史记录(&H)";
            trayViewHistorymenuItem.Click += new EventHandler(TrayViewHistoryMenuItem_Click);

            trayMiniMenuItem.Index = 1;
            trayMiniMenuItem.Text = "隐藏悬浮窗(&M)";
            trayMiniMenuItem.Click += new EventHandler(TrayMiniMenuItem_Click);
			
            trayExitMenuItem.Index = 2;
            trayExitMenuItem.Text = "退出(&E)";
            trayExitMenuItem.Click += new EventHandler(TrayExitMenuItem_Click);

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = "SmartMe已最小化到托盘，双击此处恢复窗口";
            notifyIcon.BalloonTipTitle = "SmartMe";
            notifyIcon.Text = "SmartMe";
            notifyIcon.Icon = new System.Drawing.Icon("icon.ico");
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_Click);
            notifyIcon.ContextMenu = this.trayContextMenu;
            ShowTrayIcon(true);	// Always show the icon
        }

        private void notifyIcon_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_mainWindow.IsVisible)
                {
                    HideMainWindow();
                    if (notifyIcon != null)
                    {
                        notifyIcon.ShowBalloonTip(500);
                    }
                }
                else
                {
                    ShowMainWindow();
                }
            }
        }

        private void ShowTrayIcon(bool show)
        {
            if (notifyIcon != null)
            {
                notifyIcon.Visible = show;
            }
        }

        //TrayExitMenuItem_Click
        private void TrayExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
		
        private void TrayMiniMenuItem_Click(object sender, EventArgs e)
        {
			if(this.IsVisible){
            	this.Hide();
				trayMiniMenuItem.Text = "显示悬浮窗(&M)";
			} else {
            	this.Show();
				trayMiniMenuItem.Text = "隐藏悬浮窗(&M)";
			}
			
        }

        private void TrayViewHistoryMenuItem_Click(object sender, EventArgs e)
        {
            _mainWindow.ShowHistoryWindow(this.Top, this.Left);
        }
        #endregion MinimizeToIcon

		
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        	_mainWindow.Close();

            if (notifyIcon != null)
            {
                notifyIcon.Dispose();
            }
        }
		
        public void ShowMainWindow()
        {
            _mainWindow.Visibility = Visibility.Visible;
			// make it top most
			_mainWindow.Topmost = true;
			Thread.Sleep(1);
			this.Topmost = true;
        }
        public void HideMainWindow()
        {
            _mainWindow.Visibility = Visibility.Collapsed;
        }

        private void MiniGrid_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
        {
        	//MessageBox.Show("MiniGrid_PreviewDragOver! TODO: Please Add animation storyboard to shrink window -- TT!");
        }

        private void MiniGrid_Drop(object sender, System.Windows.DragEventArgs e)
        {
        	//MessageBox.Show("MiniGrid_Drop! TODO: Please do query -- TT!");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Window_Drop:" + DateTime.Now.ToUniversalTime());
            sb.AppendLine("sender:" + sender.ToString());
            sb.AppendLine("e.Data.GetType:" + e.Data.GetType());
            sb.AppendLine("e.Data.Formats:" + string.Join("; ", e.Data.GetFormats(true)));
            sb.AppendLine("e.Source.GetType:" + e.Source.GetType());
            sb.AppendLine("e.Source.ToString:" + e.Source.ToString());
            sb.AppendLine("e.OriginalSource.GetType:" + e.OriginalSource.GetType());
            sb.AppendLine("e.OriginalSource.ToString:" + e.OriginalSource.ToString());
            sb.AppendLine("-- convert e.Data : --");
            _mainWindow.ResultTextBox.Text = sb.ToString();
            sb = new StringBuilder();

            Externel.DragArgDispatcher dispatcher = new Externel.DragArgDispatcher();
            string text = string.Empty;
            InputQueryType type = new InputQueryType();
            bool isOK = dispatcher.TryGetQuery(e, ref text, ref type);
            if (isOK)
            {
                _mainWindow.InputTextBox.Text = text;
                _mainWindow.DoDirectQuery(text, type);
            }

            _mainWindow.ResultTextBox.Text += sb.ToString();
            if (!_mainWindow.IsVisible)
            {
                ShowMainWindow();
            }
        }
		
        private void MiniGrid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	// double clicked            
            if (e.ClickCount >= 2)
            {
                // Just toggle disply mainWindow
				// TODO: Please make it more friendly to user -- TT
                // My Suggestion: 
                //      If mainWindow is not visible, make it visiable and bring to top;
                //      if visible but not on top, just bring it to top;
                //      if visible and on top, hide it
                if (_mainWindow.IsVisible)
                {
                    HideMainWindow();
                }
                else
                {
                    ShowMainWindow();
                }
            }
        }

        private void MiniViewHistoryMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			TrayViewHistoryMenuItem_Click(sender, e);
        }

        private void MiniExitMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	TrayExitMenuItem_Click(sender, e);
        }

        private void MiniHideMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			this.Hide();
			trayMiniMenuItem.Text = "显示悬浮窗(&M)";
        }
		private void MiniMinimizeMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if(_mainWindow.IsVisible) {
				HideMainWindow();
			} else {
                ShowMainWindow();
			}
		}

		private void MiniGrid_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if(_mainWindow.IsVisible){
				MiniMenuItem_Minimize.Header = "隐藏搜索窗口";
			} else {
				MiniMenuItem_Minimize.Header = "显示搜索窗口";
			}
		}
	}
}