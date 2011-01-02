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
	/// Interaction logic for ScreenWindow.xaml
	/// </summary>
	public partial class ScreenWindow : Window
	{
		private bool _isLeftMouseDown = false;

		private bool _isScreenRegionSelected = false;
        public bool IsScreenRegionSelected
        {
            get { return _isScreenRegionSelected; }
            set { _isScreenRegionSelected = value; }
        }

        private Point _p0;
        public System.Windows.Point P0
        {
            get { return _p0; }
            set { _p0 = value; }
        }
        private Point _p1;
        public System.Windows.Point P1
        {
            get { return _p1; }
            set { _p1 = value; }
        }

		public ScreenWindow()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

		private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			this.Hide();
		}

        private void DrawSelection(Point point0, Point point1)
        {
            double minX = Math.Min(point0.X, point1.X);
            double maxX = Math.Max(point0.X, point1.X);
            double minY = Math.Min(point0.Y, point1.Y);
            double maxY = Math.Max(point0.Y, point1.Y);

            double width = maxX - minX;
            double height = maxY - minY;

            this.SelectionRectangle.Margin = new Thickness(minX, minY, 0, 0);
            this.SelectionRectangle.Width = width;
            this.SelectionRectangle.Height = height;
            this.SelectionRectangle.Visibility = Visibility.Visible;

            this.SelectionRectangle.InvalidateArrange();
            this.SelectionRectangle.UpdateLayout();
        }

		private void LayoutRoot_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
            if (!_isLeftMouseDown)
            {
                Point p = e.GetPosition(this);
                _p0 = _p1 = p;
                DrawSelection(_p0, _p1);

                _isLeftMouseDown = true;
                _isScreenRegionSelected = false;
            }
		}
        private void LayoutRoot_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isLeftMouseDown)
            {
                Point p = e.GetPosition(this);
                _p1 = p;
                DrawSelection(_p0, _p1);
            }
        }
		private void LayoutRoot_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
            if (_isLeftMouseDown)
            {
                Point p = e.GetPosition(this);
                _p1 = p;

                DrawSelection(_p0, _p1);

                _isLeftMouseDown = false;
                _isScreenRegionSelected = true;

                this.Hide();
            }
		}

		private void LayoutRoot_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			_isScreenRegionSelected = false;
			this.Hide();
		}
	}
}