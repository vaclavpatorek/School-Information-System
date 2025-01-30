using System.Windows.Input;

namespace SchoolIS.App.Views.Components;

public partial class BlackButton : ContentView {
  public BlackButton() {
    InitializeComponent();
  }

  public static readonly BindableProperty FontSizeProperty =
    BindableProperty.Create(nameof(FontSize), typeof(double), typeof(BlackButton), 18.0);

  public static readonly BindableProperty TextProperty =
    BindableProperty.Create(nameof(Text), typeof(string), typeof(BlackButton), string.Empty);

  public static readonly BindableProperty CommandProperty =
    BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(BlackButton));

  public static readonly BindableProperty CommandParameterProperty =
    BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(BlackButton));

  public static readonly BindableProperty TextMarginProperty =
    BindableProperty.Create(nameof(TextMargin), typeof(Thickness), typeof(BlackButton), new Thickness(0, 8));

  public double FontSize {
    get => (double)GetValue(FontSizeProperty);
    set => SetValue(FontSizeProperty, value);
  }

  public string Text {
    get => (string)GetValue(TextProperty);
    set => SetValue(TextProperty, value);
  }

  public ICommand Command {
    get => (ICommand)GetValue(CommandProperty);
    set => SetValue(CommandProperty, value);
  }

  public object CommandParameter {
    get => GetValue(CommandParameterProperty);
    set => SetValue(CommandParameterProperty, value);
  }

  public Thickness TextMargin {
    get => (Thickness)GetValue(TextMarginProperty);
    set => SetValue(TextMarginProperty, value);
  }
}