// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.GridColumn`2
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;
using GridMvc.Filtering;
using GridMvc.Sorting;
using GridMvc.Utility;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GridMvc.Columns
{
  public class GridColumn<T, TDataType> : GridColumnBase<T> where T : class
  {
    private readonly List<IColumnOrderer<T>> _orderers = new List<IColumnOrderer<T>>();
    private readonly Func<T, TDataType> _constraint;
    private readonly IColumnFilter<T> _filter;
    private readonly Grid<T> _grid;
    private IGridCellRenderer _cellRenderer;
    private string _filterWidgetTypeName;
    private IGridColumnHeaderRenderer _headerRenderer;

    public override IGridColumnHeaderRenderer HeaderRenderer
    {
      get
      {
        if (this._headerRenderer == null)
          this._headerRenderer = this._grid.Settings.GetHeaderRenderer();
        return this._headerRenderer;
      }
      set
      {
        this._headerRenderer = value;
      }
    }

    public override IGridCellRenderer CellRenderer
    {
      get
      {
        return this._cellRenderer;
      }
      set
      {
        this._cellRenderer = value;
      }
    }

    public override IEnumerable<IColumnOrderer<T>> Orderers
    {
      get
      {
        return (IEnumerable<IColumnOrderer<T>>) this._orderers;
      }
    }

    public override bool FilterEnabled { get; set; }

    public override IColumnFilter<T> Filter
    {
      get
      {
        return this._filter;
      }
    }

    public override string FilterWidgetTypeName
    {
      get
      {
        return this._filterWidgetTypeName;
      }
    }

    public override IGrid ParentGrid
    {
      get
      {
        return (IGrid) this._grid;
      }
    }

    public GridColumn(Expression<Func<T, TDataType>> expression, Grid<T> grid)
    {
      this.EncodeEnabled = true;
      this.SortEnabled = false;
      this.SanitizeEnabled = true;
      this._filterWidgetTypeName = PropertiesHelper.GetUnderlyingType(typeof (TDataType)).FullName;
      this._grid = grid;
      this._cellRenderer = (IGridCellRenderer) new GridCellRenderer();
      if (expression == null)
        return;
      MemberExpression memberExpr = expression.Body as MemberExpression;
      if (memberExpr == null)
        throw new ArgumentException(string.Format("Expression '{0}' must be a member expression", (object) expression), "expression");
      this._constraint = expression.Compile();
      this._orderers.Insert(0, (IColumnOrderer<T>) new OrderByGridOrderer<T, TDataType>(expression));
      this._filter = (IColumnFilter<T>) new DefaultColumnFilter<T, TDataType>(expression);
      this.Name = PropertiesHelper.BuildColumnNameFromMemberExpression(memberExpr);
      this.Title = this.Name;
    }

    public override IGridColumn<T> SetFilterWidgetType(string typeName, object widgetData)
    {
      this.SetFilterWidgetType(typeName);
      if (widgetData != null)
        this.FilterWidgetData = widgetData;
      return (IGridColumn<T>) this;
    }

    public override IGridColumn<T> SetFilterWidgetType(string typeName)
    {
      if (!string.IsNullOrEmpty(typeName))
        this._filterWidgetTypeName = typeName;
      return (IGridColumn<T>) this;
    }

    public override IGridColumn<T> SortInitialDirection(GridSortDirection direction)
    {
      this.IsSorted = true;
      this.Direction = new GridSortDirection?(direction);
      return (IGridColumn<T>) this;
    }

    public override IGridColumn<T> ThenSortBy<TKey>(Expression<Func<T, TKey>> expression)
    {
      this._orderers.Add((IColumnOrderer<T>) new ThenByColumnOrderer<T, TKey>(expression, GridSortDirection.Ascending));
      return (IGridColumn<T>) this;
    }

    public override IGridColumn<T> ThenSortByDescending<TKey>(Expression<Func<T, TKey>> expression)
    {
      this._orderers.Add((IColumnOrderer<T>) new ThenByColumnOrderer<T, TKey>(expression, GridSortDirection.Descending));
      return (IGridColumn<T>) this;
    }

    public override IGridColumn<T> Sortable(bool sort)
    {
      if (sort && this._constraint == null)
        return (IGridColumn<T>) this;
      this.SortEnabled = sort;
      return (IGridColumn<T>) this;
    }

    public override IGridCell GetCell(object instance)
    {
      return this.GetValue((T) instance);
    }

    public override IGridCell GetValue(T instance)
    {
      string html;
      if (this.ValueConstraint != null)
      {
        html = this.ValueConstraint(instance);
      }
      else
      {
        if (this._constraint == null)
          throw new InvalidOperationException("You need to specify render expression using RenderValueAs");
        TDataType dataType = this._constraint(instance);
        html = (object) dataType != null ? (string.IsNullOrEmpty(this.ValuePattern) ? dataType.ToString() : string.Format(this.ValuePattern, (object) dataType)) : string.Empty;
      }
      if (!this.EncodeEnabled && this.SanitizeEnabled)
        html = this._grid.Sanitizer.Sanitize(html);
      return (IGridCell) new GridCell(html)
      {
        Encode = this.EncodeEnabled
      };
    }

    public override IGridColumn<T> Filterable(bool enable)
    {
      if (enable && this._constraint == null)
        return (IGridColumn<T>) this;
      this.FilterEnabled = enable;
      return (IGridColumn<T>) this;
    }
  }
}
