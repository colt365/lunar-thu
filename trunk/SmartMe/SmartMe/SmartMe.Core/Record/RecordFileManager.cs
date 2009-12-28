using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SmartMe.Core.Record
{
    public class RecordFileManager : IRecordFileManager
    {
        #region fields
        private XmlSerializer _xmlSerializer;
        #endregion

        #region constructors
        /// <summary>
        /// 默认的构造函数，构造指定类型的记录管理器
        /// </summary>
        /// <param name="type">记录类型</param>
        public RecordFileManager(Type type)
        {
            Type = type;
        }
        #endregion

        #region properties
        /// <summary>
        /// 管理的数据类型
        /// </summary>
        public Type Type
        {
            set
            {
                _xmlSerializer = new XmlSerializer(value);
            }
        }
        #endregion

        #region methods

        #region IRecordFileManager Members

        public void SaveToFile(object record, string filePath)
        {
            StreamWriter fileWriter = new StreamWriter(filePath);
            try
            {
                _xmlSerializer.Serialize(fileWriter, record);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                fileWriter.Close();
            }
        }

        public object ReadFromFile(string filePath)
        {
            try
            {
                StreamReader fileReader = new StreamReader(filePath);
                try
                {
                    object result = _xmlSerializer.Deserialize(fileReader);
                    return result;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    fileReader.Close();
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
        }

        public string SaveToXmlString(object record)
        {
            StringBuilder xmlString = new StringBuilder();
            StringWriter xmlStringWriter = new StringWriter(xmlString);
            _xmlSerializer.Serialize(xmlStringWriter, record);
            xmlStringWriter.Close();
            return xmlString.ToString();
        }

        public object ReadFromXmlString(string xmlString)
        {
            StringReader xmlStringReader = new StringReader(xmlString);
            object result = _xmlSerializer.Deserialize(xmlStringReader);
            xmlStringReader.Close();
            return result;
        }

        #endregion

        #endregion
    }
}
