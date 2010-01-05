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
using System.Windows.Shapes;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;

using SmartMe.Core;
using SmartMe.Core.Pipeline;
using SmartMe.Core.Data;
using SmartMe.Core.Record;
using SmartMe.Web;
using SmartMe.Web.Search;
using SmartMe.Windows.Externel;


namespace SmartMe.Windows
{
    partial class MainWindow : Window
    {
        #region Font
		bool _isFontSizeSliderLoaded = false;
        private void FontSizeSlider_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        	Properties.Settings setting = this.Window.Resources["SettingsDataSource"] as Properties.Settings;
            if (setting != null)
            {
               	FontSizeSlider.Value = setting.fontSize;
				_isFontSizeSliderLoaded = true;
            }
        }

		
        private void FontSizeSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
			double fontSize = FontSizeSlider.Value;
			if (_isFontSizeSliderLoaded) 
			{
        		SetActualFront(fontSize);
			}
        }
		
        private void SetActualFront(double fontSize)
        {
            Properties.Settings setting = this.Window.Resources["SettingsDataSource"] as Properties.Settings;
            if (setting != null)
            {
                setting.fontSize = fontSize;
                this.UpdateLayout();
            }
        }
        #endregion Font

        #region background color
        bool _isBackgoundColorLoaded = false;
        
		private void BackgroundColorMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings setting = this.Window.Resources["SettingsDataSource"] as Properties.Settings;
            
            Microsoft.Samples.CustomControls.ColorPickerDialog dialog = new Microsoft.Samples.CustomControls.ColorPickerDialog();
            if (setting != null)
            {
                dialog.StartingColor = setting.backgroundColor;
            }

            dialog.ShowDialog();
            bool? ret = dialog.DialogResult;
            if (ret.HasValue && ret.Value == true)
            {
                if (setting != null)
                {
                    MessageBox.Show(dialog.SelectedColor.ToString());
                    setting.backgroundColor = dialog.SelectedColor;
                    this.UpdateLayout();
                }
            }
        }
        #endregion background color
		
    }
}
