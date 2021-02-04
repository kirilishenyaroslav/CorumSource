// Decompiled with JetBrains decompiler
// Type: GridMvc.GridHeaderRenderer
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Columns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;


namespace GridMvc
{
  public class GridHeaderRenderer : GridStyledRenderer, IGridColumnHeaderRenderer
  {
    private readonly List<IGridColumnHeaderRenderer> _additionalRenders = new List<IGridColumnHeaderRenderer>();
    private const string ThClass = "grid-header";

    public GridHeaderRenderer()
    {
      this.AddCssClass("grid-header");
    }

    public IHtmlString Render(IGridColumn column)
    {
      string str = this.GetCssStylesString();
      string cssClassesString = this.GetCssClassesString();
      if (!string.IsNullOrWhiteSpace(column.Width))
        str = (str + " width:" + column.Width + ";").Trim();
            System.Web.Mvc.TagBuilder tagBuilder = new System.Web.Mvc.TagBuilder("th");
      if (!string.IsNullOrWhiteSpace(cssClassesString))
        tagBuilder.AddCssClass(cssClassesString);
      if (!string.IsNullOrWhiteSpace(str))
        tagBuilder.MergeAttribute("style", str);
      tagBuilder.InnerHtml = this.RenderAdditionalContent(column);
      return (IHtmlString)System.Web.Mvc.MvcHtmlString.Create(tagBuilder.ToString());
    }

    protected virtual string RenderAdditionalContent(IGridColumn column)
    {
      if (this._additionalRenders.Count == 0)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (IGridColumnHeaderRenderer columnHeaderRenderer in this._additionalRenders)
        stringBuilder.Append((object) columnHeaderRenderer.Render(column));
      return stringBuilder.ToString();
    }

    public void AddAdditionalRenderer(IGridColumnHeaderRenderer renderer)
    {
      if (this._additionalRenders.Contains(renderer))
        throw new InvalidOperationException("This renderer already exist");
      this._additionalRenders.Add(renderer);
    }

    public void InsertAdditionalRenderer(int position, IGridColumnHeaderRenderer renderer)
    {
      if (this._additionalRenders.Contains(renderer))
        throw new InvalidOperationException("This renderer already exist");
      this._additionalRenders.Insert(position, renderer);
    }
  }
}
