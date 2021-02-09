// Decompiled with JetBrains decompiler
// Type: GridMvc.DataAnnotations.GridTableAttribute
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;

namespace GridMvc.DataAnnotations
{
  [AttributeUsage(AttributeTargets.Class)]
  public class GridTableAttribute : Attribute
  {
    public bool PagingEnabled { get; set; }

    public int PageSize { get; set; }

    public int PagingMaxDisplayedPages { get; set; }

    public GridTableAttribute()
    {
      this.PagingEnabled = false;
      this.PageSize = 0;
      this.PagingMaxDisplayedPages = 0;
    }
  }
}
