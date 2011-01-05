using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace SmartMe.Log
{
    public class Logger
    {
        private string _logFilePath = null;
        private string _logDir = "log";
        private bool _isInitialized = false;

        public enum Level
        {
            Normal,
            Warning,
            Error,
            Fatal
        }

        public Logger()
        {
            _logFilePath = CreateLogFilePath();
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
        public void Fatal(string str)
        {
            Message(str, Level.Fatal);
        }

        public void Message(string str, Level level)
        {
            string message = null;
            if (level == Level.Normal)
            {
                message = string.Format("{0}", str);
            }
            else 
            {
                message = string.Format("[{0}]:{1}", level, str);
            }
            System.Console.WriteLine(message);
            Save(message);
        }

        private void Save(string str)
        {
            if ( !Directory.Exists(_logDir))
            {
                Directory.CreateDirectory(_logDir);
            }
            if (File.Exists(_logFilePath))
            {
                using (FileStream fs = new FileStream(_logFilePath, FileMode.Append))
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
                using (FileStream fs = new FileStream(_logFilePath, FileMode.OpenOrCreate))
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

        private string CreateLogFilePath()
        {
            string dateTimeNowToString = DateTime.Now.ToString("yy-MM-dd");
            string logFileName = "[SmartMe_Log]" + dateTimeNowToString + ".txt";
            string path = Path.Combine(_logDir, logFileName);
            return path;
        }

        
    }
}
