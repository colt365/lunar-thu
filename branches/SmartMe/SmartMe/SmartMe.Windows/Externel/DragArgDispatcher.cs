using System;
using System.Collections.Generic;
using System.Text;

using SmartMe.Core;
using SmartMe.Core.Data;

namespace SmartMe.Windows.Externel
{
    public class DragArgDispatcher
    {
        public bool TryGetQuery(System.Windows.DragEventArgs e, ref string query, ref InputQueryType type)
        {
            StringBuilder sb = new StringBuilder();
            query = "";
            type = InputQueryType.Unknown;
            if (e.Data.GetDataPresent("Text", true))
            {
                sb.AppendLine("Text:" + e.Data.GetData("Text", true));
                query = e.Data.GetData("Text", true).ToString();
                type = InputQueryType.Text;
                return true;
            }
            if (e.Data.GetDataPresent("text/html", true))
            {
                sb.AppendLine("text/html:" + e.Data.GetData("text/html", true));
            }
            if (e.Data.GetDataPresent("text/x-moz-url", true))
            {
                sb.AppendLine("text/x-moz-url:" + e.Data.GetData("text/x-moz-url", true));
            }
            if (e.Data.GetDataPresent("text/html", true))
            {
                sb.AppendLine("text/html:" + e.Data.GetData("text/html", true));
            }
            if (e.Data.GetDataPresent("HTML Format", true))
            {
                sb.AppendLine("HTML Format:" + e.Data.GetData("HTML Format", true));
            }
            if (e.Data.GetDataPresent("UniformText", true))
            {
                sb.AppendLine("UniformText:" + e.Data.GetData("UniformText", true));
            }
            if (e.Data.GetDataPresent("FileName", true))
            {
                sb.AppendLine("FileName:" + e.Data.GetData("FileName", true));
            }
            if (e.Data.GetDataPresent("FileNameW", true))
            {
                sb.AppendLine("FileNameW:" + e.Data.GetData("FileNameW", true));
            }
            if (e.Data.GetDataPresent("FileName", true))
            {
                sb.AppendLine("FileName:" + string.Join("; ", (String[])e.Data.GetData("FileName", true)));
            }
            if (e.Data.GetDataPresent("FileNameW", true))
            {
                sb.AppendLine("FileNameW:" + string.Join("; ", (String[])e.Data.GetData("FileNameW", true)));
                object data = e.Data.GetData("FileNameW", true);
                String[] strs = data as String[];
                if (strs != null && strs.Length > 0)
                {
                    string filepath = strs[strs.Length - 1];
                    try
                    {
                        filepath = System.IO.Path.GetFileName(filepath);
                    }
                    catch (System.Exception exception) {
                        App.Logger.Warn("Cannot handle: " + filepath);
                        App.Logger.Warn(exception.Message);
                        App.Logger.Warn(exception.StackTrace);
                    }
                    query = filepath;
                    type = InputQueryType.FileName;
                    return true;
                }
            }
            if (e.Data.GetDataPresent("UniformResourceLocator", true))
            {
                sb.AppendLine("UniformResourceLocator:" + e.Data.GetData("UniformResourceLocator", true));
            }
            if (e.Data.GetDataPresent("UniformResourceLocatorW", true))
            {
                sb.AppendLine("UniformResourceLocatorW:" + e.Data.GetData("UniformResourceLocatorW", true));
                query = e.Data.GetData("UniformResourceLocatorW", true).ToString();
                type = InputQueryType.HttpUri;
                return true;
            }
            return false;
        }
    }
}
