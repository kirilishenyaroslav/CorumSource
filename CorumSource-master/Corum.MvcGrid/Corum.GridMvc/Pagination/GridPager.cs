// Decompiled with JetBrains decompiler
// Type: GridMvc.Pagination.GridPager
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Utility;
using System;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GridMvc.Pagination
{
  public class GridPager : IGridPager
  {
    public const int DefaultMaxDisplayedPages = 5;
    public const int DefaultPageSize = 20;
    public const string DefaultPageQueryParameter = "grid-page";
    public const string DefaultPagerViewName = "_GridPager";
    private readonly HttpContext _context;
    private readonly CustomQueryStringBuilder _queryBuilder;
    private int _currentPage;
    private int _itemsCount;
    private int _maxDisplayedPages;
    private int _pageSize;

    public int PageSize
    {
      get
      {
        return this._pageSize;
      }
      set
      {
        this._pageSize = value;
        this.RecalculatePages();
      }
    }

    public int CurrentPage
    {
      get
      {
        if (this._currentPage >= 0)
          return this._currentPage;
        if (!int.TryParse(this._context.Request.QueryString[this.ParameterName] ?? "1", out this._currentPage))
          this._currentPage = 1;
        if (this._currentPage > this.PageCount)
          this._currentPage = this.PageCount;
        return this._currentPage;
      }
      protected internal set
      {
        this._currentPage = value;
        if (this._currentPage > this.PageCount)
          this._currentPage = this.PageCount;
        this.RecalculatePages();
      }
    }

    public string ParameterName { get; set; }

    public virtual int ItemsCount
    {
      get
      {
        return this._itemsCount;
      }
      set
      {
        this._itemsCount = value;
        this.RecalculatePages();
      }
    }

    public int MaxDisplayedPages
    {
      get
      {
        if (this._maxDisplayedPages != 0)
          return this._maxDisplayedPages;
        return 5;
      }
      set
      {
        this._maxDisplayedPages = value;
        this.RecalculatePages();
      }
    }

    public int PageCount { get; protected set; }

    public int StartDisplayedPage { get; protected set; }

    public int EndDisplayedPage { get; protected set; }

    public string TemplateName { get; set; }

    public GridPager()
      : this(HttpContext.Current)
    {
    }

    public GridPager(HttpContext context)
    {
      if (context == null)
        throw new Exception("No http context here!");
      this._context = context;
      this._currentPage = -1;
      this._queryBuilder = new CustomQueryStringBuilder(HttpContext.Current.Request.QueryString);
      this.ParameterName = "grid-page";
      this.TemplateName = "_GridPager";
      this.MaxDisplayedPages = this.MaxDisplayedPages;
      this.PageSize = 20;
    }

    public virtual void Initialize<T>(IQueryable<T> items)
    {
      this.ItemsCount = Queryable.Count<T>(items);
    }

    protected virtual void RecalculatePages()
    {
      if (this.ItemsCount == 0)
      {
        this.PageCount = 0;
      }
      else
      {
        this.PageCount = (int) Math.Ceiling((double) this.ItemsCount / (double) this.PageSize);
        this.StartDisplayedPage = this.CurrentPage - this.MaxDisplayedPages / 2 < 1 ? 1 : this.CurrentPage - this.MaxDisplayedPages / 2;
        this.EndDisplayedPage = this.CurrentPage + this.MaxDisplayedPages / 2 > this.PageCount ? this.PageCount : this.CurrentPage + this.MaxDisplayedPages / 2;
      }
    }

    public virtual string GetLinkForPage(int pageIndex)
    {
      return this._queryBuilder.GetQueryStringWithParameter(this.ParameterName, pageIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }
  }
}
