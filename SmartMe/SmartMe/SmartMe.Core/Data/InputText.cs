using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Data
{
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
        /// 
        /// </summary>
        private InputType _type = InputType.Unknown;

        public InputType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        #endregion

        #region nested
        public enum InputType
        {
            Unknown,
            FileName,
            HttpUri,
            FtpUri,
            Text,
        }
        #endregion

        #region properties
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

        #region methods
        public override string ToString()
        {
            return Text;
        }
        #endregion
    }
}
