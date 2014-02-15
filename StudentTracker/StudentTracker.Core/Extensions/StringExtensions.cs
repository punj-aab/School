using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Configuration;
using StudentTracker.Core.Entities;

namespace StudentTracker.Core.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Compiled regular expression for performance.
		/// </summary>
		static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

		/// <summary>
		/// Remove HTML from string with compiled Regex.
		/// </summary>
		public static string StripTags(this string source)
		{
			return source == null ? null : _htmlRegex.Replace(source, string.Empty);
		}

        public static string UppercaseFirstLetter(this string value)
        {
            //
            // Uppercase the first letter in the string this extension is called on.
            //
            if (value.Length > 0)
            {
                char[] array = value.ToCharArray();
                array[0] = char.ToUpper(array[0]);
                return new string(array);
            }
            return value;
        }

        public static string Href(this Post post, UrlHelper helper)
        {
            return helper.RouteUrl(new { controller = "Blog", action = "Post", year = post.PostedOn.Year, month = post.PostedOn.Month, title = post.UrlSlug });
        }

        public static string ToConfigLocalTime(this DateTime utcDT)
        {
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();

            var istTZ = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["Timezone"]);
            return String.Format("{0} ({1})", TimeZoneInfo.ConvertTimeFromUtc(utcDT, istTZ).ToShortDateString(), ConfigurationManager.AppSettings["TimezoneAbbr"]);
        }
	}
}