using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace SmartyMee.Kernel.Log
{
    public class Logger
    {
        private string _logFileName = null;
        private bool _isInitialized = false;

        public enum Level
        {
            Normal,
            Warning,
            Error
        }

        public Logger()
        {
            _logFileName = CreateLogFileString();
        }

        public void Message(string str)
        {
            Message(str, Level.Normal);
        }

        public void Warn(string str)
        {
            Message(str, Level.Warning);
        }

        public void Error(string str)
        {
            Message(str, Level.Error);
        }

        public void Message(string str, Level level)
        {
            string message = string.Format("{0}", str);
            switch (level)
            {
                case Level.Normal: 
                {
                    System.Console.WriteLine(message);
                    Save(message);
                    break;
                }
                case Level.Warning:
                {
                    message = "[Warn]" + message;
                    System.Console.WriteLine(message);
                    Save(message);
                    break;
                }
                case Level.Error:
                {
                    message = "[Error]" + message;
                    System.Console.WriteLine(message);
                    Save(message);
                    break;
                }
            }
        }

        private void Save(string str)
        {
            if (File.Exists(_logFileName))
            {
                using (FileStream fs = new FileStream(_logFileName, FileMode.Append))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8))
                    {
                        if ( !_isInitialized)
                        {
                            string mark = string.Format("======= {0} ======={1}", DateTime.Now.ToString(), Environment.NewLine);
                            bw.Write(Encoding.UTF8.GetBytes(mark));
                            _isInitialized = true;
                        }

                        bw.Write(Encoding.UTF8.GetBytes(str + Environment.NewLine));
                    }
                }
            }
            else
            {
                using (FileStream fs = new FileStream(_logFileName, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8))
                    {
                        if (!_isInitialized)
                        {
                            string mark = string.Format("======= {0} ======={1}", DateTime.Now.ToString(), Environment.NewLine);
                            bw.Write(Encoding.UTF8.GetBytes(mark));
                            _isInitialized = true;
                        }

                        bw.Write(Encoding.UTF8.GetBytes(str + Environment.NewLine));
                    }
                }    
            }
        }

        private string CreateLogFileString()
        {
            string dateTimeNowToString = DateTime.Now.ToString("yy-MM-dd");
            string logFileName = "[SmartMe_Log]" + dateTimeNowToString + ".txt";
            return logFileName;
        }

        public void Load()
        {
            throw new NotSupportedException();
        }
    }
}
