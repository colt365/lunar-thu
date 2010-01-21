using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Data
{
    public abstract class Message : IMessage
    {
        #region fields

        private DateTime _time;

        #endregion

        #region constructors

        public Message()
        {
            Time = DateTime.Now;
        }

        #endregion

        #region properties

        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }

        #region IMessage Members

        public abstract MessageType MessageType 
        {
            get;
        }

        #endregion

        #endregion
    }
}
