// Decompiled with JetBrains decompiler
// Type: GridMvc.Sorting.OrderByGridOrderer`2
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;
using System.Linq;
using System.Linq.Expressions;

namespace GridMvc.Sorting
{
  internal class OrderByGridOrderer<T, TKey> : IColumnOrderer<T>
  {
    private readonly Expression<Func<T, TKey>> _expression;

    public OrderByGridOrderer(Expression<Func<T, TKey>> expression)
    {
      this._expression = expression;
    }

    public IQueryable<T> ApplyOrder(IQueryable<T> items)
    {
      return this.ApplyOrder(items, GridSortDirection.Ascending);
    }

    public IQueryable<T> ApplyOrder(IQueryable<T> items, GridSortDirection direction)
    {
      switch (direction)
      {
        case GridSortDirection.Ascending:
          return (IQueryable<T>) Queryable.OrderBy<T, TKey>(items, this._expression);
        case GridSortDirection.Descending:
          return (IQueryable<T>) Queryable.OrderByDescending<T, TKey>(items, this._expression);
        default:
          throw new ArgumentOutOfRangeException("direction");
      }
    }
  }
}
