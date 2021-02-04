// Decompiled with JetBrains decompiler
// Type: GridMvc.GridBase`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace GridMvc
{
  public abstract class GridBase<T> where T : class
  {
    private readonly List<IGridItemsProcessor<T>> _preprocessors = new List<IGridItemsProcessor<T>>();
    private readonly List<IGridItemsProcessor<T>> _processors = new List<IGridItemsProcessor<T>>();
    protected IEnumerable<T> AfterItems;
    protected IQueryable<T> BeforeItems;
    private bool _itemsPreProcessed;
    private bool _itemsProcessed;
    private Func<T, string> _rowCssClassesContraint;

    public abstract IGridSettingsProvider Settings { get; set; }

    private IQueryable<T> GridItems
    {
      get
      {
        if (!this._itemsPreProcessed)
        {
          this._itemsPreProcessed = true;
          foreach (IGridItemsProcessor<T> gridItemsProcessor in this._preprocessors)
            this.BeforeItems = gridItemsProcessor.Process(this.BeforeItems);
        }
        return this.BeforeItems;
      }
    }

    public string EmptyGridText { get; set; }

    protected GridBase(IQueryable<T> items)
    {
      this.BeforeItems = items;
    }

    public void SetRowCssClassesContraint(Func<T, string> contraint)
    {
      this._rowCssClassesContraint = contraint;
    }

    public string GetRowCssClasses(object item)
    {
      if (this._rowCssClassesContraint == null)
        return string.Empty;
      T obj = item as T;
      if ((object) obj == null)
        throw new InvalidCastException(string.Format("The item must be of type '{0}'", (object) typeof (T).FullName));
      return this._rowCssClassesContraint(obj);
    }

    protected void PrepareItemsToDisplay()
    {
      if (this._itemsProcessed) return;
      this._itemsProcessed = true;
      IQueryable<T> items = this.GridItems;
      foreach (IGridItemsProcessor<T> gridItemsProcessor in Enumerable.Where<IGridItemsProcessor<T>>((IEnumerable<IGridItemsProcessor<T>>) this._processors, (Func<IGridItemsProcessor<T>, bool>) (p => p != null)))
        items = gridItemsProcessor.Process(items);
      this.AfterItems = (IEnumerable<T>) Enumerable.ToList<T>((IEnumerable<T>) items);
    }

    protected void AddItemsProcessor(IGridItemsProcessor<T> processor)
    {
      if (this._processors.Contains(processor))
        return;
      this._processors.Add(processor);
    }

    protected void RemoveItemsProcessor(IGridItemsProcessor<T> processor)
    {
      if (!this._processors.Contains(processor))
        return;
      this._processors.Remove(processor);
    }

    protected void AddItemsPreProcessor(IGridItemsProcessor<T> processor)
    {
      if (this._preprocessors.Contains(processor))
        return;
      this._preprocessors.Add(processor);
    }

    protected void RemoveItemsPreProcessor(IGridItemsProcessor<T> processor)
    {
      if (!this._preprocessors.Contains(processor))
        return;
      this._preprocessors.Remove(processor);
    }

    protected void InsertItemsProcessor(int position, IGridItemsProcessor<T> processor)
    {
      if (this._processors.Contains(processor))
        return;
      this._processors.Insert(position, processor);
    }
  }
}
