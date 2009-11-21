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

		public SearchEngineResult Parse(string html, Encoding encoding)
		{
			HTMLparser oP = HtmlParserFactory.GetInstance();
			SearchEngineResult searchResult = new SearchEngineResult();
			oP.SetEncoding(encoding);
			HTMLchunk oChunk = null;
			bool first = false;
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
            throw new NotImplementedException();
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

		}

		private void HandleOpenTag(HTMLchunk oChunk, ref int state)
		{

		}
		private void HandleCloseTag(HTMLchunk oChunk, ref int state)
		{

		}
		private void HandleText(HTMLchunk oChunk, ref int state)
		{

		}
		#endregion
	}
}
