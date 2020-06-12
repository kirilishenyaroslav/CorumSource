// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.GridColumnBase`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;
using GridMvc.Filtering;
using GridMvc.Sorting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace GridMvc.Columns
{
  public abstract class GridColumnBase<T> : IGridColumn<T>, IGridColumn, ISortableColumn, IFilterableColumn, IColumn<T>, ISortableColumn<T>, IColumn, IFilterableColumn<T>
  {
    protected Func<T, string> ValueConstraint;
    protected string ValuePattern;

    public bool EncodeEnabled { get; protected set; }

    public bool SanitizeEnabled { get; set; }

    public string Width { get; set; }

    public bool SortEnabled { get; protected set; }

    public string Title { get; set; }

    public string Name { get; set; }

    public bool IsSorted { get; set; }

    public GridSortDirection? Direction { get; set; }

    public abstract IGrid ParentGrid { get; }

    public abstract IEnumerable<IColumnOrderer<T>> Orderers { get; }

    public abstract IGridColumnHeaderRenderer HeaderRenderer { get; set; }

    public abstract IGridCellRenderer CellRenderer { get; set; }

    public abstract bool FilterEnabled { get; set; }

    public ColumnFilterValue InitialFilterSettings { get; set; }

    public abstract IColumnFilter<T> Filter { get; }

    public abstract string FilterWidgetTypeName { get; }

    public object FilterWidgetData { get; protected set; }

    public IGridColumn<T> Titled(string title)
    {
      this.Title = title;
      return (IGridColumn<T>) this;
    }

    public IGridColumn<T> Encoded(bool encode)
    {
      this.EncodeEnabled = encode;
      return (IGridColumn<T>) this;
    }

    IGridColumn<T> IColumn<T>.SetWidth(string width)
    {
      this.Width = width;
      return (IGridColumn<T>) this;
    }

    IGridColumn<T> IColumn<T>.SetWidth(int width)
    {
      this.Width = width.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "px";
      return (IGridColumn<T>) this;
    }

    public IGridColumn<T> Css(string cssClasses)
    {
      if (string.IsNullOrEmpty(cssClasses))
        return (IGridColumn<T>) this;
      GridStyledRenderer gridStyledRenderer1 = this.HeaderRenderer as GridStyledRenderer;
      if (gridStyledRenderer1 != null)
        gridStyledRenderer1.AddCssClass(cssClasses);
      GridStyledRenderer gridStyledRenderer2 = this.CellRenderer as GridStyledRenderer;
      if (gridStyledRenderer2 != null)
        gridStyledRenderer2.AddCssClass(cssClasses);
      return (IGridColumn<T>) this;
    }

    public IGridColumn<T> RenderValueAs(Func<T, string> constraint)
    {
      this.ValueConstraint = constraint;
      return (IGridColumn<T>) this;
    }

    public IGridColumn<T> Format(string pattern)
    {
      this.ValuePattern = pattern;
      return (IGridColumn<T>) this;
    }

    public virtual IGridColumn<T> Sanitized(bool sanitize)
    {
      this.SanitizeEnabled = sanitize;
      return (IGridColumn<T>) this;
    }

    public IGridColumn<T> SetInitialFilter(GridFilterType type, string value)
    {
      this.InitialFilterSettings = new ColumnFilterValue()
      {
        FilterType = type,
        FilterValue = value,
        ColumnName = this.Name
      };
      return (IGridColumn<T>) this;
    }

    public abstract IGridColumn<T> SortInitialDirection(GridSortDirection direction);

    public abstract IGridColumn<T> ThenSortBy<TKey>(Expression<Func<T, TKey>> expression);

    public abstract IGridColumn<T> ThenSortByDescending<TKey>(Expression<Func<T, TKey>> expression);

    public abstract IGridColumn<T> Sortable(bool sort);

    public abstract IGridCell GetCell(object instance);

    public abstract IGridColumn<T> Filterable(bool showColumnValuesVariants);

    public abstract IGridColumn<T> SetFilterWidgetType(string typeName);

    public abstract IGridColumn<T> SetFilterWidgetType(string typeName, object widgetData);

    public abstract IGridCell GetValue(T instance);
  }
}
