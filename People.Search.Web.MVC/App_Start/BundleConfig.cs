using System.Web.Optimization;

namespace HealthCatalyst
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery.validate.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/app-api")
                .Include("~/Scripts/app.api.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap.css",
                    "~/Content/bootstrap-theme.css",
                    "~/Content/bootstrap-datepicker3.css",
                    "~/Content/font-awesome.css",
                    "~/Content/style.css"));

            bundles.Add(new StyleBundle("~/Content/datatable")
                .Include("~/Content/dataTables.bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatable")
                .Include("~/Scripts/jquery.dataTables.js")
                .Include("~/Scripts/dataTables.bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/alertify")
                .Include("~/Content/alertifyjs/alertify.css")
                .Include("~/Content/alertifyjs/themes/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/alertify")
                .Include("~/Scripts/alertify.js"));

            bundles.Add(new ScriptBundle("~/bundles/pages/home/index")
                .Include("~/Scripts/pages/home.index.js"));
        }
    }
}