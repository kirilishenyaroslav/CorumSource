using System.Web.Optimization;

namespace BarnivannAdminUI
{
    public class BundleConfig
    {        
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.stickytableheaders.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUI").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery_ajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
                        "~/Scripts/jquery.datetimepicker.js",
                        "~/Scripts/moment.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/corum").Include(
                        "~/Scripts/datetimewrapper.js",
                        "~/Scripts/search.widget.js",
                        "~/Scripts/activeonly.widget.js",
                        "~/Scripts/unhandledonly.widget.js",
                        "~/Scripts/corum.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/grathVis").Include(
                        "~/Scripts/vis.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootbox.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/blockUI").Include(
                      "~/Scripts/jquery.blockUI.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                        "~/Scripts/select2.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                        "~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/MvcGrid").Include(
                        "~/Scripts/gridmvc.js",
                        "~/Scripts/gridmvc.lang.no.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/bootstrap-datepicker.nb.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/dashboard.css",
                      "~/Content/jquery.datetimepicker.css",
                      "~/Content/select2.css",
                      "~/Content/Gridmvc.css",
                      "~/Content/gridmvc.datepicker.css",
                      "~/Content/toastr.css",
                      "~/Content/site.css",
                      "~/Content/vis.css"));

            bundles.Add(new StyleBundle("~/Content/ui_css").Include(
                      "~/Content/themes/base/core.css",
                      "~/Content/themes/base/accordion.css",
                      "~/Content/themes/base/autocomplete.css",
                      "~/Content/themes/base/button.css",
                      "~/Content/themes/base/datepicker.css",
                      "~/Content/themes/base/dialog.css",
                      "~/Content/themes/base/draggable.css",
                      "~/Content/themes/base/menu.css",
                      "~/Content/themes/base/progressbar.css",
                      "~/Content/themes/base/resizable.css",
                      "~/Content/themes/base/selectable.css",
                      "~/Content/themes/base/selectmenu.css",
                      "~/Content/themes/base/sortable.css",
                      "~/Content/themes/base/slider.css",
                      "~/Content/themes/base/spinner.css",
                      "~/Content/themes/base/tabs.css",
                      "~/Content/themes/base/tooltip.css"));

            bundles.Add(
               new ScriptBundle("~/bundles/jqwidgets")
                .Include("~/scripts/jqwidgets/jqxcore.js")
                .Include("~/scripts/jqwidgets/jqxdata.js")
                .Include("~/scripts/jqwidgets/jqxbuttons.js")
                .Include("~/scripts/jqwidgets/jqxscrollbar.js")
                .Include("~/scripts/jqwidgets/jqxmenu.js")
                .Include("~/scripts/jqwidgets/jqxlistbox.js")
                .Include("~/scripts/jqwidgets/jqxgrid.js")
                .Include("~/scripts/jqwidgets/jqxgrid.selection.js")
                .Include("~/scripts/jqwidgets/jqxgrid.columnsresize.js")
                .Include("~/scripts/jqwidgets/jqxgrid.filter.js")
                .Include("~/scripts/jqwidgets/jqxgrid.sort.js")
                .Include("~/scripts/jqwidgets/jqxgrid.pager.js")
                .Include("~/scripts/jqwidgets/jqxgrid.grouping.js")
                .Include("~/scripts/jqwidgets/jqxcheckbox.js")
                .Include("~/scripts/jqwidgets/jqxdatatable.js")
                .Include("~/scripts/jqwidgets/jqxlistbox.js")
                .Include("~/scripts/jqwidgets/jqxdropdownlist.js")
                .Include("~/scripts/jqwidgets/jqxtreegrid.js")
                .Include("~/scripts/jqwidgets/jqxcalendar.js")
                .Include("~/scripts/jqwidgets/jqxdatetimeinput.js")
                .Include("~/scripts/jqwidgets/jqxslider.js")
                .Include("~/scripts/jqwidgets/jqxeditor.js")
                .Include("~/scripts/jqwidgets/jqxinput.js")
                .Include("~/scripts/jqwidgets/jqxdraw.js")
                .Include("~/scripts/jqwidgets/jqxradiobutton.js")
                .Include("~/scripts/jqwidgets/jqxvalidator.js")
                .Include("~/scripts/jqwidgets/jqxpanel.js")
                .Include("~/scripts/jqwidgets/jqxpasswordinput.js")
                .Include("~/scripts/jqwidgets/jqxnumberinput.js")
                .Include("~/scripts/jqwidgets/jqxcombobox.js")
                .Include("~/scripts/jqwidgets/jqxdata.export.js")
                .Include("~/scripts/jqwidgets/globalize.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/tinymce")
                .Include("~/scripts/tinymce/jquery.tinymce.min.js")
                .Include("~/scripts/tinymce/tinymce.js")
                .Include("~/scripts/tinymce/tinymce.min.js"));

            
            bundles.Add(
                new StyleBundle("~/Content/JQWidgetsCss")
                .Include("~/Content/jqwidgets/jqx.base.css")
                .Include("~/Content/jqx.nms.css")
                .Include("~/Content/jqx.orange.css")
                );


            BundleTable.EnableOptimizations = true;
#if DEBUG
            BundleTable.EnableOptimizations = false;
            
#endif




        }
    }
}
