using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;

namespace NhwTest
{
    class Program
    {
        [DllImport(@"C:\WINDOWS\system32\Nhw32.dll")]
        private static extern uint BL_SetFlag32(uint nFlag, IntPtr hNotifyWnd, int MouseX, int MouseY);

        [DllImport(@"C:\WINDOWS\system32\Nhw32.dll")]
        private static extern uint BL_GetText32([MarshalAs(UnmanagedType.LPStr)]StringBuilder lpszCurWord, int nBufferSize, ref Rectangle lpWordRect);

        [DllImport(@"C:\WINDOWS\system32\Nhw32.dll")]
        private static extern bool SetNHW32();

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        private enum GetWordFlag
        {
            enable = 1001,
            disable = 1002
        }

        static void Main(string[] args)
        {
            int bufSize = 256;
            StringBuilder sb = new StringBuilder(bufSize);

            Rectangle rect = new Rectangle(0, 0, 100, 100);

            if (SetNHW32())
            {                
                {
                    for (;;)
                    {
                        int i = 1;
                        int j = 0;
                        Point cursorPoint = new Point();
                        GetCursorPos(ref cursorPoint);
                        BL_SetFlag32((uint)GetWordFlag.enable, IntPtr.Zero, cursorPoint.X, cursorPoint.Y);
                        Thread.Sleep(1000);
                        BL_GetText32(sb, bufSize, ref rect);
                        System.Console.WriteLine(sb.ToString() + " " + rect.ToString() + " " + cursorPoint.ToString());                        
                    }
                }
            }
        }
    }
}
