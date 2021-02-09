// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.IGridColumnCollection`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Columns
{
  public interface IGridColumnCollection<T> : IGridColumnCollection, IEnumerable<IGridColumn>, IEnumerable
  {
    IGridColumn<T> Add(IGridColumn<T> column);

    IGridColumn<T> Add();

    IGridColumn<T> Add(bool hidden);

    IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint);

    IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint, string columnName);

    IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint, bool hidden);

    IGridColumn<T> Add(PropertyInfo pi);

    IGridColumn<T> Insert(int position, IGridColumn<T> column);

    IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint);

    IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint, string columnName);

    IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint, bool hidden);
  }
}
