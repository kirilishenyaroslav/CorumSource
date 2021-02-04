// Decompiled with JetBrains decompiler
// Type: GridMvc.Filtering.ColumnFilterValue
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System.Runtime.Serialization;
using System.Web;

namespace GridMvc.Filtering
{
  [DataContract]
  public struct ColumnFilterValue
  {
    public string ColumnName;
    [DataMember(Name = "filterType")]
    public GridFilterType FilterType;
    public string FilterValue;

    [DataMember(Name = "filterValue")]
    internal string FilterValueEncoded
    {
      get
      {
        return HttpUtility.UrlEncode(this.FilterValue);
      }
      set
      {
        this.FilterValue = value;
      }
    }

    public static ColumnFilterValue Null
    {
      get
      {
        return new ColumnFilterValue();
      }
    }

    public static bool operator ==(ColumnFilterValue a, ColumnFilterValue b)
    {
      if (a.ColumnName == b.ColumnName && a.FilterType == b.FilterType)
        return a.FilterValue == b.FilterValue;
      return false;
    }

    public static bool operator !=(ColumnFilterValue a, ColumnFilterValue b)
    {
      if (!(a.ColumnName != b.ColumnName) && a.FilterType == b.FilterType)
        return a.FilterValue != b.FilterValue;
      return true;
    }
  }
}
