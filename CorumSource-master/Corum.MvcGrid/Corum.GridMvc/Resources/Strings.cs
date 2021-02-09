// Decompiled with JetBrains decompiler
// Type: GridMvc.Resources.Strings
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GridMvc.Resources
{
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [CompilerGenerated]
  internal class Strings
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Strings.resourceMan, (object) null))
          Strings.resourceMan = new ResourceManager("GridMvc.Resources.Strings", typeof (Strings).Assembly);
        return Strings.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Strings.resourceCulture;
      }
      set
      {
        Strings.resourceCulture = value;
      }
    }

    internal static string DefaultGridEmptyText
    {
      get
      {
        return Strings.ResourceManager.GetString("DefaultGridEmptyText", Strings.resourceCulture);
      }
    }

    internal static string FilterButtonTooltipText
    {
      get
      {
        return Strings.ResourceManager.GetString("FilterButtonTooltipText", Strings.resourceCulture);
      }
    }

    internal static string Lang
    {
      get
      {
        return Strings.ResourceManager.GetString("Lang", Strings.resourceCulture);
      }
    }

    internal Strings()
    {
    }
  }
}
