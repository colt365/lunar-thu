using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartMe.Windows
{
	/// <summary>
	/// Interaction logic for GrabControl.xaml
	/// </summary>
	public partial class GrabControl : UserControl
	{
		public GrabControl()
		{
			this.InitializeComponent();
		}
		

        private void GrabButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            //ShowScreenWindowAction action = new ShowScreenWindowAction();
            ScreenWindow screenWindow = new ScreenWindow();
            bool? isActived = screenWindow.ShowDialog();

            if (screenWindow.IsScreenRegionSelected)
            {
                Point p0 = screenWindow.P0;
                Point p1 = screenWindow.P1;
                //this.ResultTextBox.Text = string.Format("SelectionRegion: ({0}, {1}), ({2}, {3})", new object[] { p0.X, p0.Y, p1.X, p1.Y });
            }
            else
            {
                //this.ResultTextBox.Text = string.Format("No SelectionRegion");
            }
            screenWindow.Close();
        }
	}
}