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
	/// Interaction logic for DebugControl.xaml
	/// </summary>
	public partial class DebugControl : UserControl
	{
        protected StringBuilder _debugStringBuilder = new StringBuilder();

        public void Debug(object o)
        {
            string str = string.Format("{0}", o);
            _debugStringBuilder.AppendLine(str);
            ResultTextBox.Text = _debugStringBuilder.ToString();
        }

        public void DebugClear()
        {
            _debugStringBuilder = new StringBuilder();
            ResultTextBox.Text = _debugStringBuilder.ToString();
        }

		public DebugControl()
		{
			this.InitializeComponent();
		}
	}
}