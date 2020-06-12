// Decompiled with JetBrains decompiler
// Type: GridMvc.Filtering.QueryStringFilterSettings
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GridMvc.Filtering
{
  public class QueryStringFilterSettings : IGridFilterSettings
  {
    private readonly DefaultFilterColumnCollection _filterValues = new DefaultFilterColumnCollection();
    public const string DefaultTypeQueryParameter = "grid-filter";
    private const string FilterDataDelimeter = "__";
    public const string DefaultFilterInitQueryParameter = "grid-init";
    public readonly HttpContext Context;

    public IFilterColumnCollection FilteredColumns
    {
      get
      {
        return (IFilterColumnCollection) this._filterValues;
      }
    }

    public bool IsInitState
    {
      get
      {
        if (Enumerable.Any<ColumnFilterValue>((IEnumerable<ColumnFilterValue>) this.FilteredColumns))
          return false;
        return this.Context.Request.QueryString["grid-init"] != null;
      }
    }

    public QueryStringFilterSettings()
      : this(HttpContext.Current)
    {
    }

    public QueryStringFilterSettings(HttpContext context)
    {
      if (context == null)
        throw new ArgumentException("No http context here!");
      this.Context = context;
      string[] values = this.Context.Request.QueryString.GetValues("grid-filter");
      if (values == null)
        return;
      foreach (string queryParameterValue in values)
      {
        ColumnFilterValue columnData = this.CreateColumnData(queryParameterValue);
        if (columnData != ColumnFilterValue.Null)
          this._filterValues.Add(columnData);
      }
    }

    private ColumnFilterValue CreateColumnData(string queryParameterValue)
    {
      if (string.IsNullOrEmpty(queryParameterValue))
        return ColumnFilterValue.Null;
      string[] strArray = queryParameterValue.Split(new string[1]
      {
        "__"
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length != 3)
        return ColumnFilterValue.Null;
      GridFilterType result;
      if (!Enum.TryParse<GridFilterType>(strArray[1], true, out result))
        result = GridFilterType.Equals;
      return new ColumnFilterValue()
      {
        ColumnName = strArray[0],
        FilterType = result,
        FilterValue = strArray[2]
      };
    }
  }
}
