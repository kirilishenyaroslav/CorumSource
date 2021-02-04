// Decompiled with JetBrains decompiler
// Type: GridMvc.GridCell
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System.Web;

namespace GridMvc
{
  public class GridCell : IGridCell
  {
    private readonly string _value;

    public bool Encode { get; set; }

    public string Value
    {
      get
      {
        if (!this.Encode || string.IsNullOrEmpty(this._value))
          return this._value;
        return HttpUtility.HtmlEncode(this._value);
      }
    }

    public GridCell(string value)
    {
      this._value = value;
    }

    public override string ToString()
    {
      return this.Value;
    }
  }
}
