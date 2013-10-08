﻿using System.Web;
using System.Web.Optimization;

namespace StudentTracker
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap*"));
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.css", "~/Content/bootstrap-responsive.css"));
            bundles.Add(new ScriptBundle("~/bundles/BlockUI").Include("~/Scripts/jquery.blockUI.js"));
            bundles.Add(new ScriptBundle("~/bundles/DataTable").Include("~/Scripts/DataTables-1.9.4/media/js/jquery.dataTables.js"));
            bundles.Add(new ScriptBundle("~/bundles/Areas").Include(
                "~/Areas/SAS/assets/js/jquery-1.8.2.min.js",
                "~/Areas/SAS/assets/js/modernizr.custom.js",
                "~/Areas/SAS/assets/plugins/bootstrap/js/bootstrap.min.js",
                "~/Areas/SAS/assets/plugins/flexslider/jquery.flexslider-min.js",
                "~/Areas/SAS/assets/plugins/parallax-slider/js/modernizr.js",
                "~/Areas/SAS/assets/plugins/parallax-slider/js/jquery.cslider.js",
                "~/Areas/SAS/assets/plugins/back-to-top.js",
                "~/Areas/SAS/assets/js/app.js"));
        }
    }
}