// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.DefaultColumnBuilder`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;
using GridMvc.DataAnnotations;
using GridMvc.Sorting;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Columns
{
  internal class DefaultColumnBuilder<T> : IColumnBuilder<T> where T : class
  {
    private readonly IGridAnnotaionsProvider _annotaions;
    private readonly Grid<T> _grid;

    public bool DefaultSortEnabled { get; set; }

    public bool DefaultFilteringEnabled { get; set; }

    public DefaultColumnBuilder(Grid<T> grid, IGridAnnotaionsProvider annotaions)
    {
      this._grid = grid;
      this._annotaions = annotaions;
    }

    public IGridColumn<T> CreateColumn<TDataType>(Expression<Func<T, TDataType>> constraint, bool hidden)
    {
      if (constraint != null && !(constraint.Body is MemberExpression))
        throw new NotSupportedException(string.Format("Expression '{0}' not supported by grid", (object) constraint));
      if (!hidden)
        return (IGridColumn<T>) new GridColumn<T, TDataType>(constraint, this._grid);
      return (IGridColumn<T>) new HiddenGridColumn<T, TDataType>(constraint, (IGrid) this._grid);
    }

    public IGridColumn<T> CreateColumn(PropertyInfo pi)
    {
      if (!this._annotaions.IsColumnMapped(pi))
        return (IGridColumn<T>) null;
      GridColumnAttribute annotationForColumn = this._annotaions.GetAnnotationForColumn<T>(pi);
      IGridColumn<T> column;
      if (annotationForColumn != null)
      {
        column = this.CreateColumn(pi, false);
        this.ApplyColumnAnnotationSettings(column, annotationForColumn);
      }
      else
      {
        GridHiddenColumnAttribute annotationForHiddenColumn = this._annotaions.GetAnnotationForHiddenColumn<T>(pi);
        if (annotationForHiddenColumn != null)
        {
          column = this.CreateColumn(pi, true);
          this.ApplyHiddenColumnAnnotationSettings(column, annotationForHiddenColumn);
        }
        else
        {
          column = this.CreateColumn(pi, false);
          this.ApplyColumnAnnotationSettings(column, new GridColumnAttribute());
        }
      }
      return column;
    }

    private IGridColumn<T> CreateColumn(PropertyInfo pi, bool hidden)
    {
      Type type1 = typeof (T);
      Type type2;
      if (!hidden)
        type2 = typeof (GridColumn<,>).MakeGenericType(type1, pi.PropertyType);
      else
        type2 = typeof (HiddenGridColumn<,>).MakeGenericType(type1, pi.PropertyType);
      ParameterExpression parameterExpression = Expression.Parameter(type1, "e");
      MemberExpression memberExpression = Expression.Property((Expression) parameterExpression, pi);
      LambdaExpression lambdaExpression = Expression.Lambda(typeof (Func<,>).MakeGenericType(type1, pi.PropertyType), (Expression) memberExpression, new ParameterExpression[1]
      {
        parameterExpression
      });
      IGridColumn<T> gridColumn = Activator.CreateInstance(type2, new object[2]
      {
        (object) lambdaExpression,
        (object) this._grid
      }) as IGridColumn<T>;
      if (!hidden && gridColumn != null)
      {
        gridColumn.Sortable(this.DefaultSortEnabled);
        gridColumn.Filterable(this.DefaultFilteringEnabled);
      }
      return gridColumn;
    }

    private void ApplyColumnAnnotationSettings(IGridColumn<T> column, GridColumnAttribute options)
    {
      column.Encoded(options.EncodeEnabled).Sanitized(options.SanitizeEnabled).Filterable(options.FilterEnabled).Sortable(options.SortEnabled);
      GridSortDirection? initialSortDirection = options.GetInitialSortDirection();
      if (initialSortDirection.HasValue)
        column.SortInitialDirection(initialSortDirection.Value);
      if (!string.IsNullOrEmpty(options.FilterWidgetType))
        column.SetFilterWidgetType(options.FilterWidgetType);
      if (!string.IsNullOrEmpty(options.Format))
        column.Format(options.Format);
      if (!string.IsNullOrEmpty(options.Title))
        column.Titled(options.Title);
      if (string.IsNullOrEmpty(options.Width))
        return;
      column.Width = options.Width;
    }

    private void ApplyHiddenColumnAnnotationSettings(IGridColumn<T> column, GridHiddenColumnAttribute options)
    {
      column.Encoded(options.EncodeEnabled).Sanitized(options.SanitizeEnabled);
      if (string.IsNullOrEmpty(options.Format))
        return;
      column.Format(options.Format);
    }
  }
}
