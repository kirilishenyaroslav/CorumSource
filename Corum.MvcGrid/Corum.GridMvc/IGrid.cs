// Decompiled with JetBrains decompiler
// Type: GridMvc.IGrid
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Columns;
using GridMvc.Html;
using GridMvc.Pagination;
using System.Collections.Generic;

namespace GridMvc
{
  public interface IGrid
  {
    GridRenderOptions RenderOptions { get; }

    IGridColumnCollection Columns { get; }

    IEnumerable<object> ItemsToDisplay { get; }

    int DisplayingItemsCount { get; }

    IGridPager Pager { get; }

    bool EnablePaging { get; }

    string EmptyGridText { get; }

    string Language { get; }

    ISanitizer Sanitizer { get; }

    IGridSettingsProvider Settings { get; }

    string GetRowCssClasses(object item);
  }
}
