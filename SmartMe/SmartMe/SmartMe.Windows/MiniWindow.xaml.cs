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

namespace SmartMe.Windows
{
	/// <summary>
	/// Interaction logic for MiniWindow.xaml
	/// </summary>
	public partial class MiniWindow : Window
    {
        #region UI field
        private bool _isDragging = false;
        private Point _previousMousePoint = new Point();
        #endregion

        public MiniWindow()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}


		private void Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			_isDragging = false;
		}

		private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (_isDragging)
            {
                Point previousPoint = _previousMousePoint;
                Point currentPoint = e.GetPosition(this);
                
                double deltaX = currentPoint.X - previousPoint.X;
                double deltaY = currentPoint.Y - previousPoint.Y;

                //MessageBox.Show("" + previousPoint);
                //MessageBox.Show("" + currentPoint);

                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(
                    delegate {
                         //MessageBox.Show("" + deltaX + "," +  deltaY + "," + this.Margin.Left + deltaX + "," + this.Margin.Top + deltaY);
                        this.Left = this.Left + deltaX;
                        this.Top = this.Top + deltaY;
                    })
                );

                _previousMousePoint = e.GetPosition(this);
            }
            else
            {
            }
		}

		private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			_isDragging = true;
            _previousMousePoint = e.GetPosition(this);
		}

		private void Window_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			_isDragging = false;
		}
	}
}