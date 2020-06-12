// Decompiled with JetBrains decompiler
// Type: GridMvc.DataAnnotations.GridAnnotaionsProvider
// Assembly: GridMvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90F7B3EF-1F87-4384-8BA7-2EB4A06ACE5E
// Assembly location: D:\Projects\BarniVann\barnivann\Barnivann\BarnivannAdminUI\bin\GridMvc.dll

using GridMvc.Utility;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GridMvc.DataAnnotations
{
  internal class GridAnnotaionsProvider : IGridAnnotaionsProvider
  {
    public GridColumnAttribute GetAnnotationForColumn<T>(PropertyInfo pi)
    {
      pi = this.GetMetadataProperty<T>(pi);
      GridColumnAttribute attribute = PropertiesHelper.GetAttribute<GridColumnAttribute>(pi);
      if (attribute != null)
        return attribute;
      GridColumnAttribute gridColumnAttribute1 = (GridColumnAttribute) null;
      GridAnnotaionsProvider.DataAnnotationsOptions dataAnnotations = this.ExtractDataAnnotations(pi);
      if (dataAnnotations != null)
      {
        GridColumnAttribute gridColumnAttribute2 = new GridColumnAttribute();
        gridColumnAttribute2.Title = dataAnnotations.DisplayName;
        GridColumnAttribute gridColumnAttribute3 = gridColumnAttribute2;
        bool? filterEnabled = dataAnnotations.FilterEnabled;
        int num = filterEnabled.HasValue ? (filterEnabled.GetValueOrDefault() ? 1 : 0) : 0;
        gridColumnAttribute3.FilterEnabled = num != 0;
        gridColumnAttribute2.Format = dataAnnotations.Format;
        gridColumnAttribute1 = gridColumnAttribute2;
      }
      return gridColumnAttribute1;
    }

    public GridHiddenColumnAttribute GetAnnotationForHiddenColumn<T>(PropertyInfo pi)
    {
      pi = this.GetMetadataProperty<T>(pi);
      GridHiddenColumnAttribute attribute = PropertiesHelper.GetAttribute<GridHiddenColumnAttribute>(pi);
      if (attribute != null)
        return attribute;
      GridHiddenColumnAttribute hiddenColumnAttribute = (GridHiddenColumnAttribute) null;
      GridAnnotaionsProvider.DataAnnotationsOptions dataAnnotations = this.ExtractDataAnnotations(pi);
      if (dataAnnotations != null)
        hiddenColumnAttribute = new GridHiddenColumnAttribute()
        {
          Format = dataAnnotations.Format
        };
      return hiddenColumnAttribute;
    }

    public bool IsColumnMapped(PropertyInfo pi)
    {
      return PropertiesHelper.GetAttribute<NotMappedColumnAttribute>(pi) == null;
    }

    public GridTableAttribute GetAnnotationForTable<T>()
    {
      MetadataTypeAttribute attribute1 = PropertiesHelper.GetAttribute<MetadataTypeAttribute>(typeof (T));
      if (attribute1 != null)
      {
        GridTableAttribute attribute2 = PropertiesHelper.GetAttribute<GridTableAttribute>(attribute1.MetadataClassType);
        if (attribute2 != null)
          return attribute2;
      }
      return PropertiesHelper.GetAttribute<GridTableAttribute>(typeof (T));
    }

    private PropertyInfo GetMetadataProperty<T>(PropertyInfo pi)
    {
      MetadataTypeAttribute attribute = PropertiesHelper.GetAttribute<MetadataTypeAttribute>(typeof (T));
      if (attribute != null)
      {
        PropertyInfo property = attribute.MetadataClassType.GetProperty(pi.Name);
        if (property != (PropertyInfo) null)
          return property;
      }
      return pi;
    }

    private GridAnnotaionsProvider.DataAnnotationsOptions ExtractDataAnnotations(PropertyInfo pi)
    {
      GridAnnotaionsProvider.DataAnnotationsOptions annotationsOptions = (GridAnnotaionsProvider.DataAnnotationsOptions) null;
      DisplayAttribute attribute1 = PropertiesHelper.GetAttribute<DisplayAttribute>(pi);
      if (attribute1 != null)
      {
        annotationsOptions = new GridAnnotaionsProvider.DataAnnotationsOptions();
        annotationsOptions.DisplayName = attribute1.Name;
        annotationsOptions.FilterEnabled = attribute1.GetAutoGenerateFilter();
      }
      DisplayFormatAttribute attribute2 = PropertiesHelper.GetAttribute<DisplayFormatAttribute>(pi);
      if (attribute2 != null)
      {
        if (annotationsOptions == null)
          annotationsOptions = new GridAnnotaionsProvider.DataAnnotationsOptions();
        annotationsOptions.Format = attribute2.DataFormatString;
      }
      return annotationsOptions;
    }

    private class DataAnnotationsOptions
    {
      public string Format { get; set; }

      public string DisplayName { get; set; }

      public bool? FilterEnabled { get; set; }

      public int Order { get; set; }
    }
  }
}
