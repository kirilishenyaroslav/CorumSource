// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.HiddenGridColumn`2
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
  public class HiddenGridColumn<T, TDataType> : GridColumnBase<T>
  {
    private readonly Func<T, TDataType> _constraint;
    private readonly IGrid _grid;
    private IGridCellRenderer _cellRenderer;

    public override IEnumerable<IColumnOrderer<T>> Orderers
    {
      get
      {
        throw new InvalidOperationException("You cannot sort hidden field");
      }
    }

    public override IGridColumnHeaderRenderer HeaderRenderer
    {
      get
      {
        return (IGridColumnHeaderRenderer) new GridHiddenHeaderRenderer();
      }
      set
      {
        throw new InvalidOperationException("You can't set header renderer of hidden column");
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

    public override bool FilterEnabled
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public override IColumnFilter<T> Filter
    {
      get
      {
        return (IColumnFilter<T>) null;
      }
    }

    public override string FilterWidgetTypeName
    {
      get
      {
        return PropertiesHelper.GetUnderlyingType(typeof (TDataType)).FullName;
      }
    }

    public override IGrid ParentGrid
    {
      get
      {
        return this._grid;
      }
    }

    public HiddenGridColumn(Expression<Func<T, TDataType>> expression, IGrid grid)
    {
      this._grid = grid;
      this._cellRenderer = (IGridCellRenderer) new GridHiddenCellRenderer();
      this.SortEnabled = false;
      if (expression == null)
        return;
      MemberExpression memberExpr = expression.Body as MemberExpression;
      if (memberExpr == null)
        throw new ArgumentException(string.Format("Expression '{0}' must be a member expression", (object) expression), "expression");
      this._constraint = expression.Compile();
      this.Name = PropertiesHelper.BuildColumnNameFromMemberExpression(memberExpr);
    }

    public override IGridColumn<T> SetFilterWidgetType(string typeName, object widgetData)
    {
      return (IGridColumn<T>) this;
    }

    public override IGridColumn<T> SetFilterWidgetType(string typeName)
    {
      return (IGridColumn<T>) this;
    }

    public override IGridColumn<T> SortInitialDirection(GridSortDirection direction)
    {
      return (IGridColumn<T>) this;
    }

    public override IGridColumn<T> ThenSortBy<TKey>(Expression<Func<T, TKey>> expression)
    {
      return (IGridColumn<T>) this;
    }

    public override IGridColumn<T> ThenSortByDescending<TKey>(Expression<Func<T, TKey>> expression)
    {
      return (IGridColumn<T>) this;
    }

    public override IGridColumn<T> Sortable(bool sort)
    {
      return (IGridColumn<T>) this;
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

    public override IGridColumn<T> Filterable(bool showColumnValuesVariants)
    {
      return (IGridColumn<T>) this;
    }

    public override IGridCell GetCell(object instance)
    {
      return this.GetValue((T) instance);
    }
  }
}
