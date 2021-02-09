// Decompiled with JetBrains decompiler
// Type: GridMvc.Html.GridExtensions
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;
using GridMvc.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace GridMvc.Html
{
  public static class GridExtensions
  {
    internal const string DefaultPartialViewName = "_Grid";

    public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, IQueryable<T> items) where T : class
    {
      return GridExtensions.Grid<T>(helper, items, "_Grid");
    }

    public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, IQueryable<T> items, string viewName) where T : class
    {
      return GridExtensions.Grid<T>(helper, items, GridRenderOptions.Create(string.Empty, viewName));
    }

    public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, IQueryable<T> items, GridRenderOptions renderOptions) where T : class
    {
      return new HtmlGrid<T>(new Grid<T>(items)
      {
        RenderOptions = renderOptions
      }, helper.ViewContext, renderOptions.ViewName);
    }

    public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, Grid<T> sourceGrid) where T : class
    {
      return new HtmlGrid<T>(sourceGrid, helper.ViewContext, "_Grid");
    }

    public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, Grid<T> sourceGrid, string viewName) where T : class
    {
      return new HtmlGrid<T>(sourceGrid, helper.ViewContext, viewName);
    }

    public static IGridColumn<T> RenderValueAs<T>(this IGridColumn<T> column, Func<T, IHtmlString> constraint)
    {
      Func<T, string> constraint1 = (Func<T, string>) (a => constraint(a).ToHtmlString());
      return column.RenderValueAs(constraint1);
    }

    public static IGridColumn<T> RenderValueAs<T>(this IGridColumn<T> column, Func<T, Func<object, HelperResult>> constraint)
    {
      Func<T, string> constraint1 = (Func<T, string>) (a => constraint(a)((object) null).ToHtmlString());
      return column.RenderValueAs(constraint1);
    }
  }
}
