// Decompiled with JetBrains decompiler
// Type: GridMvc.Columns.IFilterableColumn`1
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Filtering;

namespace GridMvc.Columns
{
  public interface IFilterableColumn<T>
  {
    IColumnFilter<T> Filter { get; }

    IGridColumn<T> Filterable(bool enalbe);

    IGridColumn<T> SetInitialFilter(GridFilterType type, string value);

    IGridColumn<T> SetFilterWidgetType(string typeName);

    IGridColumn<T> SetFilterWidgetType(string typeName, object widgetData);
  }
}
