using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartyMee.Kernel.Message
{
    public class MessageTester
    {
        public void Run()
        {
            WordList wordList = new WordList();
            WordObserver wordObserver = new WordObserver();

            wordList.Attatch(wordObserver);
            {
                wordList.Messages.Add("Hello");
                wordList.NotifyAll();

                wordList.Messages.Add("Message");


                wordList.Messages.Add("Tester");
                foreach(IObserver ob in wordList.GetObservers())
                {
                    wordList.Notify(ob);
                }
            }
            wordList.Detach(wordObserver);
        }
    }
}

