﻿// Decompiled with JetBrains decompiler
// Type: GridMvc.Filtering.Types.DecimalFilterType
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Filtering;
using System;

namespace GridMvc.Filtering.Types
{
  internal sealed class DecimalFilterType : FilterTypeBase
  {
    public override Type TargetType
    {
      get
      {
        return typeof (Decimal);
      }
    }

    public override GridFilterType GetValidType(GridFilterType type)
    {
      switch (type)
      {
        case GridFilterType.Equals:
        case GridFilterType.GreaterThan:
        case GridFilterType.LessThan:
          return type;
        default:
          return GridFilterType.Equals;
      }
    }

    public override object GetTypedValue(string value)
    {
      Decimal result;
      if (!Decimal.TryParse(value, out result))
        return (object) null;
      return (object) result;
    }
  }
}
