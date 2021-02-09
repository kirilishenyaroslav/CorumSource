// Decompiled with JetBrains decompiler
// Type: GridMvc.Sorting.QueryStringSortColumnHeaderRenderer
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;
using GridMvc.Columns;
using GridMvc.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace GridMvc.Sorting
{
  internal class QueryStringSortColumnHeaderRenderer : IGridColumnHeaderRenderer
  {
    private readonly QueryStringSortSettings _settings;

    public QueryStringSortColumnHeaderRenderer(QueryStringSortSettings settings)
    {
      this._settings = settings;
    }

    public IHtmlString Render(IGridColumn column)
    {
      return (IHtmlString) MvcHtmlString.Create(this.GetSortHeaderContent(column));
    }

    protected string GetSortHeaderContent(IGridColumn column)
    {
      TagBuilder tagBuilder1 = new TagBuilder("div");
      tagBuilder1.AddCssClass("grid-header-title");
      if (column.SortEnabled)
      {
        TagBuilder tagBuilder2 = new TagBuilder("a")
        {
          InnerHtml = column.Title
        };
        string sortUrl = this.GetSortUrl(column.Name, column.Direction);
        tagBuilder2.Attributes.Add("href", sortUrl);
        tagBuilder1.InnerHtml += tagBuilder2.ToString();
      }
      else
      {
        TagBuilder tagBuilder2 = new TagBuilder("span")
        {
          InnerHtml = column.Title
        };
        tagBuilder1.InnerHtml += tagBuilder2.ToString();
      }
      if (column.IsSorted)
      {
        tagBuilder1.AddCssClass("sorted");
        TagBuilder tagBuilder2 = tagBuilder1;
        GridSortDirection? direction = column.Direction;
        string str = (direction.GetValueOrDefault() != GridSortDirection.Ascending ? 0 : (direction.HasValue ? 1 : 0)) != 0 ? "sorted-asc" : "sorted-desc";
        tagBuilder2.AddCssClass(str);
        TagBuilder tagBuilder3 = new TagBuilder("span");
        tagBuilder3.AddCssClass("grid-sort-arrow");
        tagBuilder1.InnerHtml += tagBuilder3.ToString();
      }
      return tagBuilder1.ToString();
    }

    private string GetSortUrl(string columnName, GridSortDirection? direction)
    {
      GridSortDirection? nullable = direction;
      GridSortDirection gridSortDirection = (nullable.GetValueOrDefault() != GridSortDirection.Ascending ? 0 : (nullable.HasValue ? 1 : 0)) != 0 ? GridSortDirection.Descending : GridSortDirection.Ascending;
      string queryStringExcept = new CustomQueryStringBuilder(this._settings.Context.Request.QueryString).GetQueryStringExcept((IList<string>) new string[3]
      {
        "grid-page",
        this._settings.ColumnQueryParameterName,
        this._settings.DirectionQueryParameterName
      });
      return string.Format("{0}{1}={2}&{3}={4}", (object) (!string.IsNullOrEmpty(queryStringExcept) ? queryStringExcept + "&" : "?"), (object) this._settings.ColumnQueryParameterName, (object) columnName, (object) this._settings.DirectionQueryParameterName, (object) ((int) gridSortDirection).ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }
  }
}
