using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Data
{
    public enum InputQueryType
    {
        Unknown,
        FileName,
        HttpUri,
        FtpUri,
        Text,
    }
    
    /// <summary>
    /// 供查询的文本
    /// </summary>
    public class InputQuery : IMessage
    {
        #region fields
        /// <summary>
        /// 输入的文字
        /// </summary>
        private string _text;

        /// <summary>
        /// 输入文本类型
        /// </summary>
        private InputQueryType _type = InputQueryType.Unknown;

        /// <summary>
        /// 消息类型
        /// </summary>
        private const MessageType _messageType = MessageType.InputQuery;
        #endregion

        #region constructor
        /// <summary>
        /// 输入的文字的构造函数
        /// </summary>
        /// <param name="text">输入的文字</param>
        public InputQuery(String text)
        {
            if (text == null)
            {
                _text = "";
            }
            else
            {
                _text = text;
            }
        }
        #endregion

        #region properties
        public InputQueryType QueryType
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public MessageType Type
        {
            get
            {
                return _messageType;
            }
        }

        /// <summary>
        /// 输入的文字
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }
        #endregion

        #region methods
        public override string ToString()
        {
            return Text;
        }
        #endregion
    }
}
