using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartyMee.Kernel.Log
{
    public class Logger
    {
        protected List<string> logList = new List<string>();
        protected string logPath = @".\log.txt";

        public void WriteLine(string str)
        {
            logList.Add(str + "\r\n");
        }

        public void Write(string str)
        {
            logList.Add(str);
        }

        public void Save()
        {
            throw new NotSupportedException();
        }

        public void Load()
        {
            throw new NotSupportedException();
        }
    }
}
