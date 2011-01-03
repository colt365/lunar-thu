using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Threading;
using SmartMe.Web.Properties;

namespace SmartMe.Web.Crawl
{
	public class Crawler
	{
		public static string Crawl(string query, Encoding encoding)
		{
			HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(query);
			request.Timeout = Settings.Default.CrawlerTimeout;
            request.UserAgent = Settings.Default.CrawlerUserAgent;
            int retryTime = Settings.Default.CrawlerRetryTimes;
            int timeoutInc = Settings.Default.CrawlerRetryTimeoutInc;

			WebResponse response = null;
			Stream resStream =null;
			StreamReader sr = null;
			string result = null;
            for (int i = 0; i < retryTime; ++i)
			{
				try
				{
					response = request.GetResponse();
					resStream = response.GetResponseStream();
					sr = new System.IO.StreamReader(resStream, encoding);
					result = sr.ReadToEnd();
				}
				catch (System.Exception)
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
                    request.Timeout += timeoutInc;
				}
			}
			return result;

		}
	}
}
