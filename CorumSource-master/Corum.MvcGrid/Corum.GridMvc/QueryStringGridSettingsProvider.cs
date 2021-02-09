// Decompiled with JetBrains decompiler
// Type: GridMvc.QueryStringGridSettingsProvider
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Filtering;
using GridMvc.Sorting;

namespace GridMvc
{
  public class QueryStringGridSettingsProvider : IGridSettingsProvider
  {
    private readonly QueryStringFilterSettings _filterSettings;
    private readonly QueryStringSortSettings _sortSettings;

    public IGridSortSettings SortSettings
    {
      get
      {
        return (IGridSortSettings) this._sortSettings;
      }
    }

    public IGridFilterSettings FilterSettings
    {
      get
      {
        return (IGridFilterSettings) this._filterSettings;
      }
    }

    public QueryStringGridSettingsProvider()
    {
      this._sortSettings = new QueryStringSortSettings();
      this._filterSettings = new QueryStringFilterSettings();
    }

    public IGridColumnHeaderRenderer GetHeaderRenderer()
    {
      GridHeaderRenderer gridHeaderRenderer = new GridHeaderRenderer();
      gridHeaderRenderer.AddAdditionalRenderer((IGridColumnHeaderRenderer) new QueryStringFilterColumnHeaderRenderer(this._filterSettings));
      gridHeaderRenderer.AddAdditionalRenderer((IGridColumnHeaderRenderer) new QueryStringSortColumnHeaderRenderer(this._sortSettings));
      return (IGridColumnHeaderRenderer) gridHeaderRenderer;
    }
  }
}
