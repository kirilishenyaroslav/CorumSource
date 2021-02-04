// Decompiled with JetBrains decompiler
// Type: GridMvc.Utility.PropertiesHelper
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace GridMvc.Utility
{
  internal static class PropertiesHelper
  {
    private const string PropertiesQueryStringDelimeter = ".";

    public static string BuildColumnNameFromMemberExpression(MemberExpression memberExpr)
    {
      StringBuilder stringBuilder = new StringBuilder();
      Expression nextExpr = (Expression) memberExpr;
      while (true)
      {
        string expressionMemberName = PropertiesHelper.GetExpressionMemberName(nextExpr, ref nextExpr);
        if (!string.IsNullOrEmpty(expressionMemberName))
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Insert(0, ".");
          stringBuilder.Insert(0, expressionMemberName);
        }
        else
          break;
      }
      return stringBuilder.ToString();
    }

    private static string GetExpressionMemberName(Expression expr, ref Expression nextExpr)
    {
      if (expr is MemberExpression)
      {
        MemberExpression memberExpression = (MemberExpression) expr;
        nextExpr = memberExpression.Expression;
        return memberExpression.Member.Name;
      }
      if (!(expr is BinaryExpression) || expr.NodeType != ExpressionType.ArrayIndex)
        return string.Empty;
      BinaryExpression binaryExpression = (BinaryExpression) expr;
      string expressionMemberName = PropertiesHelper.GetExpressionMemberName(binaryExpression.Left, ref nextExpr);
      if (string.IsNullOrEmpty(expressionMemberName))
        throw new InvalidDataException("Cannot parse your column expression");
      return string.Format("{0}[{1}]", (object) expressionMemberName, (object) binaryExpression.Right);
    }

    public static PropertyInfo GetPropertyFromColumnName(string columnName, Type type, out IEnumerable<PropertyInfo> propertyInfoSequence)
    {
      string[] strArray = columnName.Split(new string[1]
      {
        "."
      }, StringSplitOptions.RemoveEmptyEntries);
      if (!Enumerable.Any<string>((IEnumerable<string>) strArray))
      {
        propertyInfoSequence = (IEnumerable<PropertyInfo>) null;
        return (PropertyInfo) null;
      }
      PropertyInfo propertyInfo = (PropertyInfo) null;
      List<PropertyInfo> list = new List<PropertyInfo>();
      foreach (string name in strArray)
      {
        propertyInfo = type.GetProperty(name);
        if (propertyInfo == (PropertyInfo) null)
        {
          propertyInfoSequence = (IEnumerable<PropertyInfo>) null;
          return (PropertyInfo) null;
        }
        list.Add(propertyInfo);
        type = propertyInfo.PropertyType;
      }
      propertyInfoSequence = (IEnumerable<PropertyInfo>) list;
      return propertyInfo;
    }

    public static Type GetUnderlyingType(Type type)
    {
      return !type.IsGenericType || !(type.GetGenericTypeDefinition() == typeof (Nullable<>)) ? type : Nullable.GetUnderlyingType(type);
    }

    public static T GetAttribute<T>(this PropertyInfo pi)
    {
      return (T) Enumerable.FirstOrDefault<object>((IEnumerable<object>) pi.GetCustomAttributes(typeof (T), true));
    }

    public static T GetAttribute<T>(this Type type)
    {
      return (T) Enumerable.FirstOrDefault<object>((IEnumerable<object>) type.GetCustomAttributes(typeof (T), true));
    }
  }
}
