// Decompiled with JetBrains decompiler
// Type: GridMvc.Filtering.QueryStringFilterColumnHeaderRenderer
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;
using GridMvc.Columns;
using GridMvc.Pagination;
using GridMvc.Resources;
using GridMvc.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GridMvc.Filtering
{
  internal class QueryStringFilterColumnHeaderRenderer : IGridColumnHeaderRenderer
  {
    private const string FilteredButtonCssClass = "filtered";
    private const string FilterButtonCss = "grid-filter-btn";
    private readonly QueryStringFilterSettings _settings;

    public QueryStringFilterColumnHeaderRenderer(QueryStringFilterSettings settings)
    {
      this._settings = settings;
    }

    public IHtmlString Render(IGridColumn column)
    {
      if (!column.FilterEnabled)
        return (IHtmlString) MvcHtmlString.Create(string.Empty);
      List<ColumnFilterValue> t = new List<ColumnFilterValue>();
      if (this._settings.IsInitState && column.InitialFilterSettings != ColumnFilterValue.Null)
        t.Add(column.InitialFilterSettings);
      else
        t.AddRange(this._settings.FilteredColumns.GetByColumn(column));
      bool flag = Enumerable.Any<ColumnFilterValue>((IEnumerable<ColumnFilterValue>) t);
      CustomQueryStringBuilder queryStringBuilder = new CustomQueryStringBuilder(this._settings.Context.Request.QueryString);
      List<string> list = new List<string>()
      {
        "grid-filter",
        "grid-init"
      };
      string queryParameterName = this.GetPagerQueryParameterName(column.ParentGrid.Pager);
      if (!string.IsNullOrEmpty(queryParameterName))
        list.Add(queryParameterName);
      string queryStringExcept = queryStringBuilder.GetQueryStringExcept((IList<string>) list);
      TagBuilder tagBuilder1 = new TagBuilder("span");
      tagBuilder1.AddCssClass("grid-filter-btn");
      if (flag)
        tagBuilder1.AddCssClass("filtered");
      tagBuilder1.Attributes.Add("title", Strings.FilterButtonTooltipText);
      TagBuilder tagBuilder2 = new TagBuilder("div");
      Dictionary<string, string> dictionary = new Dictionary<string, string>()
      {
        {
          "data-type",
          column.FilterWidgetTypeName
        },
        {
          "data-name",
          column.Name
        },
        {
          "data-widgetdata",
          JsonHelper.JsonSerializer<object>(column.FilterWidgetData)
        },
        {
          "data-filterdata",
          JsonHelper.JsonSerializer<List<ColumnFilterValue>>(t)
        },
        {
          "data-url",
          queryStringExcept
        }
      };
      tagBuilder2.InnerHtml = tagBuilder1.ToString();
      tagBuilder2.AddCssClass("grid-filter");
      foreach (KeyValuePair<string, string> keyValuePair in dictionary)
      {
        if (!string.IsNullOrWhiteSpace(keyValuePair.Value))
          tagBuilder2.Attributes.Add(keyValuePair.Key, keyValuePair.Value);
      }
      return (IHtmlString) MvcHtmlString.Create(tagBuilder2.ToString());
    }

    private string GetPagerQueryParameterName(IGridPager pager)
    {
      GridPager gridPager = pager as GridPager;
      if (gridPager == null)
        return string.Empty;
      return gridPager.ParameterName;
    }
  }
}
