// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.IColumnBuilder`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Columns
{
  public interface IColumnBuilder<T>
  {
    bool DefaultSortEnabled { get; set; }

    bool DefaultFilteringEnabled { get; set; }

    IGridColumn<T> CreateColumn<TDataType>(Expression<Func<T, TDataType>> expression, bool hidden);

    IGridColumn<T> CreateColumn(PropertyInfo pi);
  }
}
