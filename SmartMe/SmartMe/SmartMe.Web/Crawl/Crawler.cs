﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Threading;

namespace SmartMe.Web.Crawl
{
	public class Crawler
	{
		public static string Crawl(string query,Encoding encoding)
		{
			HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(query);
			request.Timeout = 5000;
			request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";

			WebResponse response = null;
			Stream resStream =null;
			StreamReader sr = null;
			string result = null;
			for (int i = 0; i < 5; ++i)
			{
				try
				{
					response = request.GetResponse();
					resStream = response.GetResponseStream();
					sr = new System.IO.StreamReader(resStream, encoding);
					result = sr.ReadToEnd();
				}
				catch (System.Exception e)
				{

					result = null;
				}
				finally
				{
					if (sr != null)
					{
						sr.Close();
					}
					if (resStream != null)
					{
						resStream.Close();
					}
					if (response != null)
					{
						response.Close();
					}

				}
				if (result != null)
				{
					break;
				}
				else
				{
					
					request.Timeout += 500;
				}
			}
			return result;

		}
	}
}
