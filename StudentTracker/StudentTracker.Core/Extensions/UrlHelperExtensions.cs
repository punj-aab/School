using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Core.Extensions
{
	public static class UrlHelperExtensions
	{
		public static string ActionWithOptionalId(this UrlHelper urlHelper, string actionName, string controllerName)
		{
			string id = urlHelper.RequestContext.RouteData.Values["id"] as string;
			object routeValues = null;
			if (!string.IsNullOrEmpty(id))
			{
				routeValues = new { id = id };
			}
			return urlHelper.Action(actionName, controllerName, routeValues);
		}
	}
}