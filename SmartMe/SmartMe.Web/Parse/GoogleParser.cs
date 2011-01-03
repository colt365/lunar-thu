using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Majestic12;
using SmartMe.Core.Data;


namespace SmartMe.Web.Parse
{
	class GoogleParser: IParser
	{

		#region IParser Members

		SearchEngineResult searchResult;
		SearchEngineResult.ResultItem item;

		public SearchEngineResult Parse(string html, Encoding encoding)
		{
			
			HTMLparser oP = HtmlParserFactory.GetInstance();
			searchResult = new SearchEngineResult();
            searchResult.SearchEngineType = SearchEngineType.Google;
			item = new SearchEngineResult.ResultItem();
			//item.Source = "Google";
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
			return searchResult;
		}

		private void HandleMetaEncoding(HTMLparser oP, HTMLchunk oChunk, ref bool bEncodingSet)
		{
			// if encoding already set then we should not be trying to set new one
			// this is the logic that major browsers follow - the first Encoding is assumed to be 
			// the correct one
			if (bEncodingSet)
				return;

			if (HTMLparser.HandleMetaEncoding(oP, oChunk, ref bEncodingSet))
			{
				if (!bEncodingSet)
					Console.WriteLine("Failed to set encoding from META: {0}", oChunk.GenerateHTML());
			}
		}

		private void HandleParam(HTMLchunk oChunk, ref int state)
		{
			if (oChunk.iParams > 0)
			{
				for (int i = 0; i < oChunk.iParams; i++)
				{
					switch (oChunk.cParamChars[i])
					{
						
						default:
							if (oChunk.sValues[i] == "g" && oChunk.sParams[i] == "class" && state == 2)
							{
								state = 3;
								if (item.Url!=null && item.Url!="")
								{
									searchResult.Items.Add(item);
									item = new SearchEngineResult.ResultItem();
									//item.Source = "Google";
								}
							}else if(oChunk.sValues[i] == "r" && oChunk.sParams[i] == "class" && state == 3)
							{
								state = 4;
							}else if(oChunk.sValues[i] == "s" && oChunk.sParams[i] == "class" && state == 6)
							{
								state = 7;
							}
							else if (oChunk.sValues[i] == "gl" && oChunk.sParams[i] == "class" && state == 7)
							{
								state = 8;
							}
							else if (oChunk.sParams[i] == "href")
							{
								if (state == 5)
								{
									item.Url = oChunk.sValues[i];
								}
								else if (state == 9 || state == 11)
								{
									if (oChunk.sValues[i].IndexOf("q=related")!=-1)
									{
										item.SimilarUrl = oChunk.sValues[i];
									}
                                    else if (oChunk.sValues[i].IndexOf("q=cache") != -1)
									{
										item.CacheUrl = oChunk.sValues[i];
									}
								}
								
							}
							break;
					}
				}

			}
		}

		private void HandleOpenTag(HTMLchunk oChunk, ref int state)
		{
			if (oChunk.sTag == "ol")
			{
				state = 1;
			}
			else if (oChunk.sTag == "li" && state > 0 )
			{
				state = 2;
			}
			else if(oChunk.sTag== "a")
			{
				if (state == 4 || state == 8 || state == 10)
					state += 1;
				/*else if (state == 9)
				{
					state = 8;
				}*/
			}

		}
		private void HandleCloseTag(HTMLchunk oChunk, ref int state)
		{
			if(oChunk.sTag=="ol")
			{
				state = 0;
			}
			else if(oChunk.sTag== "a")
			{
				if (state == 5 || state == 9 || state == 11)
					state += 1;
			}
		}
		private void HandleText(HTMLchunk oChunk, ref int state)
		{
			if (state == 5)
			{
				item.Title += oChunk.oHTML;
			}
			else if (state == 7)
			{
				item.Description += oChunk.oHTML;
			}
		}
		#endregion
	}
}
