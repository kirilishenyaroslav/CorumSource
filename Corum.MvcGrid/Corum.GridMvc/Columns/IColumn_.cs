// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.IColumn`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;

namespace GridMvc.Columns
{
  public interface IColumn<T>
  {
    IGridColumn<T> Titled(string title);

    IGridColumn<T> Encoded(bool encode);

    IGridColumn<T> Sanitized(bool sanitize);

    IGridColumn<T> SetWidth(string width);

    IGridColumn<T> SetWidth(int width);

    IGridColumn<T> Css(string cssClasses);

    IGridColumn<T> RenderValueAs(Func<T, string> constraint);

    IGridColumn<T> Format(string pattern);
  }
}
