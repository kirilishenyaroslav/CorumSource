// Decompiled with JetBrains decompiler
// Type: GridMvc.Pagination.PagerGridItemsProcessor`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc;
using System.Linq;

namespace GridMvc.Pagination
{
  public class PagerGridItemsProcessor<T> : IGridItemsProcessor<T> where T : class
  {
    private readonly IGridPager _pager;

    public PagerGridItemsProcessor(IGridPager pager)
    {
      this._pager = pager;
    }

    public IQueryable<T> Process(IQueryable<T> items)
    {
      this._pager.Initialize<T>(items);
      if (this._pager.CurrentPage <= 0)
        return items;
      int count = (this._pager.CurrentPage - 1) * this._pager.PageSize;
      return Queryable.Take<T>(Queryable.Skip<T>(items, count), this._pager.PageSize);
    }
  }
}
