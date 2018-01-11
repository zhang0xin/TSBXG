﻿using System.Web;
using System.Web.Optimization;

namespace TSBXG
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region not used

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // 使用 Modernizr 的开发版本进行开发和了解信息。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

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
            #endregion

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            // bootstrap
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                        "~/Scripts/jquery.js"));

            bundles.Add(new ScriptBundle("~/Scripts/pdfobject").Include(
                        "~/Scripts/pdfobject.js"));

            bundles.Add(new StyleBundle("~/Content/summernote")
                .Include("~/Content/summernote-0.8.2-dist/summernote.css"));
            bundles.Add(new ScriptBundle("~/Scripts/summernote").Include(
                "~/Content/summernote-0.8.2-dist/summernote.js",
                "~/Content/summernote-0.8.2-dist/lang/summernote-zh-CN.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap")
                .Include("~/Content/bootstrap-3.3.7-dist/css/bootstrap.css"));
            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include(
                "~/Content/bootstrap-3.3.7-dist/js/bootstrap.js",
                "~/Content/bootstrap-treeview.js"
                ));
        }
    }
}