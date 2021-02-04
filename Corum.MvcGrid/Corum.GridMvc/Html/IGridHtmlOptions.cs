// Decompiled with JetBrains decompiler
// Type: GridMvc.Html.IGridHtmlOptions`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Columns;
using System;
using System.Web;

namespace GridMvc.Html
{
  public interface IGridHtmlOptions<T> : IHtmlString
  {
    IGridHtmlOptions<T> Columns(Action<IGridColumnCollection<T>> columnBuilder);

    IGridHtmlOptions<T> WithPaging(int pageSize);

    IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems);

    IGridHtmlOptions<T> WithPaging(int pageSize, int maxDisplayedItems, string queryStringParameterName);

    IGridHtmlOptions<T> Sortable();

    IGridHtmlOptions<T> Sortable(bool enable);

    IGridHtmlOptions<T> Filterable();

    IGridHtmlOptions<T> Filterable(bool enable);

    IGridHtmlOptions<T> Selectable(bool set);

    IGridHtmlOptions<T> EmptyText(string text);

    IGridHtmlOptions<T> SetLanguage(string lang);

    IGridHtmlOptions<T> SetRowCssClasses(Func<T, string> contraint);

    IGridHtmlOptions<T> Named(string gridName);

    IGridHtmlOptions<T> AutoGenerateColumns();

    IGridHtmlOptions<T> WithMultipleFilters();

    string Render();
  }
}
