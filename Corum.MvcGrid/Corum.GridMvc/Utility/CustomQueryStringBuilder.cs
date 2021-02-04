// Decompiled with JetBrains decompiler
// Type: GridMvc.Utility.CustomQueryStringBuilder
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace GridMvc.Utility
{
  internal class CustomQueryStringBuilder : NameValueCollection
  {
    public CustomQueryStringBuilder(NameValueCollection collection)
      : base(collection)
    {
    }

    public override string ToString()
    {
      return this.GetQueryStringExcept((IList<string>) new string[0]);
    }

    public string GetQueryStringWithParameter(string parameterName, string parameterValue)
    {
      if (string.IsNullOrEmpty(parameterName))
        throw new ArgumentException("parameterName");
      if (this[parameterName] != null)
        this[parameterName] = parameterValue;
      else
        this.Add(parameterName, parameterValue);
      return this.ToString();
    }

    public string GetQueryStringExcept(IList<string> parameterNames)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string name in this.AllKeys)
      {
        if (!string.IsNullOrEmpty(name) && !parameterNames.Contains(name))
        {
          string[] values = this.GetValues(name);
          if (values != null && Enumerable.Count<string>((IEnumerable<string>) values) != 0)
          {
            if (stringBuilder.Length == 0)
              stringBuilder.Append("?");
            foreach (string str in values)
              stringBuilder.Append(name + "=" + HttpUtility.UrlEncode(str) + "&");
          }
        }
      }
      string str1 = stringBuilder.ToString();
      if (!str1.EndsWith("&"))
        return str1;
      return str1.Substring(0, str1.Length - 1);
    }
  }
}
