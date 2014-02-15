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
    public static class HtmlHelperExtensions
    {
        //public static MvcHtmlString DisplayInformationMessage(this HtmlHelper helper)
        //{
        //    string message = helper.ViewContext.TempData[Constants.ST_TEMPDATA_INFORMATIONMESSAGE] as string;
        //    string htmlMessage = "";
        //    if (!string.IsNullOrEmpty(message))
        //    {
        //        htmlMessage = string.Format(@"<div class='informationmessage autoslidedown'>{0}</div>", message);

        //    }
        //    return new MvcHtmlString(htmlMessage);
        //}

        //public static MvcHtmlString DisplayErrorMessage(this HtmlHelper helper)
        //{
        //    string message = helper.ViewContext.TempData[Constants.ST_TEMPDATA_ERRORMESSAGE] as string;
        //    string htmlMessage = "";
        //    if (!string.IsNullOrEmpty(message))
        //    {
        //        htmlMessage = string.Format(@"<div class='errormessage autoslidedown'>{0}</div>", message);
        //    }
        //    return new MvcHtmlString(htmlMessage);
        //}

        //public static MvcHtmlString ActionImage(this HtmlHelper html, string action, string controller, string imagePath, object htmlAttributes, object routeValues)
        //{
        //    var url = new UrlHelper(html.ViewContext.RequestContext);
        //    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

        //    // build the <img> tag
        //    var imgBuilder = new TagBuilder("img");
        //    imgBuilder.MergeAttribute("src", url.Content(imagePath));
        //    foreach (var attr in attributes) { imgBuilder.MergeAttribute(attr.Key, attr.Value.ToString()); }
        //    string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

        //    // build the <a> tag
        //    var anchorBuilder = new TagBuilder("a");
        //    anchorBuilder.MergeAttribute("href", url.Action(action, controller, routeValues));
        //    anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside
        //    string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

        //    return MvcHtmlString.Create(anchorHtml);
        //}

        //public static MvcHtmlString ActionImage(this HtmlHelper html, string action, string controller, string imagePath, object htmlAttributes)
        //{
        //    return ActionImage(html, action, controller, imagePath, htmlAttributes, new { });
        //}



        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(
                        this HtmlHelper<TModel> htmlHelper,
                        Expression<Func<TModel, TProperty>> expression,
                        IEnumerable<SelectListItem> listOfValues)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();

            if (listOfValues != null)
            {
                foreach (SelectListItem item in listOfValues)
                {
                    var id = string.Format(
                            "{0}_{1}",
                            metaData.PropertyName,
                            item.Value
                    );

                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id }).ToHtmlString();
                    sb.AppendFormat(
                            "<label for=\"{0}\">{1}</label> {2} ",
                            id,
                            radio,
                            HttpUtility.HtmlEncode(item.Text)
                    );
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        //public static MvcHtmlString CurrentEasoName(this HtmlHelper helper)
        //{
        //	EasoCacheDTO easo = helper.ViewContext.RequestContext.HttpContext.Session.GetEaso();
        //	return new MvcHtmlString(easo.EasoName);
        //}

        public static MvcHtmlString ToAbsoluteUrl(this HtmlHelper helper, string relativeUrl, bool isUserName = false)
        {


            if (string.IsNullOrEmpty(relativeUrl))
                return MvcHtmlString.Create(relativeUrl);

            if (helper.ViewContext.HttpContext == null)
                return MvcHtmlString.Create(relativeUrl);

            Uri url = helper.ViewContext.HttpContext.Request.Url;
            string port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            if (isUserName)
                return MvcHtmlString.Create(String.Format("{0}://{1}{2}/Join/{3}",
                url.Scheme, url.Host, port, relativeUrl));


            return MvcHtmlString.Create(String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl)));
        }

        //public static MvcHtmlString ToUserNameUrl(this HtmlHelper helper, string relativeUrl)
        //{
        //	if (string.IsNullOrEmpty(relativeUrl))
        //		return MvcHtmlString.Create(relativeUrl);

        //	if (helper.ViewContext.HttpContext == null)
        //		return MvcHtmlString.Create(relativeUrl);

        //	Uri url = helper.ViewContext.HttpContext.Request.Url;
        //	string port = url.Port != 80 ? (":" + url.Port) : String.Empty;

        //	return MvcHtmlString.Create(String.Format("{0}://{1}{2}{3}",
        //		url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl)));
        //}
    }
}