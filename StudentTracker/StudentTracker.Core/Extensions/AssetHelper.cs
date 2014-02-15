﻿using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Core.Extensions
{
	// AssetsHelper code from: http://stackoverflow.com/questions/5110028/add-css-or-js-files-to-layout-head-from-views-or-partial-views
	// Used to dynamically add CSS or Javascript to a _Layout based on the needs of a partial or view. Sweet!

	//TODO: The Styles should be in the head but the order of execution is wrong (the render is called before the Styles are added
	public class AssetsHelper
	{
		public static AssetsHelper GetInstance(HtmlHelper htmlHelper)
		{
			var instanceKey = "AssetsHelperInstance";

			var context = htmlHelper.ViewContext.HttpContext;
			if (context == null) return null;

			var assetsHelper = (AssetsHelper)context.Items[instanceKey];

			if (assetsHelper == null)
				context.Items.Add(instanceKey, assetsHelper = new AssetsHelper());

			return assetsHelper;
		}

		public ItemRegistrar Styles { get; private set; }

		public ItemRegistrar Scripts { get; private set; }

		public AssetsHelper()
		{
			Styles = new ItemRegistrar(ItemRegistrarFromatters.StyleFormat);
			Scripts = new ItemRegistrar(ItemRegistrarFromatters.ScriptFormat);
		}
	}

	public class ItemRegistrar
	{
		private readonly string _format;
		private readonly IList<string> _items;

		public ItemRegistrar(string format)
		{
			_format = format;
			_items = new List<string>();
		}

		public ItemRegistrar Add(string url)
		{
			if (!_items.Contains(url))
				_items.Add(url);

			return this;
		}

		public IHtmlString Render()
		{
			var sb = new StringBuilder();

			foreach (var item in _items)
			{
				var fmt = string.Format(_format, item);
				sb.AppendLine(fmt);
			}

			return new HtmlString(sb.ToString());
		}
	}

	public class ItemRegistrarFromatters
	{
		public const string StyleFormat = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
		public const string ScriptFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";
	}
}