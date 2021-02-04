// Decompiled with JetBrains decompiler
// Type: GridMvc.Utility.JsonHelper
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System.Web.Script.Serialization;

namespace GridMvc.Utility
{
  internal static class JsonHelper
  {
    public static string JsonSerializer<T>(T t)
    {
      return new JavaScriptSerializer().Serialize((object) t);
    }

    public static T JsonDeserialize<T>(string jsonString)
    {
      return new JavaScriptSerializer().Deserialize<T>(jsonString);
    }
  }
}
