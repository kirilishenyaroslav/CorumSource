// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.ISortableColumn`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Sorting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GridMvc.Columns
{
  public interface ISortableColumn<T> : IColumn
  {
    IEnumerable<IColumnOrderer<T>> Orderers { get; }

    IGridColumn<T> Sortable(bool sort);

    IGridColumn<T> SortInitialDirection(GridSortDirection direction);

    IGridColumn<T> ThenSortBy<TKey>(Expression<Func<T, TKey>> expression);

    IGridColumn<T> ThenSortByDescending<TKey>(Expression<Func<T, TKey>> expression);
  }
}
