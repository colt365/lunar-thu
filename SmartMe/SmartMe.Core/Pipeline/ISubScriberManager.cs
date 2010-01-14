using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Core.Pipeline
{
    /// <summary>
    /// 用来管理向别人传递消息的实体，每个模块的接口都应该拥有一个
    /// 标准的管理者为这个package当中的SubscriberManager类
    /// 也可以自定义
    /// </summary>
    public interface ISubScriberManager
    {
        #region methods
        /// <summary>
        /// 返回所有的Subscriber
        /// </summary>
        List<ISubScriber> Subscribers { get; }

        /// <summary>
        /// 加入一个subscriber，当参数为null，或者已经加入的时候，不做任何事情。
        /// </summary>
        /// <param name="subscriber">待加入的subscriber</param>
        /// <returns>是否加入了这个subscriber</returns>
        bool AddSubscriber(ISubScriber subscriber);

        /// <summary>
        /// 移除一个subscriber，当参数为null或者没有加入的subscriber时，不做任何事情
        /// </summary>
        /// <param name="subscriber">待移除的subscriber</param>
        /// <returns>是否移除了subscriber</returns>
        bool RemoveSubscriber(ISubScriber subscriber);

        /// <summary>
        /// 移除指定位置的subscriber，位置不在范围内时，不做任何事情
        /// </summary>
        /// <param name="position">移除的位置</param>
        /// <returns>是否移除</returns>
        bool RemoveSubscriberAt(int position);

        /// <summary>
        /// 通知一个subscriber收到了消息，此处不检验这个subscriber是不是在已经subscribe的列表里面！
        /// </summary>
        /// <param name="message">待通知的消息（为null时不做任何事）</param>
        /// <param name="subscriber">被通知的subscriber（为null时不做任何事）</param>
        /// <returns>通知到了subscriber，则返回该管道；否则返回null</returns>
        Pipe Notify(IMessage message, ISubScriber subscriber);

        /// <summary>
        /// 通知所有订阅的subscriber收到消息
        /// </summary>
        /// <param name="message">待通知的消息（为null时不做任何事）</param>
        /// <returns>成功广播了消息，则返回消息队列；否则返回一个空队列</returns>
        List<Pipe> NotifyAll(IMessage message);
        #endregion
    }
}
