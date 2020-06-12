﻿// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.IColumn
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;

namespace GridMvc.Columns
{
  public interface IColumn
  {
    string Title { get; }

    string Name { get; set; }

    string Width { get; set; }

    bool EncodeEnabled { get; }

    bool SanitizeEnabled { get; }

    IGridColumnHeaderRenderer HeaderRenderer { get; set; }

    IGridCellRenderer CellRenderer { get; set; }

    IGridCell GetCell(object instance);
  }
}
