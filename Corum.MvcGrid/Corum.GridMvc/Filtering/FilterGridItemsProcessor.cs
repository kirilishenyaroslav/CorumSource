// Decompiled with JetBrains decompiler
// Type: GridMvc.Filtering.FilterGridItemsProcessor`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;
using GridMvc.Columns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GridMvc.Filtering
{
  internal class FilterGridItemsProcessor<T> : IGridItemsProcessor<T> where T : class
  {
    private readonly IGrid _grid;
    private IGridFilterSettings _settings;

    public FilterGridItemsProcessor(IGrid grid, IGridFilterSettings settings)
    {
      if (settings == null)
        throw new ArgumentNullException("settings");
      this._grid = grid;
      this._settings = settings;
    }

    public void UpdateSettings(IGridFilterSettings settings)
    {
      if (settings == null)
        throw new ArgumentNullException("settings");
      this._settings = settings;
    }

    public IQueryable<T> Process(IQueryable<T> items)
    {
      foreach (IGridColumn column in (IEnumerable<IGridColumn>) this._grid.Columns)
      {
        IGridColumn<T> gridColumn = column as IGridColumn<T>;
        if (gridColumn != null && gridColumn.Filter != null)
        {
          IEnumerable<ColumnFilterValue> enumerable;
          if (!this._settings.IsInitState)
            enumerable = this._settings.FilteredColumns.GetByColumn(column);
          else
            enumerable = (IEnumerable<ColumnFilterValue>) new List<ColumnFilterValue>()
            {
              column.InitialFilterSettings
            };
          foreach (ColumnFilterValue columnFilterValue in enumerable)
          {
            if (!(columnFilterValue == ColumnFilterValue.Null))
              items = gridColumn.Filter.ApplyFilter(items, columnFilterValue);
          }
        }
      }
      return items;
    }
  }
}
