// Decompiled with JetBrains decompiler
// Type: GridMvc.Sorting.QueryStringSortSettings
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;
using System.Web;

namespace GridMvc.Sorting
{
  public class QueryStringSortSettings : IGridSortSettings
  {
    public const string DefaultDirectionQueryParameter = "grid-dir";
    public const string DefaultColumnQueryParameter = "grid-column";
    public readonly HttpContext Context;
    private string _columnQueryParameterName;
    private string _directionQueryParameterName;

    public string ColumnQueryParameterName
    {
      get
      {
        return this._columnQueryParameterName;
      }
      set
      {
        this._columnQueryParameterName = value;
        this.RefreshColumn();
      }
    }

    public string DirectionQueryParameterName
    {
      get
      {
        return this._directionQueryParameterName;
      }
      set
      {
        this._directionQueryParameterName = value;
        this.RefreshDirection();
      }
    }

    public string ColumnName { get; set; }

    public GridSortDirection Direction { get; set; }

    public QueryStringSortSettings()
      : this(HttpContext.Current)
    {
    }

    public QueryStringSortSettings(HttpContext context)
    {
      if (context == null)
        throw new ArgumentException("No http context here!");
      this.Context = context;
      this.ColumnQueryParameterName = "grid-column";
      this.DirectionQueryParameterName = "grid-dir";
    }

    private void RefreshColumn()
    {
      string str = this.Context.Request.QueryString[this.ColumnQueryParameterName] ?? string.Empty;
      this.ColumnName = str;
      if (!string.IsNullOrEmpty(str))
        return;
      this.Direction = GridSortDirection.Ascending;
    }

    private void RefreshDirection()
    {
      string str = this.Context.Request.QueryString[this.DirectionQueryParameterName] ?? string.Empty;
      if (string.IsNullOrEmpty(str))
      {
        this.Direction = GridSortDirection.Ascending;
      }
      else
      {
        GridSortDirection result;
        Enum.TryParse<GridSortDirection>(str, true, out result);
        this.Direction = result;
      }
    }
  }
}
