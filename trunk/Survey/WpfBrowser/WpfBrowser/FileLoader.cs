using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WpfBrowser
{
    public static class FileLoader
    {
        public static bool LoadScript(string filepath, out string content, out string error)
        {
            content = "";
            error = "";
            bool isOk = false;
            try 
            {
                using (StreamReader streamReader = new StreamReader(
                                        new FileStream(filepath, FileMode.Open)))
                {
                    content = streamReader.ReadToEnd();
                }
                isOk = true;
            } 
            catch (Exception e)
            {
                error = e.Message;
            }
            return isOk;
        }
    }
}
