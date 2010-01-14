using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Pipeline
{
    public class SubscriberManager : ISubScriberManager
    {
        #region fields
        private List<ISubScriber> _subscribers = new List<ISubScriber>();
        #endregion

        #region constructors
        #endregion

        #region nest
        #endregion

        #region properties
        public List<ISubScriber> Subscribers
        {
            get
            {
                return _subscribers;
            }
        }
        #endregion

        #region methods
        public bool AddSubscriber(ISubScriber subscriber)
        {
            if (subscriber == null)
            {
                return false;
            }
            if (_subscribers.IndexOf(subscriber) != -1)
            {
                return false;
            }
            _subscribers.Add(subscriber);
            return true;
        }

        public bool RemoveSubscriber(ISubScriber subscriber)
        {
            return _subscribers.Remove(subscriber);
        }

        public bool RemoveSubscriberAt(int position)
        {
            if (position < 0 || position >= _subscribers.Count)
            {
                return false;
            }
            _subscribers.RemoveAt(position);
            return true;
        }

        public Pipe Notify(IMessage message, ISubScriber subscriber)
        {
            if (message == null || subscriber == null)
            {
                return null;
            }
            Pipe pipe = new Pipe(message, subscriber);
            pipe.SendMessage();
            return pipe;
        }

        public List<Pipe> NotifyAll(IMessage message)
        {
            List<Pipe> pipes = new List<Pipe>();
            if (message == null)
            {
                return pipes;
            }
            foreach (ISubScriber subscriber in _subscribers)
            {
                Pipe pipe = new Pipe(message, subscriber);
                pipe.SendMessage();
                pipes.Add(pipe);
            }
            return pipes;
        }
        #endregion
    }
}
