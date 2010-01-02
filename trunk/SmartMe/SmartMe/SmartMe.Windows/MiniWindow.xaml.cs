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
		private MainWindow _mainWindow = new MainWindow();
        public MiniWindow()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
            HideMainWindow();
		}
		
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        	_mainWindow.Close();
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