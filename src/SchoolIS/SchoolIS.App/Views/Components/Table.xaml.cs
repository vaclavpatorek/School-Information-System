using Microsoft.Maui.Controls.Shapes;
using SchoolIS.BL.Models;
using ColumnDefinition = Microsoft.Maui.Controls.ColumnDefinition;

namespace SchoolIS.App.Views.Components;

public partial class Table : ContentView {
  private readonly List<LayoutOptions> _columnAlignments = [];
  private readonly ColumnDefinitionCollection _columnDefinitionCollection = [];

  public Table() {
    InitializeComponent();
  }

  public static readonly BindableProperty ColumnDefinitionsProperty =
    BindableProperty.Create(nameof(ColumnDefinitionsProperty), typeof(string),
      typeof(Table),
      propertyChanged: (bindable, _, _) =>
        OnChanged(bindable, ChangeColumnDefinitions));


  public static readonly BindableProperty ColumnSpacingProperty =
    BindableProperty.Create(nameof(ColumnSpacing), typeof(double), typeof(Table), 0.0,
      propertyChanged: (bindable, _, _) =>
        OnChanged(bindable, ChangeColumnsSpacing));


  public static readonly BindableProperty HeaderLabelsProperty =
    BindableProperty.Create(nameof(HeaderLabels), typeof(string), typeof(Table), String.Empty,
      propertyChanged: (bindable, _, _) =>
        OnChanged(bindable, ChangeHeaderLabels));


  public static readonly BindableProperty ColumnAlignmentProperty =
    BindableProperty.Create(nameof(ColumnAlignment), typeof(string), typeof(Table), String.Empty,
      propertyChanged: (bindable, _, _) =>
        OnChanged(bindable, ChangeColumnAlignment));


  public static readonly BindableProperty ModelListProperty =
    BindableProperty.Create(nameof(ModelList), typeof(IEnumerable<ModelBase>), typeof(Table),
      propertyChanged: (bindable, _, _) =>
        OnChanged(bindable, ChangeCollectionData));

  public static readonly BindableProperty FiltersProperty =
    BindableProperty.Create(nameof(Filters), typeof(object), typeof(Table),
      propertyChanged: (bindable, _, _) =>
        OnChanged(bindable, AddFilters));


