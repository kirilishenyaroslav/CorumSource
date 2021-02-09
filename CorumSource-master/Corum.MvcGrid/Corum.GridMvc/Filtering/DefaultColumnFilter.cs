// Decompiled with JetBrains decompiler
// Type: GridMvc.Filtering.DefaultColumnFilter`2
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Filtering.Types;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Filtering
{
  internal class DefaultColumnFilter<T, TData> : IColumnFilter<T>
  {
    private readonly FilterTypeResolver _typeResolver = new FilterTypeResolver();
    private readonly Expression<Func<T, TData>> _expression;

    public DefaultColumnFilter(Expression<Func<T, TData>> expression)
    {
      this._expression = expression;
    }

    public IQueryable<T> ApplyFilter(IQueryable<T> items, ColumnFilterValue value)
    {
      if (value == ColumnFilterValue.Null)
        throw new ArgumentNullException("value");
      Expression<Func<T, bool>> filterExpression = this.GetFilterExpression((PropertyInfo) ((MemberExpression) this._expression.Body).Member, value);
      if (filterExpression == null)
        return items;
      return Queryable.Where<T>(items, filterExpression);
    }

    private Expression<Func<T, bool>> GetFilterExpression(PropertyInfo pi, ColumnFilterValue value)
    {
      bool flag = pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>);
      Type type = flag ? Nullable.GetUnderlyingType(pi.PropertyType) : pi.PropertyType;
      IFilterType filterType = this._typeResolver.GetFilterType(type);
      ParameterExpression parameterExpression = this._expression.Parameters[0];
      Expression leftExpr = flag ? (Expression) Expression.Property(this._expression.Body, pi.PropertyType.GetProperty("Value")) : this._expression.Body;
      Expression expression = filterType.GetFilterExpression(leftExpr, value.FilterValue, value.FilterType);
      if (expression == null)
        return (Expression<Func<T, bool>>) null;
      if (type == typeof (string))
        expression = (Expression) Expression.AndAlso((Expression) Expression.NotEqual(this._expression.Body, (Expression) Expression.Constant((object) null)), expression);
      else if (flag)
        expression = (Expression) Expression.AndAlso((Expression) Expression.Property(this._expression.Body, pi.PropertyType.GetProperty("HasValue")), expression);
      return Expression.Lambda<Func<T, bool>>(expression, new ParameterExpression[1]
      {
        parameterExpression
      });
    }
  }
}
