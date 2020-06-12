// Decompiled with JetBrains decompiler
// Type: GridMvc.Filtering.DefaultFilterColumnCollection
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Columns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GridMvc.Filtering
{
  public class DefaultFilterColumnCollection : List<ColumnFilterValue>, IFilterColumnCollection, IEnumerable<ColumnFilterValue>, IEnumerable
  {
    public IEnumerable<ColumnFilterValue> GetByColumn(IGridColumn column)
    {
      return Enumerable.Where<ColumnFilterValue>((IEnumerable<ColumnFilterValue>) this, (Func<ColumnFilterValue, bool>) (c => c.ColumnName.ToUpper() == column.Name.ToUpper()));
    }
  }
}
