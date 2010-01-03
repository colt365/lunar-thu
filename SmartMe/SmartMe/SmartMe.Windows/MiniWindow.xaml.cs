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
        private System.Windows.Forms.MenuItem trayViewHistorymenuItem;

        private MainWindow _mainWindow = new MainWindow(ref notifyIcon);

        public MiniWindow()
		{
			this.InitializeComponent();

            InitNotifyIcon();
			// Insert code required on object creation below this point.
            HideMainWindow();
		}

        #region MinimizeToIcon
        // private bool firstShowTip = true;
        private void InitNotifyIcon()
        {
            trayContextMenu = new System.Windows.Forms.ContextMenu();
            trayExitMenuItem = new System.Windows.Forms.MenuItem();
            trayViewHistorymenuItem = new System.Windows.Forms.MenuItem();

            trayContextMenu.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { trayViewHistorymenuItem, trayExitMenuItem });

            trayViewHistorymenuItem.Index = 0;
            trayViewHistorymenuItem.Text = "查看历史记录(&H)";
            trayViewHistorymenuItem.Click += new EventHandler(TrayViewHistoryMenuItem_Click);

            trayExitMenuItem.Index = 1;
            trayExitMenuItem.Text = "退出(&E)";
            trayExitMenuItem.Click += new EventHandler(TrayExitMenuItem_Click);

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = "SmartMe已最小化到托盘，双击此处恢复窗口";
            notifyIcon.BalloonTipTitle = "SmartMe";
            notifyIcon.Text = "SmartMe";
            notifyIcon.Icon = new System.Drawing.Icon("icon.ico");
            notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
            notifyIcon.ContextMenu = this.trayContextMenu;
            ShowTrayIcon(true);	// Always show the icon
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (_mainWindow.IsVisible)
            {
                HideMainWindow();
                if (notifyIcon != null)// && firstShowTip)
                {
                    // firstShowTip = false;
                    notifyIcon.ShowBalloonTip(500);
                }
            }
            else
            {
                ShowMainWindow();
            }
        }

        private void ShowTrayIcon(bool show)
        {
            if (notifyIcon != null)
                notifyIcon.Visible = show;
        }

        //TrayExitMenuItem_Click
        private void TrayExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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
            Thread.Sleep(1);
			_mainWindow.Topmost = false;
        }
        public void HideMainWindow()
        {
            _mainWindow.Visibility = Visibility.Collapsed;
        }


        private void MiniGrid_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
        {
        	MessageBox.Show("MiniGrid_PreviewDragOver! TODO: Please Add animation storyboard to shrink window -- TT!");
        }

        private void MiniGrid_Drop(object sender, System.Windows.DragEventArgs e)
        {
        	MessageBox.Show("MiniGrid_Drop! TODO: Please do query -- TT!");
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

        

	}
}