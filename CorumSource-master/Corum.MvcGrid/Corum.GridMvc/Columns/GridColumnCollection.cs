// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.GridColumnCollection`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Sorting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Columns
{
  public class GridColumnCollection<T> : KeyedCollection<string, IGridColumn>, IGridColumnCollection<T>, IGridColumnCollection, IEnumerable<IGridColumn>, IEnumerable
  {
    private readonly IColumnBuilder<T> _columnBuilder;
    private readonly IGridSortSettings _sortSettings;

    public GridColumnCollection(IColumnBuilder<T> columnBuilder, IGridSortSettings sortSettings)
    {
      this._columnBuilder = columnBuilder;
      this._sortSettings = sortSettings;
    }

    public IGridColumn<T> Add()
    {
      return this.Add(false);
    }

    public IGridColumn<T> Add(bool hidden)
    {
      return this.Add<string>((Expression<Func<T, string>>) null, hidden);
    }

    public IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint)
    {
      return this.Add<TKey>(constraint, false);
    }

    public IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint, string columnName)
    {
      return this.Add(this.CreateColumn<TKey>(constraint, false, columnName));
    }

    public IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint, bool hidden)
    {
      return this.Add(this.CreateColumn<TKey>(constraint, hidden, string.Empty));
    }

    public IGridColumn<T> Add(PropertyInfo pi)
    {
      IGridColumn<T> column = this._columnBuilder.CreateColumn(pi);
      if (column == null)
        return (IGridColumn<T>) null;
      return this.Add(column);
    }

    public IGridColumn<T> Add(IGridColumn<T> column)
    {
      if (column == null)
        throw new ArgumentNullException("column");
      try
      {
        this.Add((IGridColumn) column);
      }
      catch (ArgumentException ex)
      {
        throw new ArgumentException(string.Format("Column '{0}' already exist in the grid", (object) column.Name));
      }
      this.UpdateColumnsSorting();
      return column;
    }

    public IGridColumn<T> Insert(int position, IGridColumn<T> column)
    {
      this.Insert(position, (IGridColumn) column);
      this.UpdateColumnsSorting();
      return column;
    }

    public IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint)
    {
      return this.Insert<TKey>(position, constraint, false);
    }

    public IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint, string columnName)
    {
      IGridColumn<T> column = this.CreateColumn<TKey>(constraint, false, columnName);
      return this.Insert(position, column);
    }

    public IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint, bool hidden)
    {
      IGridColumn<T> column = this.CreateColumn<TKey>(constraint, hidden, string.Empty);
      return this.Insert(position, column);
    }

    public new IEnumerator<IGridColumn> GetEnumerator()
    {
      return base.GetEnumerator();
    }

    public IGridColumn GetByName(string name)
    {
      return Enumerable.FirstOrDefault<IGridColumn>((IEnumerable<IGridColumn>) this, (Func<IGridColumn, bool>) (c => c.Name.ToUpper() == name.ToUpper()));
    }

    protected override string GetKeyForItem(IGridColumn item)
    {
      return item.Name;
    }

    private IGridColumn<T> CreateColumn<TKey>(Expression<Func<T, TKey>> constraint, bool hidden, string columnName)
    {
      IGridColumn<T> column = this._columnBuilder.CreateColumn<TKey>(constraint, hidden);
      if (!string.IsNullOrEmpty(columnName))
        column.Name = columnName;
      return column;
    }

    internal void UpdateColumnsSorting()
    {
      if (string.IsNullOrEmpty(this._sortSettings.ColumnName))
        return;
      foreach (IGridColumn gridColumn in this)
      {
        gridColumn.IsSorted = gridColumn.Name == this._sortSettings.ColumnName;
        if (gridColumn.Name == this._sortSettings.ColumnName)
          gridColumn.Direction = new GridSortDirection?(this._sortSettings.Direction);
        else
          gridColumn.Direction = new GridSortDirection?();
      }
    }
  }
}
