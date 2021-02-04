// Decompiled with JetBrains decompiler
// Type: GridMvc.DataAnnotations.IGridAnnotaionsProvider
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System.Reflection;

namespace GridMvc.DataAnnotations
{
  internal interface IGridAnnotaionsProvider
  {
    GridColumnAttribute GetAnnotationForColumn<T>(PropertyInfo pi);

    GridHiddenColumnAttribute GetAnnotationForHiddenColumn<T>(PropertyInfo pi);

    bool IsColumnMapped(PropertyInfo pi);

    GridTableAttribute GetAnnotationForTable<T>();
  }
}
