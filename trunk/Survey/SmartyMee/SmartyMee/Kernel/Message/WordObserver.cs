using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace SmartyMee.Kernel.Message
{
    public class WordObserver : IObserver
    {
        public void Update(Subject s)
        {
            WordList wordList = s as WordList;
            string str = string.Format("{0}:{1}", wordList.Count, wordList.Peek());
            App.Log.WriteLine(str);
        }
    }
}
