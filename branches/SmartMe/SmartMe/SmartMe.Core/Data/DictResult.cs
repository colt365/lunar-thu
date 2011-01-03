using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartMe.Core.Pipeline;

namespace SmartMe.Core.Data
{

    public enum DictType
    {
        Dict_cn,
        Other
    }
    [Serializable]
    public class DictResult:IQueryResultItem
    {
        public DictResult()
        {
            _type = QueryResultItemType.DictResult;
        }

        #region IQueryResultItem Members

        public new QueryResultItemType ResultType
        {
            get
            {
                return QueryResultItemType.DictResult;
            }
            set
            {
                _resultType = value;
            }
        }
        public override string ToString ( )
        {
            return "Word:" + _word + "\n" +
                   "Examples:" + _examples + "\n" +
                   "englishExplanations:" + _englishExplanations + "\n" +
                   "ChineseExplanations:" + _chineseExplanations + "\n" +
                   "FromEncyclopedia:" + _fromEncyclopedia + "\n" +
                   "Pronunciation:" + _pronunciation + "\n" +
                   "Variations:" + _variations;
        }
        #endregion

        #region IMessage Members

        public new MessageType MessageType
        {
            get
            {
                return MessageType.QueryResultItem;
            }
            set
            {
                _messageType = value;
            }

        }

        #endregion

        #region fields
        private string _searchUrl;
        private DictType _dictionary;

       
        private string _word="";
        private string _examples = "";
        private string _englishExplanations="";
        private string _chineseExplanations="";
        private string _fromEncyclopedia="";
        private string _pronunciation="";
        private string _variations = "";
        private QueryResultItemType _resultType;
        private MessageType _messageType;
        #endregion

        #region properties
        public DictType DictType
        {
            get
            {
                return _dictionary;
            }
            set
            {
                _dictionary = value;
            }
        }
        public string SearchUrl
        {
            get
            {
                return _searchUrl;
            }
            set
            {
                _searchUrl = value;
            }
        }

        public string Word
        {
            get
            {
                return _word;
            }
            set
            {
                _word = value;
            }
               
        }
        public string Examples
        {
            get
            {
                return _examples;
            }
            set
            {
                _examples = value;
            }

        }
        public string EnglishExplanations
        {
            get
            {
                return _englishExplanations;
            }
            set
            {
                _englishExplanations = value;
            }

        }
        public string ChineseExplanations
        {
            get
            {
                return _chineseExplanations;
            }
            set
            {
                _chineseExplanations = value;
            }

        }
        public string FromEncyclopedia
        {
            get
            {
                return _fromEncyclopedia;
            }
            set
            {
                _fromEncyclopedia = value;
            }

        }
        public string Pronunciation
        {
            get
            {
                return _pronunciation;
            }
            set
            {
                _pronunciation = value;
            }

        }
        public string Variations
        {
            get
            {
                return _variations;
            }
            set
            {
                _variations = value;
            }

        }
        

        #endregion


    }
}
