using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SmartMe.Core.Pipeline
{
    /// <summary>
    /// 这是一个用来传递消息的管道，用它来传递消息，以保持线程安全
    /// </summary>
    public class Pipe
    {
        #region fields
        /// <summary>
        /// 消息
        /// </summary>
        private IMessage _message;

        /// <summary>
        /// 接收器
        /// </summary>
        private ISubScriber _subscriber;

        /// <summary>
        /// 传递消息的线程
        /// </summary>
        private Thread _pipeThread;
        #endregion

        #region constructor
        /// <summary>
        /// 建立一个管道
        /// </summary>
        /// <param name="message">传递的消息</param>
        /// <param name="subscriber">被传递的对象</param>
        public Pipe(IMessage message, ISubScriber subscriber)
        {
            _message = message;
            _subscriber = subscriber;
        }
        #endregion

        #region properties
        /// <summary>
        /// 信息
        /// </summary>
        public IMessage Message
        {
            get
            {
                return _message;
            }
        }

        /// <summary>
        /// 接收器
        /// </summary>
        public ISubScriber Subscriber
        {
            get
            {
                return _subscriber;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMessage()
        {
            _pipeThread = new Thread(new ThreadStart(handleMessage));
            _pipeThread.IsBackground = true;
            _pipeThread.Start();
        }

        /// <summary>
        /// 终止消息
        /// </summary>
        public void CancelMessage()
        {
            if (_pipeThread != null)
            {
                _pipeThread.Abort();
                _pipeThread = null;
            }
        }

        private void handleMessage()
        {
            _subscriber.Handle(_message);
        }
        #endregion
    }
}
