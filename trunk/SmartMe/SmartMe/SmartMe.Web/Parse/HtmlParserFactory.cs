using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Majestic12;

namespace SmartMe.Web.Parse
{
	internal class HtmlParserFactory
	{
		internal static HTMLparser GetInstance()
		{
			HTMLparser oP = new HTMLparser();

			// This is optional, but if you want high performance then you may
			// want to set chunk hash mode to FALSE. This would result in tag params
			// being added to string arrays in HTMLchunk object called sParams and sValues, with number
			// of actual params being in iParams. See code below for details.
			//
			// When TRUE (and its default) tag params will be added to hashtable HTMLchunk (object).oParams
			oP.SetChunkHashMode(false);

			// if you set this to true then original parsed HTML for given chunk will be kept - 
			// this will reduce performance somewhat, but may be desireable in some cases where
			// reconstruction of HTML may be necessary
			oP.bKeepRawHTML = true;

			// if set to true (it is false by default), then entities will be decoded: this is essential
			// if you want to get strings that contain final representation of the data in HTML, however
			// you should be aware that if you want to use such strings into output HTML string then you will
			// need to do Entity encoding or same string may fail later
			oP.bDecodeEntities = true;

			// we have option to keep most entities as is - only replace stuff like &nbsp; 
			// this is called Mini Entities mode - it is handy when HTML will need
			// to be re-created after it was parsed, though in this case really
			// entities should not be parsed at all
			oP.bDecodeMiniEntities = true;

			if (!oP.bDecodeEntities && oP.bDecodeMiniEntities)
				oP.InitMiniEntities();

			// if set to true, then in case of Comments and SCRIPT tags the data set to oHTML will be
			// extracted BETWEEN those tags, rather than include complete RAW HTML that includes tags too
			// this only works if auto extraction is enabled
			oP.bAutoExtractBetweenTagsOnly = true;

			// if true then comments will be extracted automatically
			oP.bAutoKeepComments = true;

			// if true then scripts will be extracted automatically: 
			oP.bAutoKeepScripts = true;

			// if this option is true then whitespace before start of tag will be compressed to single
			// space character in string: " ", if false then full whitespace before tag will be returned (slower)
			// you may only want to set it to false if you want exact whitespace between tags, otherwise it is just
			// a waste of CPU cycles
			oP.bCompressWhiteSpaceBeforeTag = true;

			// if true (default) then tags with attributes marked as CLOSED (/ at the end) will be automatically
			// forced to be considered as open tags - this is no good for XML parsing, but I keep it for backwards
			// compatibility for my stuff as it makes it easier to avoid checking for same tag which is both closed
			// or open
			oP.bAutoMarkClosedTagsWithParamsAsOpen = false;

			return oP;
		}
	}
}
