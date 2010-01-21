using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Data;
using SmartMe.Core.Pipeline;
using SmartMe.Core.Record;
using System.Threading;

namespace SmartMe.Core.Test
{
    public class InputQueryRecordManagerTest
    {
        public static void Test()
        {
            InputQueryRecordManager manager =
                new InputQueryRecordManager("result.xml", new TimeSpan(0, 1, 0));

            WriteInputQueryList(manager.QueryList);

            Pipeline.Pipeline pipeline = new Pipeline.Pipeline();
            pipeline.InputTextSubscriberManager.AddSubscriber(manager);

            pipeline.OnInputTextReady(new InputQuery("Wait"));
            pipeline.OnInputTextReady(new InputQuery("苹果"));
            pipeline.OnInputTextReady(new InputQuery("林添"));
            pipeline.OnInputTextReady(new InputQuery("SmartMe"));

            Thread.Sleep(1000);

            WriteInputQueryList(manager.QueryList);

            InputQueryRecordManager anotherManager = new InputQueryRecordManager
                ("result.xml", TimeSpan.MaxValue);

            Thread.Sleep(1000);

            WriteInputQueryList(anotherManager.QueryList);
        }

        private static void WriteInputQueryList(List<InputQuery> queryList)
        {
            Console.WriteLine(queryList.Count);
            foreach (InputQuery query in queryList)
            {
                Console.WriteLine(query.ToString());
            }
            Console.WriteLine("------------------------------------------------");
        }
    }
}
