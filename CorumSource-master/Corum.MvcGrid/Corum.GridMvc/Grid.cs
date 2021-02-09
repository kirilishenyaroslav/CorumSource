// Decompiled with JetBrains decompiler
// Type: GridMvc.Grid`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Columns;
using GridMvc.DataAnnotations;
using GridMvc.Filtering;
using GridMvc.Html;
using GridMvc.Pagination;
using GridMvc.Resources;
using GridMvc.Sorting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GridMvc
{
  public class Grid<T> : GridBase<T>, IGrid where T : class
  {
    private int _displayingItemsCount = -1;
    private readonly IGridAnnotaionsProvider _annotaions;
    private readonly IColumnBuilder<T> _columnBuilder;
    private readonly GridColumnCollection<T> _columnsCollection;
    private readonly FilterGridItemsProcessor<T> _currentFilterItemsProcessor;
    private readonly SortGridItemsProcessor<T> _currentSortItemsProcessor;
    private bool _enablePaging;
    private IGridPager _pager;
    private IGridItemsProcessor<T> _pagerProcessor;
    private IGridSettingsProvider _settings;

    public IGridColumnCollection<T> Columns
    {
      get
      {
        return (IGridColumnCollection<T>) this._columnsCollection;
      }
    }

    public bool DefaultSortEnabled
    {
      get
      {
        return this._columnBuilder.DefaultSortEnabled;
      }
      set
      {
        this._columnBuilder.DefaultSortEnabled = value;
      }
    }

    public bool DefaultFilteringEnabled
    {
      get
      {
        return this._columnBuilder.DefaultFilteringEnabled;
      }
      set
      {
        this._columnBuilder.DefaultFilteringEnabled = value;
      }
    }

    public GridRenderOptions RenderOptions { get; set; }

    public override IGridSettingsProvider Settings
    {
      get
      {
        return this._settings;
      }
      set
      {
        this._settings = value;
        this._currentSortItemsProcessor.UpdateSettings(this._settings.SortSettings);
        this._currentFilterItemsProcessor.UpdateSettings(this._settings.FilterSettings);
      }
    }

    IEnumerable<object> IGrid.ItemsToDisplay
    {
      get
      {
        return (IEnumerable<object>) this.GetItemsToDisplay();
      }
    }

    public virtual int DisplayingItemsCount
    {
      get
      {
        if (this._displayingItemsCount >= 0)
          return this._displayingItemsCount;
        this._displayingItemsCount = Enumerable.Count<T>(this.GetItemsToDisplay());
        return this._displayingItemsCount;
      }
    }

    public bool EnablePaging
    {
      get
      {
        return this._enablePaging;
      }
      set
      {
        if (this._enablePaging == value)
          return;
        this._enablePaging = value;
        if (this._enablePaging)
        {
          if (this._pagerProcessor == null)
            this._pagerProcessor = (IGridItemsProcessor<T>) new PagerGridItemsProcessor<T>(this.Pager);
          this.AddItemsProcessor(this._pagerProcessor);
        }
        else
          this.RemoveItemsProcessor(this._pagerProcessor);
      }
    }

    public string Language { get; set; }

    public ISanitizer Sanitizer { get; set; }

    public IGridPager Pager
    {
      get
      {
        return this._pager ?? (this._pager = (IGridPager) new GridPager());
      }
      set
      {
        this._pager = value;
      }
    }

    IGridColumnCollection IGrid.Columns
    {
      get
      {
        return (IGridColumnCollection) this.Columns;
      }
    }

    public Grid(IEnumerable<T> items)
      : this(Queryable.AsQueryable<T>(items))
    {
    }

    public Grid(IQueryable<T> items)
      : base(items)
    {
      this._settings = (IGridSettingsProvider) new QueryStringGridSettingsProvider();
      this.Sanitizer = (ISanitizer) new GridMvc.Sanitizer();
      this.EmptyGridText = Strings.DefaultGridEmptyText;
      this.Language = Strings.Lang;
      this._currentSortItemsProcessor = new SortGridItemsProcessor<T>((IGrid) this, this._settings.SortSettings);
      this._currentFilterItemsProcessor = new FilterGridItemsProcessor<T>((IGrid) this, this._settings.FilterSettings);
      this.AddItemsPreProcessor((IGridItemsProcessor<T>) this._currentFilterItemsProcessor);
      this.InsertItemsProcessor(0, (IGridItemsProcessor<T>) this._currentSortItemsProcessor);
      this._annotaions = (IGridAnnotaionsProvider) new GridAnnotaionsProvider();
      this._columnBuilder = (IColumnBuilder<T>) new DefaultColumnBuilder<T>(this, this._annotaions);
      this._columnsCollection = new GridColumnCollection<T>(this._columnBuilder, this._settings.SortSettings);
      this.RenderOptions = new GridRenderOptions();
      this.ApplyGridSettings();
    }

    private void ApplyGridSettings()
    {
      GridTableAttribute annotationForTable = this._annotaions.GetAnnotationForTable<T>();
      if (annotationForTable == null)
        return;
      this.EnablePaging = annotationForTable.PagingEnabled;
      if (annotationForTable.PageSize > 0)
        this.Pager.PageSize = annotationForTable.PageSize;
      if (annotationForTable.PagingMaxDisplayedPages <= 0 || !(this.Pager is GridPager))
        return;
      (this.Pager as GridPager).MaxDisplayedPages = annotationForTable.PagingMaxDisplayedPages;
    }

    protected internal virtual IEnumerable<T> GetItemsToDisplay()
    {
      this.PrepareItemsToDisplay();
      return this.AfterItems;
    }

    public virtual void AutoGenerateColumns()
    {
      foreach (PropertyInfo pi in typeof (T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (pi.CanRead)
          this.Columns.Add(pi);
      }
    }
  }
}
