using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Record;
using SmartMe.Core.Test;
using System.IO;

namespace SmartMe.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //InputQueryRecordManagerTest.Test();
            QueryResultRecordManagerTest.Test();
            //XMLTest.Test();

            //Test();
        }

        static void Test()
        {
            DateTime date = QueryResultRecordManager.GetDate("2009-09-08");
            Console.WriteLine(date);

            string[] allDirectories = Directory.GetDirectories("data");
            foreach (string dir in allDirectories)
            {
                string fileName = Path.GetFileName(dir);
                Console.WriteLine(fileName);
            }
        }
    }
}
