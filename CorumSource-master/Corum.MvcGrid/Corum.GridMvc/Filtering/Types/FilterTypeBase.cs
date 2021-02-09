// Decompiled with JetBrains decompiler
// Type: GridMvc.Filtering.Types.FilterTypeBase
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Filtering;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Filtering.Types
{
  internal abstract class FilterTypeBase : IFilterType
  {
    public abstract Type TargetType { get; }

    public abstract GridFilterType GetValidType(GridFilterType type);

    public abstract object GetTypedValue(string value);

    public virtual Expression GetFilterExpression(Expression leftExpr, string value, GridFilterType filterType)
    {
      filterType = this.GetValidType(filterType);
      object typedValue = this.GetTypedValue(value);
      if (typedValue == null)
        return (Expression) null;
      Type targetType = this.TargetType;
      Expression right = (Expression) Expression.Constant(typedValue);
      switch (filterType)
      {
        case GridFilterType.Equals:
          return (Expression) Expression.Equal(leftExpr, right);
        case GridFilterType.Contains:
          MethodInfo method1 = this.TargetType.GetMethod("Contains", new Type[1]
          {
            typeof (string)
          });
          return (Expression) Expression.Call(leftExpr, method1, new Expression[1]
          {
            right
          });
        case GridFilterType.StartsWith:
          MethodInfo method2 = targetType.GetMethod("StartsWith", new Type[1]
          {
            typeof (string)
          });
          return (Expression) Expression.Call(leftExpr, method2, new Expression[1]
          {
            right
          });
        case GridFilterType.EndsWidth:
          MethodInfo method3 = targetType.GetMethod("EndsWith", new Type[1]
          {
            typeof (string)
          });
          return (Expression) Expression.Call(leftExpr, method3, new Expression[1]
          {
            right
          });
        case GridFilterType.GreaterThan:
          return (Expression) Expression.GreaterThan(leftExpr, right);
        case GridFilterType.LessThan:
          return (Expression) Expression.LessThan(leftExpr, right);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
