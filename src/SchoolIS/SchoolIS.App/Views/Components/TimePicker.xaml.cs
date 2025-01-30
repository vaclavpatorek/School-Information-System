namespace SchoolIS.App.Views.Components;

public partial class TimePicker : ContentView {
  public TimePicker() {
    InitializeComponent();
  }

  public static readonly BindableProperty FontSizeProperty =
    BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TimePicker), 13.0);

  public static readonly BindableProperty DateProperty =
    BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(TimePicker), DateTime.Today,
      propertyChanged: (bindable, _, _) => ((TimePicker)bindable).PropDateChanged());

  private void PropDateChanged() {
    HourTextField.Text = DateHour;
    MinuteTextField.Text = DateMinute;
  }

  public DateTime Date {
    get => (DateTime)GetValue(DateProperty);
    set => SetValue(DateProperty, value);
  }

  public double FontSize {
    get => (double)GetValue(FontSizeProperty); 
    set => SetValue(FontSizeProperty, value);
  }

  public string DateHour {
    get => $"{Date:HH}";
    set {
      if (int.TryParse(value, out var hour)) {
        Date = Date.AddHours(-Date.Hour + hour);
      }
    }
  }

  public string DateMinute {
    get => $"{Date:mm}";
    set {
      if (int.TryParse(value, out var minute)) {
        Date = Date.AddMinutes(-Date.Minute + minute);
      }
    }
  }
}