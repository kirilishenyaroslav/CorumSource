// Decompiled with JetBrains decompiler
// Type: GridMvc.Sorting.SortGridItemsProcessor`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;
using GridMvc.Columns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GridMvc.Sorting
{
  internal class SortGridItemsProcessor<T> : IGridItemsProcessor<T> where T : class
  {
    private readonly IGrid _grid;
    private IGridSortSettings _settings;

    public SortGridItemsProcessor(IGrid grid, IGridSortSettings settings)
    {
      if (settings == null)
        throw new ArgumentNullException("settings");
      this._grid = grid;
      this._settings = settings;
    }

    public void UpdateSettings(IGridSortSettings settings)
    {
      if (settings == null)
        throw new ArgumentNullException("settings");
      this._settings = settings;
    }

    public IQueryable<T> Process(IQueryable<T> items)
    {
      if (string.IsNullOrEmpty(this._settings.ColumnName))
        return items;
      IGridColumn<T> gridColumn = Enumerable.FirstOrDefault<IGridColumn>((IEnumerable<IGridColumn>) this._grid.Columns, (Func<IGridColumn, bool>) (c => c.Name == this._settings.ColumnName)) as IGridColumn<T>;
      if (gridColumn == null || !gridColumn.SortEnabled)
        return items;
      foreach (IColumnOrderer<T> columnOrderer in gridColumn.Orderers)
        items = columnOrderer.ApplyOrder(items, this._settings.Direction);
      return items;
    }
  }
}
