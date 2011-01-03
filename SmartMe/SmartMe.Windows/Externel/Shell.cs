using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace SmartMe.Windows.Externel
{
    public class Shell
    {
        private void DoShellCall(object processStartInfoObj)
        {
            ProcessStartInfo info = processStartInfoObj as ProcessStartInfo;

            if (info == null)
            {
                return;
            }
            else
            {
                Process process = null;
                try
                {
                    process = Process.Start(info);
                }
                catch (ArgumentNullException e)
                {
                    App.Logger.Error(e.Message + "\n" + "when: info=" + info.ToString() + "\n" + e.StackTrace);
                    return;
                }
                catch (Win32Exception e)
                {
                    App.Logger.Error(e.Message + "\n" + "when: info=" + info.ToString() + "\n" + e.StackTrace);
                    return;
                }
                catch (ObjectDisposedException e)
                {
                    App.Logger.Error(e.Message + "\n" + "when: info=" + info.ToString() + "\n" + e.StackTrace);
                    return;
                }
                catch (InvalidOperationException e)
                {
                    App.Logger.Error(e.Message + "\n" + "when: info=" + info.ToString() + "\n" + e.StackTrace);
                    return;
                }
                catch (Exception e)
                {
                    App.Logger.Error(e.Message + "\n" + "when:" + info.ToString() + "\n" + e.StackTrace);
                    return;
                }
            }
        }

        public bool DoOpenWebBrowser(string uri)
        {
            Debug.Assert( !string.IsNullOrEmpty(uri), "uri should not be null or empty!");

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();

            info.UseShellExecute = true;
            info.FileName = uri;
            info.Arguments = "";
            info.WorkingDirectory = ".";

            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(this.DoShellCall);
            Thread thread = new Thread(threadStart);
            thread.Start((object)info);
            //App.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(
            //        delegate()
            //        {
            //            this.DoShellCall(info);
            //        })
            //);
            return true;
        }
    }
}
