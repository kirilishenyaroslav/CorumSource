// Decompiled with JetBrains decompiler
// Type: GridMvc.GridCellRenderer
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Columns;
using System.Web;

namespace GridMvc
{
  public class GridCellRenderer : GridStyledRenderer, IGridCellRenderer
  {
    private const string TdClass = "grid-cell";

    public GridCellRenderer()
    {
      this.AddCssClass("grid-cell");
    }

    public IHtmlString Render(IGridColumn column, IGridCell cell)
    {
      string cssStylesString = this.GetCssStylesString();
      string cssClassesString = this.GetCssClassesString();
      System.Web.Mvc.TagBuilder tagBuilder = new System.Web.Mvc.TagBuilder("td");
      if (!string.IsNullOrWhiteSpace(cssClassesString))
        tagBuilder.AddCssClass(cssClassesString);
      if (!string.IsNullOrWhiteSpace(cssStylesString))
        tagBuilder.MergeAttribute("style", cssStylesString);
      tagBuilder.MergeAttribute("data-name", column.Name);
      tagBuilder.InnerHtml = cell.ToString();
      return (IHtmlString)System.Web.Mvc.MvcHtmlString.Create(tagBuilder.ToString());
    }
  }
}
