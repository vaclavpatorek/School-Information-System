using System.Globalization;

namespace SchoolIS.App.Views.Components;

public partial class AppBar : ContentView {
  private readonly Timer _timer;
  private DateTime? _currentDate;

  public AppBar() {
    InitializeComponent();
    _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
  }


  private void TimerCallback(object? state) {
    MainThread.BeginInvokeOnMainThread(() => {
      // Update current time
      Time.Text = DateTime.Now.ToLocalTime().ToShortTimeString();
      
      // Update current date
      if (_currentDate != DateTime.Now.Date) {
        UpdateDate();
      }

      // Reset timer
      _timer.Change(TimeSpan.FromSeconds(1), Timeout.InfiniteTimeSpan);
    });
  }

  private void UpdateDate() {
    _currentDate = DateTime.Now.Date;

    string month = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
    string day = DateTime.Now.Day.ToString();
    string suffix = "th";

    if (day.EndsWith('1') && day != "11") suffix = "st";
    else if (day.EndsWith('2') && day != "12") suffix = "nd";
    else if (day.EndsWith('3') && day != "13") suffix = "rd";

    Date.Text = $"{month} {day}{suffix}";
  }
}