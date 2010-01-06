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
            if (!_isFontSizeSliderLoaded)
            {
                Properties.Settings setting = App.Current.Resources["SettingsDataSource"] as Properties.Settings;
                if (setting != null)
                {
                    FontSizeSlider.Value = setting.fontSize;
                    _isFontSizeSliderLoaded = true;
                }
            }
        }

		
        private void FontSizeSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
			if (_isFontSizeSliderLoaded) 
			{
                double fontSize = FontSizeSlider.Value;
                Properties.Settings setting = App.Current.Resources["SettingsDataSource"] as Properties.Settings;
                if (setting != null)
                {
                    setting.fontSize = fontSize;
                    this.UpdateLayout();
                }
			}
        }
        #endregion Font

        #region background color        
		private void BackgroundColorMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Properties.Settings setting = App.Current.Resources["SettingsDataSource"] as Properties.Settings;
            
            Microsoft.Samples.CustomControls.ColorPickerDialog dialog = new Microsoft.Samples.CustomControls.ColorPickerDialog();
            if (setting != null)
            {
                dialog.StartingColor = setting.backgroundColor.Color;
            }

            dialog.ShowDialog();
            bool? ret = dialog.DialogResult;
            if (ret.HasValue && ret.Value == true)
            {
                if (setting != null)
                {
                    setting.backgroundColor = new SolidColorBrush(dialog.SelectedColor);
                    this.UpdateLayout();
                }
            }
        }
        #endregion background color
		
		#region opacity 
        bool _isOpacitySliderLoaded = false;
		private void OpacitySlider_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (! _isOpacitySliderLoaded)
            {
                Properties.Settings setting = App.Current.Resources["SettingsDataSource"] as Properties.Settings;
                if (setting != null)
                {
                    double opacity = (double)setting.opacity * Convert.ToDouble(OpacitySlider.Maximum);
                    OpacitySlider.Value = opacity;
                    _isFontSizeSliderLoaded = true;
                }
            }
        }

        private void OpacitySlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isFontSizeSliderLoaded)
            {
                Properties.Settings setting = App.Current.Resources["SettingsDataSource"] as Properties.Settings;
                if (setting != null)
                {
                    double opacity = (double)OpacitySlider.Value / OpacitySlider.Maximum;
                    setting.opacity = opacity;
                }
            }
        }
		#endregion opacity
    }
}
