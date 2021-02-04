// Decompiled with JetBrains decompiler
// Type: GridMvc.Filtering.Types.TextFilterType
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Filtering;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Filtering.Types
{
  internal sealed class TextFilterType : FilterTypeBase
  {
    public override Type TargetType
    {
      get
      {
        return typeof (string);
      }
    }

    public override GridFilterType GetValidType(GridFilterType type)
    {
      switch (type)
      {
        case GridFilterType.Equals:
        case GridFilterType.Contains:
        case GridFilterType.StartsWith:
        case GridFilterType.EndsWidth:
          return type;
        default:
          return GridFilterType.Equals;
      }
    }

    public override object GetTypedValue(string value)
    {
      return (object) value;
    }

    public override Expression GetFilterExpression(Expression leftExpr, string value, GridFilterType filterType)
    {
      filterType = this.GetValidType(filterType);
      object typedValue = this.GetTypedValue(value);
      if (typedValue == null)
        return (Expression) null;
      Expression rightExpr = (Expression) Expression.Constant(typedValue);
      switch (filterType)
      {
        case GridFilterType.Equals:
          return this.GetCaseInsensitiveСompartion(string.Empty, leftExpr, rightExpr);
        case GridFilterType.Contains:
          return this.GetCaseInsensitiveСompartion("Contains", leftExpr, rightExpr);
        case GridFilterType.StartsWith:
          return this.GetCaseInsensitiveСompartion("StartsWith", leftExpr, rightExpr);
        case GridFilterType.EndsWidth:
          return this.GetCaseInsensitiveСompartion("EndsWith", leftExpr, rightExpr);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private Expression GetCaseInsensitiveСompartion(string methodName, Expression leftExpr, Expression rightExpr)
    {
      Type targetType = this.TargetType;
      MethodInfo method1 = targetType.GetMethod("ToUpper", new Type[0]);
      MethodCallExpression methodCallExpression1 = Expression.Call(rightExpr, method1);
      MethodCallExpression methodCallExpression2 = Expression.Call(leftExpr, method1);
      if (string.IsNullOrEmpty(methodName))
        return (Expression) Expression.Equal((Expression) methodCallExpression2, (Expression) methodCallExpression1);
      MethodInfo method2 = targetType.GetMethod(methodName, new Type[1]
      {
        typeof (string)
      });
      if (method2 == (MethodInfo) null)
        throw new MissingMethodException("There is no method - " + methodName);
      return (Expression) Expression.Call((Expression) methodCallExpression2, method2, new Expression[1]
      {
        (Expression) methodCallExpression1
      });
    }
  }
}
