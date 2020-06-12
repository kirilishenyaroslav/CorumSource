// Decompiled with JetBrains decompiler
// Type: GridMvc.Filtering.Types.FilterTypeResolver
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;
using System.Collections.Generic;

namespace GridMvc.Filtering.Types
{
  internal class FilterTypeResolver
  {
    private readonly List<IFilterType> _filterCollection = new List<IFilterType>();

    public FilterTypeResolver()
    {
      this._filterCollection.Add((IFilterType) new TextFilterType());
      this._filterCollection.Add((IFilterType) new IntegerFilterType());
      this._filterCollection.Add((IFilterType) new BooleanFilterType());
      this._filterCollection.Add((IFilterType) new DateTimeFilterType());
      this._filterCollection.Add((IFilterType) new DecimalFilterType());
      this._filterCollection.Add((IFilterType) new IntegerFilterType());
      this._filterCollection.Add((IFilterType) new ByteFilterType());
      this._filterCollection.Add((IFilterType) new SingleFilterType());
      this._filterCollection.Add((IFilterType) new LongFilterType());
      this._filterCollection.Add((IFilterType) new DoubleFilterType());
    }

    public IFilterType GetFilterType(Type type)
    {
      foreach (IFilterType filterType in this._filterCollection)
      {
        if (filterType.TargetType.FullName == type.FullName)
          return filterType;
      }
      return (IFilterType) new TextFilterType();
    }
  }
}
