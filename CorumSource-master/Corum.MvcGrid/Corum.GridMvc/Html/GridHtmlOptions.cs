// Decompiled with JetBrains decompiler
// Type: GridMvc.Html.GridHtmlOptions`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;
using GridMvc.Columns;
using GridMvc.Pagination;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace GridMvc.Html
{
  public class GridHtmlOptions<T> : IGridHtmlOptions<T>, IHtmlString where T : class
  {
    private readonly Grid<T> _source;
    private readonly ViewContext _viewContext;

    public string GridViewName { get; set; }

    public GridHtmlOptions(Grid<T> source, ViewContext viewContext, string viewName)
    {
      this._source = source;
      this._viewContext = viewContext;
      this.GridViewName = viewName;
    }

    public string ToHtmlString()
    {
      return GridHtmlOptions<T>.RenderPartialViewToString(this.GridViewName, (object) this, this._viewContext);
    }

    public string Render()
    {
      return this.ToHtmlString();
    }

    public IGridHtmlOptions<T> Columns(Action<IGridColumnCollection<T>> columnBuilder)
    {
      columnBuilder(this._source.Columns);
      return (IGridHtmlOptions<T>) this;
    }

    public IGridHtmlOptions<T> WithPaging(int pageSize)
    {
      return this.WithPaging(pageSize, 0);
    }

    public IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems)
    {
      return this.WithPaging(pageSize, maxDisplayedItems, string.Empty);
    }

    public IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems, string queryStringParameterName)
    {
      this._source.EnablePaging = true;
      this._source.Pager.PageSize = pageSize;
      GridPager gridPager = this._source.Pager as GridPager;
      if (gridPager == null)
        return (IGridHtmlOptions<T>) this;
      if (maxDisplayedItems > 0)
        gridPager.MaxDisplayedPages = maxDisplayedItems;
      if (!string.IsNullOrEmpty(queryStringParameterName))
        gridPager.ParameterName = queryStringParameterName;
      this._source.Pager = (IGridPager) gridPager;
      return (IGridHtmlOptions<T>) this;
    }

    public IGridHtmlOptions<T> Sortable()
    {
      return this.Sortable(true);
    }

    public IGridHtmlOptions<T> Sortable(bool enable)
    {
      this._source.DefaultSortEnabled = enable;
      foreach (IGridColumn gridColumn1 in (IEnumerable<IGridColumn>) this._source.Columns)
      {
        IGridColumn<T> gridColumn2 = gridColumn1 as IGridColumn<T>;
        if (gridColumn2 != null)
          gridColumn2.Sortable(enable);
      }
      return (IGridHtmlOptions<T>) this;
    }

    public IGridHtmlOptions<T> Filterable()
    {
      return this.Filterable(true);
    }

    public IGridHtmlOptions<T> Filterable(bool enable)
    {
      this._source.DefaultFilteringEnabled = enable;
      foreach (IGridColumn gridColumn1 in (IEnumerable<IGridColumn>) this._source.Columns)
      {
        IGridColumn<T> gridColumn2 = gridColumn1 as IGridColumn<T>;
        if (gridColumn2 != null)
          gridColumn2.Filterable(enable);
      }
      return (IGridHtmlOptions<T>) this;
    }

    public IGridHtmlOptions<T> Selectable(bool set)
    {
      this._source.RenderOptions.Selectable = set;
      return (IGridHtmlOptions<T>) this;
    }

    public IGridHtmlOptions<T> EmptyText(string text)
    {
      this._source.EmptyGridText = text;
      return (IGridHtmlOptions<T>) this;
    }

    public IGridHtmlOptions<T> SetLanguage(string lang)
    {
      this._source.Language = lang;
      return (IGridHtmlOptions<T>) this;
    }

    public IGridHtmlOptions<T> SetRowCssClasses(Func<T, string> contraint)
    {
      this._source.SetRowCssClassesContraint(contraint);
      return (IGridHtmlOptions<T>) this;
    }

    public IGridHtmlOptions<T> Named(string gridName)
    {
      this._source.RenderOptions.GridName = gridName;
      return (IGridHtmlOptions<T>) this;
    }

    public IGridHtmlOptions<T> AutoGenerateColumns()
    {
      this._source.AutoGenerateColumns();
      return (IGridHtmlOptions<T>) this;
    }

    public IGridHtmlOptions<T> WithMultipleFilters()
    {
      this._source.RenderOptions.AllowMultipleFilters = true;
      return (IGridHtmlOptions<T>) this;
    }

    private static string RenderPartialViewToString(string viewName, object model, ViewContext viewContext)
    {
      if (string.IsNullOrEmpty(viewName))
        throw new ArgumentException("viewName");
      ControllerContext controllerContext = new ControllerContext(viewContext.RequestContext, viewContext.Controller);
      using (StringWriter stringWriter = new StringWriter())
      {
        ViewEngineResult partialView = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
        if (partialView.View == null)
          throw new InvalidDataException(string.Format("Specified view name for Grid.Mvc not found. ViewName: {0}", (object) viewName));
        ViewContext viewContext1 = new ViewContext(controllerContext, partialView.View, viewContext.ViewData, viewContext.TempData, (TextWriter) stringWriter)
        {
          ViewData = {
            Model = model
          }
        };
        partialView.View.Render(viewContext1, (TextWriter) stringWriter);
        return stringWriter.GetStringBuilder().ToString();
      }
    }
  }
}
