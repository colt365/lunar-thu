using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartyMee.Kernel.Message
{
    public abstract class MessageContainer<TMessage> : Subject
    {
        protected List<TMessage> m_Messages = new List<TMessage>();
        public List<TMessage> Messages
        {
            get { return m_Messages; }
            set { m_Messages = value; }
        }
        public int Count
        {
            get { return m_Messages.Count; }
        }

        protected bool m_IsReadOnly = false;
        public bool IsReadOnly
        {
            get { return m_IsReadOnly; }
            set { m_IsReadOnly = value; }
        }

        #region Queue Method
        public virtual TMessage Peek()
        {
            if (m_Messages.Count == 0)
            {
                return default(TMessage);
            }
            else 
            {
                return m_Messages[m_Messages.Count - 1];
            }
        }

        public virtual void Push(TMessage message)
        {
            if (message != null)
            {
                m_Messages.Add(message);
            }
        }

        public virtual void Clear()
        {
            m_Messages.Clear();    
        }

        public virtual TMessage Pop()
        {
            TMessage message = Peek();
            int count = m_Messages.Count;
            if (count > 0)
            {
                m_Messages.RemoveAt(count - 1);
            }
            else 
            {
                throw new InvalidOperationException(
                     "Cannot pop an empty stack");
            }
            return message;
        }
        #endregion Queue Method
    }
}
