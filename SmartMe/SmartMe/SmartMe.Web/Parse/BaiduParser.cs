using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Majestic12;
using SmartMe.Core.Data;


namespace SmartMe.Web.Parse
{
	class BaiduParser : IParser
	{

		#region IParser Members
		SearchEngineResult searchResult;
		SearchEngineResult.ResultItem item;
		public SearchEngineResult Parse(string html, Encoding encoding)
		{
			HTMLparser oP = HtmlParserFactory.GetInstance();
			searchResult = new SearchEngineResult();
            searchResult.SearchEngine = SearchEngineType.Baidu;
			item = new SearchEngineResult.ResultItem();
			//item.Source = "Baidu";
			oP.Init(encoding.GetBytes(html));
			oP.SetEncoding(encoding);
			HTMLchunk oChunk = null;
		
			int state = 0;
			bool bEncodingSet = false;
			while ((oChunk = oP.ParseNext()) != null)
			{

				switch (oChunk.oType)
				{
					case HTMLchunkType.OpenTag:
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
							if (oChunk.sValues[i] == "f" && oChunk.sParams[i] == "class" && state == 2)
							{
								state = 3;
								if (item.Url != null && item.Url != "")
								{
									searchResult.Results.Add(item);
									item = new SearchEngineResult.ResultItem();
									//item.Source = "Baidu";
								}
							}
							else if (oChunk.sParams[i] == "href")
							{
								if (state == 4)
								{
									item.Url = oChunk.sValues[i];
								}
								else if (state == 6)
								{
									item.CacheUrl = oChunk.sValues[i];
								}
								else if (state == 8)
								{
									if (item.SimilarUrl == null || item.SimilarUrl == "")
										item.SimilarUrl = oChunk.sValues[i];
									else
									{
										item.CacheUrl = item.SimilarUrl;
										item.SimilarUrl = oChunk.sValues[i];
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
			if (oChunk.sTag == "tr")
			{
				state = 1;
			}
			else if (oChunk.sTag == "td" && state > 0)
			{
				state = 2;
			}
			else if (oChunk.sTag == "a")
			{
				if (state == 3 || state == 5 || state == 7)
					state += 1;
				else if (state == 9)
				{
					state = 8;
				}
			}
		}
		private void HandleCloseTag(HTMLchunk oChunk, ref int state)
		{
			if (oChunk.sTag == "tr")
			{
				state = 0;
			}
			else if (oChunk.sTag == "td")
			{
				state = 1;
			}
			else if (oChunk.sTag == "a")
			{
				if (state == 4 || state == 6 || state == 8)
					state += 1;

			}
		}
		private void HandleText(HTMLchunk oChunk, ref int state)
		{
			if (state == 4)
			{
				item.Title += oChunk.oHTML;
			}
			else if (state == 5)
			{
				item.Description += oChunk.oHTML;
			}
		}
		#endregion
	}
}
