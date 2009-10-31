using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartyMee.Kernel.Message
{
    public interface IObserver
    {
        void Update(Subject s);
    }

    public interface IObservable
    {
        void Attatch(IObserver observer);
        void Detach(IObserver observer);
        void Notify(IObserver observer);
        void NotifyAll();
        List<IObserver> GetObservers();
    }
}
