using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SchoolIS.BL.Models;
using SkiaSharp;

namespace SchoolIS.App.Views.Components;

public partial class BarChart : ContentView {
  public BarChart() {
    InitializeComponent();
  }

  public static readonly BindableProperty HasSubjectDataProperty =
    BindableProperty.Create(nameof(HasSubjectData), typeof(IEnumerable<HasSubjectDetailModel>),
      typeof(BarChart),
      propertyChanged: (bindable, _, _) => ((BarChart)bindable).ChangeData());

  public IEnumerable<HasSubjectDetailModel> HasSubjectData {
    get => (IEnumerable<HasSubjectDetailModel>)GetValue(HasSubjectDataProperty);
    set => SetValue(HasSubjectDataProperty, value);
  }

  private void ChangeData() {
    IEnumerable<IGrouping<int, HasSubjectDetailModel>> groupBy =
      HasSubjectData.GroupBy(s => s.TotalPoints / 10).ToList();

    if (!groupBy.Any()) return;

    List<int> arr = Enumerable.Repeat(0, 10).ToList();

    foreach (var group in groupBy) {
      arr[Math.Clamp(group.Key, 0, 9)] = group.Count();
    }

    Series = [
      new RowSeries<int> {
        Values = arr,
        // Defines the max width a bar can have
        MaxBarWidth = double.PositiveInfinity,
        Fill = new SolidColorPaint(GetColor("#96A5BD")),
        Padding = 5,
        YToolTipLabelFormatter = point => $"{point.Model}",
      }
    ];
  }

  public Axis[] XAxes { get; set; } = [
    new Axis {
      Name = "Student Count",
      NamePaint = new SolidColorPaint(SKColors.White),
      LabelsPaint = new SolidColorPaint(GetColor("#676566")),
      TextSize = 14,
      SeparatorsPaint = new SolidColorPaint(GetColor("#676566")),
      MinLimit = 0.0,
      MinStep = 1.0,
    }
  ];


  public Axis[] YAxes { get; set; } = [
    new Axis {
      Name = "Points",
      NamePaint = new SolidColorPaint(SKColors.White),
      Padding = new Padding(0, 0, 10, 0),
      LabelsPaint = new SolidColorPaint(GetColor("#676566")),
      TextSize = 12,
      SeparatorsPaint = new SolidColorPaint(SKColors.Transparent),
      Labels = new List<string>
        { "0-9", "10-19", "20-29", "30-39", "40-49", "50-59", "60-69", "70-79", "80-89", "90-100" },
    }
  ];

  public ISeries[] Series { get; set; } = [
    new RowSeries<int> {
      Values = Enumerable.Repeat(0, 10),
      MaxBarWidth = double.PositiveInfinity,
      Fill = new SolidColorPaint(GetColor("#96A5BD")),
      Padding = 5
    }
  ];

  public static SKColor GetColor(string hex) {
    return SKColor.TryParse(hex, out SKColor color) ? color : SKColors.Red;
  }
}