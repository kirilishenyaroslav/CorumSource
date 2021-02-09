// Decompiled with JetBrains decompiler
// Type: GridMvc.GridStyledRenderer
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System.Collections.Generic;

namespace GridMvc
{
  public abstract class GridStyledRenderer
  {
    private readonly List<string> _classes = new List<string>();
    private readonly List<string> _styles = new List<string>();

    protected string GetCssClassesString()
    {
      return string.Join(" ", (IEnumerable<string>) this._classes);
    }

    protected string GetCssStylesString()
    {
      return string.Join(" ", (IEnumerable<string>) this._styles);
    }

    public void AddCssClass(string className)
    {
      if (this._classes.Contains(className))
        return;
      this._classes.Add(className);
    }

    public void AddCssStyle(string styleString)
    {
      if (this._styles.Contains(styleString))
        return;
      this._styles.Add(styleString);
    }
  }
}
