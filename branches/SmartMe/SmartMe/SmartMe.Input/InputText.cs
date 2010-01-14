using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartMe.Input
{
    /// <summary>
    /// 供查询的文本
    /// </summary>
    public class InputText
    {
        #region fields
        /// <summary>
        /// 输入的文字
        /// </summary>
        private String _text;
        #endregion

        #region properties
        /// <summary>
        /// 输入的文字
        /// </summary>
        public String Text
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
        public InputText(String text)
        {
            if (text == null)
            {
                _text = new String();
            }
            else
            {
                _text = text;
            }
        }
        #endregion
    }
}
