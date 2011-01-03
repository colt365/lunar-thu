using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Majestic12;
using SmartMe.Core.Data;

namespace SmartMe.Web.Parse
{
    class DictCnParser
    {

        #region IParser Members
        DictResult dictResult;
        int divCount = 0;
        public SmartMe.Core.Data.DictResult Parse ( string html, Encoding encoding )
        {
            dictResult = new DictResult();
            HTMLparser oP = HtmlParserFactory.GetInstance();

            dictResult.DictType = DictType.Dict_cn;
			
			oP.Init(encoding.GetBytes(html));
			oP.SetEncoding(encoding);
			HTMLchunk oChunk = null;

			int state = 0;
			bool bEncodingSet = false;
			while ((oChunk = oP.ParseNext()) != null)
			{

				switch (oChunk.oType)
				{
					case  HTMLchunkType.OpenTag:
						HandleOpenTag(oChunk, ref state);

				        // printParams:
						if (oChunk.sTag == "meta")
						{
							HandleMetaEncoding(oP, oChunk, ref bEncodingSet);
						};
					    HandleParam(oChunk, ref state);


					break;

					case HTMLchunkType.CloseTag:
					HandleCloseTag(oChunk, ref state);
						break;

					case HTMLchunkType.Text:
						HandleText(oChunk, ref state);
						break;

					default:
						break;
				}
			}
	
            return dictResult;
        }


        private void HandleParam ( HTMLchunk oChunk, ref int state )
        {

            if ( oChunk.iParams > 0 )
            {
                for ( int i = 0; i < oChunk.iParams; i++ )
                {
                    switch ( oChunk.cParamChars[i] )
                    {
                        default:
                            if ( oChunk.sValues[i] == "main_right_left" && oChunk.sParams[i] == "id" && state == 0 )
                            {
                                state = 1;

                            }
                            else if ( oChunk.sValues[i] == "word" && oChunk.sParams[i] == "id" && state > 0 )
                            {
                                state = 2;
                            }
                            else if ( oChunk.sValues[i] == "pron" && oChunk.sParams[i] == "id" && state ==3 )
                            {
                                state = 4;
                            }
                            else if ( oChunk.sValues[i] == "exp_exp" && oChunk.sParams[i] == "id" && state>2 )
                            {
                                state =6;
                                divCount = 1;
                            }
                            else if ( oChunk.sValues[i] == "exp_eg" && oChunk.sParams[i] == "id" && state>2 )
                            {
                                state = 8;
                            }
                            else if ( oChunk.sValues[i] == "exp_tran" && oChunk.sParams[i] == "id" && state > 2 )
                            {
                                state = 11;
                            }
                            else if ( oChunk.sValues[i] == "exp_eee" && oChunk.sParams[i] == "id" && state > 2 )
                            {
                                state = 14;
                                divCount = 1;
                            }
                            else if ( oChunk.sValues[i] == "exp_baike" && oChunk.sParams[i] == "id" && state > 2 )
                            {
                                state = 16;
                                divCount = 1;
                            }
                            
                            break;
                    }
                }

            }

        }

        private void HandleOpenTag ( HTMLchunk oChunk, ref int state )
        {
            
            if ( oChunk.sTag == "ol" && state==8 )
            {
                state =9;
            }
            else if ( oChunk.sTag == "table" && state == 11 )
            {
                state = 12;
            }else if( oChunk.sTag== "div" && ( state==6 || state== 14 || state ==16) )
            {
                ++divCount;
            }
            
           
        }
        private void HandleCloseTag ( HTMLchunk oChunk, ref int state )
        {

            if (   state == 2  )
            {
                state += 1;
            }
            else if ( state ==4 )
            {
                state = 5;
            }
            
            else if ( oChunk.sTag == "ol" && ( state == 9 ) )
            {
                state = 10;
            }
            else if ( oChunk.sTag == "table" && state == 12 )
            {
                state = 13;
            }
            else if ( oChunk.sTag == "div" && ( state==6 || state == 14 || state == 16 ) )
            {
                if(--divCount==0)
                {
                    state +=1;
                }
            }
            
           

        }
        private void HandleText ( HTMLchunk oChunk, ref int state )
        {
            if ( state == 2 )
            {
                dictResult.Word += ( oChunk.oHTML.Trim(new char[] { ' ', '\t', '\r', '\n' }) );
            }
            else if ( state == 4 )
            {
                dictResult.Pronunciation += ( System.Web.HttpUtility.HtmlDecode( oChunk.oHTML ) );
            }
            else if ( state == 6 )
            {
               
                dictResult.ChineseExplanations += ( oChunk.oHTML.Trim(new char[] { ' ', '\t', '\r', '\n' }) );
            }
           
            else if ( state == 12 )
            {
                dictResult.Variations += ( oChunk.oHTML.Trim( new char[] { ' ', '\t', '\r', '\n' } ) );
            }
           
            else if ( state == 14 )
            {
                dictResult.EnglishExplanations += ( oChunk.oHTML.Trim( new char[] { ' ', '\t', '\r', '\n' } ) );
            }
            else if ( state == 9 )
            {
                dictResult.Examples += ( (oChunk.oHTML.Trim( new char[] { ' ', '\t', '\r', '\n' } ) ));
            }
            else if ( state == 16 )
            {
                dictResult.FromEncyclopedia += ( oChunk.oHTML.Trim( new char[] { ' ', '\t', '\r', '\n' } ) );
            }
           
           

        }

        private void HandleMetaEncoding ( HTMLparser oP, HTMLchunk oChunk, ref bool bEncodingSet )
        {
            // if encoding already set then we should not be trying to set new one
            // this is the logic that major browsers follow - the first Encoding is assumed to be 
            // the correct one
            if ( bEncodingSet )
                return;

            if ( HTMLparser.HandleMetaEncoding( oP, oChunk, ref bEncodingSet ) )
            {
                if ( !bEncodingSet )
                    Console.WriteLine( "Failed to set encoding from META: {0}", oChunk.GenerateHTML() );
            }
        }



        #endregion
    }
}
