using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartyMee.Kernel.Message
{
    interface IObserver
    {
        void Update(Subject s);
    }

    public abstract class Subject
    {
        protected List<IObserver> observers = new List<IObserver>();

        public virtual void Attatch(IObserver observer)
        {
            if (observer != null)
            {
                observers.Add(observer);
            }
        }

        public virtual void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        public virtual void Notify()
        {
            foreach(IObserver observer in observers)
            {
                observer.Update(s);
            }
        }
    }
}
