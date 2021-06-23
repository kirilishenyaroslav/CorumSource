using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Corum.ReportsUI
{
    public static class VisitorInfo
    {
        public static string GetVisitorIP()
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }

            return string.Empty;
        }
    }




    public static class ExtensionMethods
    {
        public static TConvert ConvertTo<TConvert>(this object entity) where TConvert : new()
        {
            var convertProperties = TypeDescriptor.GetProperties(typeof(TConvert)).Cast<PropertyDescriptor>();
            var entityProperties = TypeDescriptor.GetProperties(entity).Cast<PropertyDescriptor>();

            var convert = new TConvert();

            foreach (var entityProperty in entityProperties)
            {
                var property = entityProperty;
                var convertProperty = convertProperties.FirstOrDefault(prop => prop.Name == property.Name);
                if (convertProperty != null)
                {
                    convertProperty.SetValue(convert, Convert.ChangeType(entityProperty.GetValue(entity), convertProperty.PropertyType));
                }
            }

            return convert;
        }
    }
    public class BootstrapHtml
    {
        public static MvcHtmlString Dropdown(string id, List<SelectListItem> selectListItems, string label)
        {
            var button = new TagBuilder("a")
            {
                Attributes =
            {
                {"id", id},
                {"data-toggle", "dropdown"}
            }
            };

            button.AddCssClass("dropdown-toggle");

            button.SetInnerText(label);
            button.InnerHtml += " " + BuildCaret();

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("dropdown");

            wrapper.InnerHtml += button;
            wrapper.InnerHtml += BuildDropdown(id, selectListItems);

            return new MvcHtmlString(wrapper.ToString());
        }

        private static string BuildCaret()
        {
            var caret = new TagBuilder("span");
            caret.AddCssClass("caret");

            return caret.ToString();
        }

        private static string BuildDropdown(string id, IEnumerable<SelectListItem> items)
        {
            var list = new TagBuilder("ul")
            {
                Attributes =
            {
                {"class", "dropdown-menu"},
                {"role", "menu"},
                {"aria-labelledby", id}
            }
            };

            var listItem = new TagBuilder("li");
            listItem.Attributes.Add("role", "presentation");

            foreach (var x in items)
            {
                list.InnerHtml += "<li role=\"presentation\">" + BuildListRow(x) + "</li>";
            }

            return list.ToString();
        }

        private static string BuildListRow(SelectListItem item)
        {
            var anchor = new TagBuilder("a")
            {
                Attributes =
            {
                {"role", "menuitem"},
                {"tabindex", "-1"},
                {"href", item.Value}
            }
            };

            anchor.SetInnerText(item.Text);

            return anchor.ToString();
        }
    }
}