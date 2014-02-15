using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Text;


namespace StudentTracker.Core.Extensions
{
	public static class HtmlRequestExtensions
	{
		public static string ToAbsoluteUrl(this HttpRequestBase request, string relativeUrl)
		{
			if (string.IsNullOrEmpty(relativeUrl))
				return relativeUrl;

			if (request == null)
				return relativeUrl;

			Uri url = request.Url;
			string port = url.Port != 80 ? (":" + url.Port) : String.Empty;

			return String.Format("{0}://{1}{2}{3}",
				url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
		}
	}
}