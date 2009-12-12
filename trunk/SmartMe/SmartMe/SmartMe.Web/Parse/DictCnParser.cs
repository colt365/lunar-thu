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
        public SmartMe.Core.Data.DictResult Parse ( string html, Encoding encoding )
        {
            dictResult = new DictResult();
            HTMLparser oP = HtmlParserFactory.GetInstance();

            dictResult.DictionaryType = DictionaryType.Dict_cn;
			
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

				printParams:
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
                            if ( oChunk.sValues[i] == "mutti" && oChunk.sParams[i] == "class" && state == 0 )
                            {
                                state = 1;

                            }
                            else if ( oChunk.sValues[i] == "pronounce" && oChunk.sParams[i] == "class" )
                            {
                                state = 5;
                            }
                            else if ( oChunk.sValues[i] == "mut_jies" && oChunk.sParams[i] == "class" )
                            {
                                state = 7;
                            }
                            else if ( oChunk.sValues[i] == "mut_ol" && oChunk.sParams[i] == "class" && ( state == 12 || state == 20 ) )
                            {
                                state += 1;
                            }
                            else if ( oChunk.sValues[i] == "more" && oChunk.sParams[i] == "class" && state == 17 )
                            {
                                state = 18;
                            }
                            break;
                    }
                }

            }

        }

        private void HandleOpenTag ( HTMLchunk oChunk, ref int state )
        {

            if ( oChunk.sTag == "td" && ( state == 1 || state == 10 ) )
            {
                state += 1;
            }
            else if ( oChunk.sTag == "ul" && state == 6 )
            {
                state = 7;
            }
            else if ( oChunk.sTag == "span" && state == 3 )
            {
                state = 4;
            }
            else if ( oChunk.sTag == "strong" && state == 7 )
            {
                state = 8;
            }
            else if ( oChunk.sTag == "li" && ( state == 13 || state == 21 ) )
            {
                state += 1;
            }
            else if ( oChunk.sTag == "div" && state == 16 )
            {
                state += 1;
            }
        }
        private void HandleCloseTag ( HTMLchunk oChunk, ref int state )
        {
            if ( oChunk.sTag == "td" && ( state == 2 ) )
            {
                state += 1;
            }
            else if ( oChunk.sTag == "span" && state == 5 )
            {
                state = 6;
            }
            else if ( state == 8 && oChunk.sTag == "strong" )
            {
                state = 9;
            }
            else if ( oChunk.sTag == "td" && ( state == 11 ) )
            {
                state = 10;
            }
            else if ( oChunk.sTag == "li" && state == 14 )
            {
                state -= 1;
            }
            else if ( oChunk.sTag == "div" && ( state == 18 ) )
            {
                state += 1;
            }
            else if ( oChunk.sTag == "ol" && ( state == 22 ) )
            {
                state += 1;
            }

        }
        private void HandleText ( HTMLchunk oChunk, ref int state )
        {
            if ( state == 2 )
            {
                dictResult.Word += ( oChunk.oHTML );
            }
            else if ( state == 5 )
            {
                dictResult.Pronunciation += ( oChunk.oHTML );
            }
            else if ( state == 8 )
            {
                dictResult.ChineseExplanations += ( oChunk.oHTML );
            }
            else if ( oChunk.oHTML == "词形变化:" )
            {
                state = 10;
            }
            else if ( state == 11 )
            {
                dictResult.Variations += ( oChunk.oHTML );
            }
            else if ( oChunk.oHTML == "英英解释:" )
            {
                state = 12;
            }
            else if ( state == 14 )
            {
                dictResult.EnglishExplanations += ( oChunk.oHTML );
            }
            else if ( state == 22 )
            {
                dictResult.Examples += ( oChunk.oHTML );
            }
            else if ( oChunk.oHTML == "互动百科:" )
            {
                state = 16;
            }
            else if ( state == 18 )
            {
                dictResult.FromEncyclopedia += ( oChunk.oHTML );
            }
            else if ( oChunk.oHTML == "例句与用法:" )
            {
                state = 20;
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
