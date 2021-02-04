// Decompiled with JetBrains decompiler
// Type: GridMvc.DataAnnotations.GridColumnAttribute
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Sorting;
using System;

namespace GridMvc.DataAnnotations
{
  [AttributeUsage(AttributeTargets.Property)]
  public class GridColumnAttribute : GridHiddenColumnAttribute
  {
    private GridSortDirection? _initialDirection;

    public string Title { get; set; }

    public bool SortEnabled { get; set; }

    public bool FilterEnabled { get; set; }

    public string Width { get; set; }

    public string FilterWidgetType { get; set; }

    public GridSortDirection SortInitialDirection
    {
      get
      {
        if (!this._initialDirection.HasValue)
          return GridSortDirection.Ascending;
        return this._initialDirection.Value;
      }
      set
      {
        this._initialDirection = new GridSortDirection?(value);
      }
    }

    public GridColumnAttribute()
    {
      this.EncodeEnabled = true;
      this.SanitizeEnabled = true;
      this.SortEnabled = false;
    }

    public GridSortDirection? GetInitialSortDirection()
    {
      return this._initialDirection;
    }
  }
}
