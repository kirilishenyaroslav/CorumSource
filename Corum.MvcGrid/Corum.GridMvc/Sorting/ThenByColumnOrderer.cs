// Decompiled with JetBrains decompiler
// Type: GridMvc.Sorting.ThenByColumnOrderer`2
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;
using System.Linq;
using System.Linq.Expressions;

namespace GridMvc.Sorting
{
  internal class ThenByColumnOrderer<T, TKey> : IColumnOrderer<T>
  {
    private readonly Expression<Func<T, TKey>> _expression;
    private readonly GridSortDirection _initialDirection;

    public ThenByColumnOrderer(Expression<Func<T, TKey>> expression, GridSortDirection initialDirection)
    {
      this._expression = expression;
      this._initialDirection = initialDirection;
    }

    public IQueryable<T> ApplyOrder(IQueryable<T> items)
    {
      IOrderedQueryable<T> source = items as IOrderedQueryable<T>;
      if (source == null)
        return items;
      switch (this._initialDirection)
      {
        case GridSortDirection.Ascending:
          return (IQueryable<T>) Queryable.ThenBy<T, TKey>(source, this._expression);
        case GridSortDirection.Descending:
          return (IQueryable<T>) Queryable.ThenByDescending<T, TKey>(source, this._expression);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IQueryable<T> ApplyOrder(IQueryable<T> items, GridSortDirection direction)
    {
      return this.ApplyOrder(items);
    }
  }
}
