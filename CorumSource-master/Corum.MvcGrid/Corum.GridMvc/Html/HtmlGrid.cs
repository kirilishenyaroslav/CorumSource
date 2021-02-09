// Decompiled with JetBrains decompiler
// Type: GridMvc.Html.HtmlGrid`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;
using GridMvc.Columns;
using GridMvc.Pagination;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GridMvc.Html
{
  public class HtmlGrid<T> : GridHtmlOptions<T>, IGrid where T : class
  {
    private readonly Grid<T> _source;

    public GridRenderOptions RenderOptions
    {
      get
      {
        return this._source.RenderOptions;
      }
    }

    IGridColumnCollection IGrid.Columns
    {
      get
      {
        return (IGridColumnCollection) this._source.Columns;
      }
    }

    IEnumerable<object> IGrid.ItemsToDisplay
    {
      get
      {
        return ((IGrid) this._source).ItemsToDisplay;
      }
    }

    int IGrid.DisplayingItemsCount
    {
      get
      {
        return this._source.DisplayingItemsCount;
      }
    }

    IGridPager IGrid.Pager
    {
      get
      {
        return this._source.Pager;
      }
    }

    bool IGrid.EnablePaging
    {
      get
      {
        return this._source.EnablePaging;
      }
    }

    string IGrid.EmptyGridText
    {
      get
      {
        return this._source.EmptyGridText;
      }
    }

    string IGrid.Language
    {
      get
      {
        return this._source.Language;
      }
    }

    public ISanitizer Sanitizer
    {
      get
      {
        return this._source.Sanitizer;
      }
    }

    IGridSettingsProvider IGrid.Settings
    {
      get
      {
        return this._source.Settings;
      }
    }

    public HtmlGrid(Grid<T> source, ViewContext viewContext, string viewName)
      : base(source, viewContext, viewName)
    {
      this._source = source;
    }

    string IGrid.GetRowCssClasses(object item)
    {
      return this._source.GetRowCssClasses(item);
    }
  }
}
