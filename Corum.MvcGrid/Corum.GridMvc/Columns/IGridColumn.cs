﻿// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.IGridColumn
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;

namespace GridMvc.Columns
{
  public interface IGridColumn : ISortableColumn, IFilterableColumn, IColumn
  {
    IGrid ParentGrid { get; }
  }
}
