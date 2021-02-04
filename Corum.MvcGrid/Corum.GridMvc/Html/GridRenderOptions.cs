// Decompiled with JetBrains decompiler
// Type: GridMvc.Html.GridRenderOptions
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

namespace GridMvc.Html
{
  public class GridRenderOptions
  {
    public string ViewName { get; set; }

    public bool AllowMultipleFilters { get; set; }

    public bool Selectable { get; set; }

    public string GridName { get; set; }

    public bool RenderRowsOnly { get; set; }

    public GridRenderOptions(string gridName, string viewName)
    {
      this.ViewName = viewName;
      this.GridName = gridName;
      this.Selectable = true;
      this.AllowMultipleFilters = false;
    }

    public GridRenderOptions()
      : this(string.Empty, "_Grid")
    {
    }

    public static GridRenderOptions Create(string gridName)
    {
      return new GridRenderOptions(gridName, "_Grid");
    }

    public static GridRenderOptions Create(string gridName, string viewName)
    {
      return new GridRenderOptions(gridName, viewName);
    }
  }
}