  public static readonly BindableProperty ItemTemplateProperty =
    BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(Table));

  public static new readonly BindableProperty HeightRequestProperty =
    BindableProperty.Create(nameof(HeightRequest), typeof(int), typeof(Table), 300);

  public DataTemplate ItemTemplate {
    get { return (DataTemplate)GetValue(ItemTemplateProperty); }
    set { SetValue(ItemTemplateProperty, value); }
  }

  public string ColumnDefinitions {
    get => (string)GetValue(ColumnDefinitionsProperty);
    set => SetValue(ColumnDefinitionsProperty, value);
  }

  public string ColumnAlignment {
    get => (string)GetValue(ColumnAlignmentProperty);
    set => SetValue(ColumnAlignmentProperty, value);
  }

  public string HeaderLabels {
    get => (string)GetValue(HeaderLabelsProperty);
    set => SetValue(HeaderLabelsProperty, value);
  }

  public double ColumnSpacing {
    get => (double)GetValue(ColumnSpacingProperty);
    set => SetValue(ColumnSpacingProperty, value);
  }

  public IEnumerable<ModelBase> ModelList {
    get => (IEnumerable<ModelBase>)GetValue(ModelListProperty);
    set => SetValue(ModelListProperty, value);
  }

  public object Filters {
    get => GetValue(FiltersProperty);
    set => SetValue(FiltersProperty, value);
  }

  public new int HeightRequest {
    get => (int)GetValue(HeightRequestProperty);
    set => SetValue(HeightRequestProperty, value);
  }
  

  static void OnChanged(BindableObject bindable,
    Action<Table> updateTable) {
    if (bindable is Table table) {
      updateTable.Invoke(table);
    }
  }

  private static void ChangeColumnDefinitions(Table table) {
    table.GetColumnDefinitionCollection();
    table.Header.ColumnDefinitions = table._columnDefinitionCollection;
  }

  private void GetColumnDefinitionCollection() {
    foreach (var columnDefinition in ColumnDefinitions.Split(",")) {
      ColumnDefinition column;

      if (columnDefinition.Trim().ToLower() == "auto") {
        column = new ColumnDefinition {
          Width = GridLength.Auto
        };
      } else if (columnDefinition.Trim() == "*") {
        column = new ColumnDefinition {
          Width = new GridLength(1, GridUnitType.Star)
        };
      } else {
        if (int.TryParse(columnDefinition, out int width)) {
          column = new ColumnDefinition {
            Width = new GridLength(width, GridUnitType.Absolute)
          };
        } else {
          // Handle invalid column definition here
          throw new ArgumentException("Invalid Table column definition: " + columnDefinition);
        }
      }

      _columnDefinitionCollection.Add(column);
    }
  }

  private static void ChangeColumnAlignment(Table table) {
    foreach (var alignment in table.ColumnAlignment.Split(',')) {
      string layout = alignment.Trim().ToLower();

      switch (layout) {
        case "start" or "":
          table._columnAlignments.Add(LayoutOptions.Start);
          break;

        case "end":
          table._columnAlignments.Add(LayoutOptions.End);
          break;

        case "center":
          table._columnAlignments.Add(LayoutOptions.Center);
          break;

        default:
          throw new ArgumentException($"Invalid Table column layout `{alignment}`");
      }
    }

    for (var i = 0; i < table.Header.Children.Count; i++) {
      Label label = (Label)table.Header.Children[i];
      label.HorizontalOptions = table._columnAlignments.ElementAtOrDefault(i);
    }
  }

  private static void ChangeHeaderLabels(Table table) {
    int columnIdx = 0;
    foreach (string s in table.HeaderLabels.Split(',')) {
      Label headerLabel = new() {
        Text = s.Trim(),
        HorizontalOptions = table._columnAlignments.ElementAtOrDefault(columnIdx),
        Style = (Style)table.Resources["HeaderStyle"]
      };

      table.Header.Add(headerLabel, column: columnIdx++);
    }
  }

  private static void ChangeColumnsSpacing(Table table) {
    table.Header.ColumnSpacing = table.ColumnSpacing;
  }

  static T Inv<T>(Func<T> f) => f();

  private static void ChangeCollectionData(Table table) {
    table.CollectionData.ItemTemplate = new DataTemplate(() => {
      var views = Inv(() => {
        var content = table.ItemTemplate.CreateContent();

        if (content is Layout) {
          var layout = content as Layout;
          // We need to remove the childs from the layout, so we can add them to the table.
          var children = layout!.Children.Select(c => (View)c).ToList();
          foreach (var child in children) {
            layout.Remove(child);
          }

          return children;
        }

        return (IEnumerable<View>)content;
      });

      // Create the Grid element
      var grid = new Grid {
        RowDefinitions = [
          new RowDefinition { Height = GridLength.Star },
          new RowDefinition { Height = GridLength.Auto }
        ]
      };

      Grid dataRow = new() {
        ColumnDefinitions = table._columnDefinitionCollection,
        ColumnSpacing = table.ColumnSpacing,
        Style = (Style)table.Resources["ColumnsLayout"]
      };

      RowBorder? rowBorder = null;

      int columnIdx = 0;
      foreach (var view in views) {
        view.VerticalOptions = LayoutOptions.Center;
        view.HorizontalOptions = table._columnAlignments.ElementAtOrDefault(columnIdx);

        if (view is RowBorder border) {
          rowBorder = border;
        } else {
          dataRow.Add(view, column: columnIdx++);
        }
      }

      // Add innerGrid and stackLayout to the main Grid

      if (rowBorder != null) {
        rowBorder.InnerBorder.Content = dataRow;
#pragma warning disable CS0618 // Type or member is obsolete
        rowBorder.HorizontalOptions = LayoutOptions.FillAndExpand;
#pragma warning restore CS0618 // Type or member is obsolete

        grid.Add(rowBorder, row: 0);
      } else {
        grid.Add(dataRow, row: 0);
      }

      var boxView = new BoxView {
        Style = (Style)table.Resources["DataDivider"]
      };
      grid.Add(boxView, row: 1);

      return grid;
    });
  }

  private static void AddFilters(Table table) {
    IEnumerable<View> filters = Inv(() => {
      if (table.Filters is Layout) {
        var layout = table.Filters as Layout;
        var children = layout!.Children.Select(c => (View)c).ToList();
        foreach (var child in children)
          layout.Remove(child);
        return children;
      }

      return (IEnumerable<View>)table.Filters;
    });

    int columnIdx = 0;
    table.FiltersRow.ColumnDefinitions = table._columnDefinitionCollection;
    table.FiltersRow.ColumnSpacing = table.ColumnSpacing;

    foreach (var filter in filters) {
      filter.HorizontalOptions = table._columnAlignments[columnIdx];
      table.FiltersRow.Add(filter, column: columnIdx++);
    }
  }
}