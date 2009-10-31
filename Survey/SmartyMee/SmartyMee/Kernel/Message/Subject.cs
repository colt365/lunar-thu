using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartyMee.Kernel.Message
{
    public abstract class Subject : IObservable
    {
        protected List<IObserver> m_Observers = new List<IObserver>();

        public virtual void Attatch(IObserver observer)
        {
            if (observer != null)
            {
                if (! m_Observers.Contains(observer))
                {
                    m_Observers.Add(observer);
                }
            }
        }

        public virtual void Detach(IObserver observer)
        {
            m_Observers.Remove(observer);
        }

        public virtual void Notify(IObserver observer)
        {
            if (observer != null && m_Observers.Contains(observer))
            {
                observer.Update(this);
            }
        }

        public virtual void NotifyAll()
        {
            foreach (IObserver observer in m_Observers)
            {
                observer.Update(this);
            }
        }

        public virtual List<IObserver> GetObservers()
        {
            return m_Observers;
        }
    }
}
