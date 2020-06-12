// Decompiled with JetBrains decompiler
// Type: GridMvc.Filtering.Types.BooleanFilterType
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Filtering;
using System;

namespace GridMvc.Filtering.Types
{
  internal sealed class BooleanFilterType : FilterTypeBase
  {
    public override Type TargetType
    {
      get
      {
        return typeof (bool);
      }
    }

    public override GridFilterType GetValidType(GridFilterType type)
    {
      return GridFilterType.Equals;
    }

    public override object GetTypedValue(string value)
    {
      bool result;
      if (!bool.TryParse(value, out result))
        return (object) null;
      return Convert.ToBoolean(result ? 1 : 0);
    }
  }
}
