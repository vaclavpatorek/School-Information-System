using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SchoolIS.BL.Models;
using SkiaSharp;

namespace SchoolIS.App.Views.Components;

public partial class PieChart : ContentView {
  private const int MaxNumOfSegments = 5;
  private uint _groupCount = 0;

  private static readonly SKColor[] Colors = [
    SKColor.Parse("#FFFFFF"),
    SKColor.Parse("#D7CCEE"),
    SKColor.Parse("#AD87FF"),
    SKColor.Parse("#804CFF"),
    SKColor.Parse("#4926B7")
  ];

  public PieChart() {
    InitializeComponent();

    Chart.LegendTextPaint = new SolidColorPaint(SKColors.White);
  }

  public static readonly BindableProperty EvaluationsProperty =
    BindableProperty.Create(nameof(Evaluations), typeof(IEnumerable<EvaluationListModel>),
      typeof(PieChart),
      propertyChanged: (bindable, _, _) => ((PieChart)bindable).ChangeData());

  public IEnumerable<EvaluationListModel> Evaluations {
    get => (IEnumerable<EvaluationListModel>)GetValue(EvaluationsProperty);
    set => SetValue(EvaluationsProperty, value);
  }

  private void ChangeData() {
    uint maxPoints = Evaluations.MaxBy(e => e.Points)?.Points ?? 1;

    if (maxPoints == 0)
      return;

    _groupCount = Math.Min(MaxNumOfSegments, maxPoints + 1);
    double groupRange = maxPoints / (double)_groupCount;

    IEnumerable<IGrouping<double, EvaluationListModel>> groups =
      Evaluations.GroupBy(s => Math.Floor(s.Points / groupRange)).ToList();

    if (!groups.Any()) return;

    List<uint> arr = Enumerable.Repeat<uint>(0, (int)_groupCount).ToList();

    foreach (var group in groups) {
      arr[Math.Min((int)group.Key, (int)_groupCount - 1)] = (uint)group.Count();
    }

    int cnt = 0;
    Series = [
      ..arr.Select(i => new PieSeries<double> {
        Values = new[] { (double)i },
        Fill = new SolidColorPaint(Colors[cnt]),
        Name = RangeFormatter(groupRange, ++cnt),
        Stroke = new SolidColorPaint(SKColors.White)
      }).Where(e => (int?)(e.Values?.FirstOrDefault()) != 0)
    ];
  }

  private string RangeFormatter(double range, int index) {
    int low = (int)Math.Ceiling(range * (index - 1));

    int high = (int)Math.Ceiling(range * index - 1);
  
    // Increment to include maximum points option
    if (_groupCount == index) high++;

    return low < high ? $"{low}-{high}" : $"{low}";
  }

  public ISeries[] Series { get; set; }
    = [new PieSeries<double> { Values = new[] { (double)1 }, Name = "0" }];
}