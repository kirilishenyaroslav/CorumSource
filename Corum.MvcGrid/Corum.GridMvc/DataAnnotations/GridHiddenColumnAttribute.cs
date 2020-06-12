// Decompiled with JetBrains decompiler
// Type: GridMvc.DataAnnotations.GridHiddenColumnAttribute
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;

namespace GridMvc.DataAnnotations
{
  public class GridHiddenColumnAttribute : Attribute
  {
    public bool EncodeEnabled { get; set; }

    public bool SanitizeEnabled { get; set; }

    public string Format { get; set; }

    public GridHiddenColumnAttribute()
    {
      this.EncodeEnabled = true;
      this.SanitizeEnabled = true;
    }
  }
}
