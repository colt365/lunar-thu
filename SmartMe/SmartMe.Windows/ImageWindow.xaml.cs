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
	/// Interaction logic for ImageWindow.xaml
	/// </summary>
	public partial class ImageWindow : Window
	{
		public ImageWindow()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
            MyImage.Source = new BitmapImage(
                    new Uri(@"http://ts3.cn.mm.bing.net/images/thumbnail.aspx?q=1513435433638&id=0928f77ac95be8376b0ca74fb42aa885&url=http%3a%2f%2fwww.hbqnb.com%2fnews%2fFiles%2fadminfiles%2fshiwei%2f20070905%2fbadaling.jpg", UriKind.RelativeOrAbsolute)
                    );
            MyImage1.Source = new BitmapImage(
                    new Uri(@"http://ts2.cn.mm.bing.net/images/thumbnail.aspx?q=1347115360745&id=3110fea8b9b5beea5c708dfd7543d99e&url=http%3a%2f%2fnews.xinhuanet.com%2ftravel%2f2007-05%2f18%2fxinsrc_0620504181417312758829.jpg", UriKind.RelativeOrAbsolute)
                    );
                
		}
	}
}