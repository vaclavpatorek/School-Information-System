using System.Text.RegularExpressions;
using System.Windows.Input;
using zoft.MauiExtensions.Core.Extensions;

namespace SchoolIS.App.Views.Components;

public partial class Textfield : ContentView {
  public Textfield() {
    InitializeComponent();
  }

  public static readonly BindableProperty PlaceholderProperty =
    BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(Textfield), string.Empty);

  public static readonly BindableProperty TextProperty =
    BindableProperty.Create(nameof(Text), typeof(string), typeof(Textfield), string.Empty,
      BindingMode.TwoWay);

  public static readonly BindableProperty TextChangeCommandProperty =
    BindableProperty.Create(nameof(TextChangeCommand), typeof(ICommand), typeof(Textfield),
      propertyChanged: (bindable, _, _) => ((Textfield)bindable).PropertyTextChanged());

  private void PropertyTextChanged() {
    TextField.TextChanged += (_, e) => TextChangeCommand.Execute(e.NewTextValue);
  }

  public static readonly BindableProperty SpaceAllowedProperty =
    BindableProperty.Create(nameof(SpaceAllowed), typeof(bool), typeof(Textfield), true);

  public static readonly BindableProperty NumbersOnlyProperty =
    BindableProperty.Create(nameof(NumbersOnly), typeof(bool), typeof(Textfield), false);
  
  public new static readonly BindableProperty HeightRequestProperty =
    BindableProperty.Create(nameof(HeightRequest), typeof(double), typeof(Textfield), 30.0);

  public new static readonly BindableProperty BackgroundColorProperty =
    BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(Textfield),
      Color.FromArgb("#202020"));

  public static readonly BindableProperty TextColorProperty =
    BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Textfield), Colors.White);

  public static readonly BindableProperty PlaceholderColorProperty =
    BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(Textfield),
      App.StaticResource("PlaceholderColor"));

  public static readonly BindableProperty FontSizeProperty =
    BindableProperty.Create(nameof(FontSize), typeof(double), typeof(Textfield), 14.0);

  public static readonly BindableProperty CornerRadiusProperty =
    BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(Textfield), 10);

  public static readonly BindableProperty MinValueProperty =
    BindableProperty.Create(nameof(MinValue), typeof(int), typeof(Textfield), int.MinValue);

  public static readonly BindableProperty MaxValueProperty =
    BindableProperty.Create(nameof(MaxValue), typeof(int), typeof(Textfield), int.MaxValue);
  public static readonly BindableProperty BorderColorProperty =
    BindableProperty.Create(nameof(BorderColor), typeof(Brush), typeof(Textfield),
      Brush.Transparent);


  public static readonly BindableProperty BorderWidthProperty =
    BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(Textfield), 0.0);

  public static readonly BindableProperty IsPasswordProperty =
    BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(Textfield), false);

  public static readonly BindableProperty OnCompletedCommandProperty =
    BindableProperty.Create(nameof(OnCompletedCommand), typeof(ICommand), typeof(Textfield),
      propertyChanged: (bindable, _, _) => ((Textfield)bindable).CompletedChanged());
 
  public static readonly BindableProperty OnUnFocusCommandProperty =
    BindableProperty.Create(nameof(OnUnFocusCommand), typeof(ICommand), typeof(Textfield),
      propertyChanged: (bindable, _, _) => ((Textfield)bindable).UnFocusChanged());

  public static readonly BindableProperty EmptyValueProperty =
    BindableProperty.Create(nameof(EmptyValue), typeof(string), typeof(Textfield));


  public ICommand OnCompletedCommand {
    get { return (ICommand)GetValue(OnCompletedCommandProperty); }
    set { SetValue(OnCompletedCommandProperty, value); }
  }

  public ICommand OnUnFocusCommand {
    get { return (ICommand)GetValue(OnUnFocusCommandProperty); }
    set { SetValue(OnUnFocusCommandProperty, value); }
  }

  private void UnFocusChanged() {
    TextField.Unfocused += (_, _) => OnUnFocusCommand.Execute(Text);
  }
  
  private void CompletedChanged() {
    TextField.Completed += (_, _) => OnCompletedCommand.Execute(Text);
  }

  public ICommand TextChangeCommand {
    get { return (ICommand)GetValue(TextChangeCommandProperty); }
    set { SetValue(TextChangeCommandProperty, value); }
  }

  private string CheckString(string s) {
    if (!SpaceAllowed)
      s = s.Trim();
    if (NumbersOnly) {
      s = MatchNonNumbers().Replace(s, "");

      if (int.TryParse(s, out int n)) {
        s = Math.Clamp(n, MinValue, MaxValue).ToString();
      }
    }

    if (s.IsNullOrEmpty())
      s = EmptyValue;

    return s;
  }

  public string Text {
    get => (string)GetValue(TextProperty);
    set => SetValue(TextProperty, CheckString(value));
  }

  public string Placeholder {
    get => (string)GetValue(PlaceholderProperty);
    set => SetValue(PlaceholderProperty, value);
  }

  public bool SpaceAllowed {
    get => (bool)GetValue(SpaceAllowedProperty);
    set => SetValue(SpaceAllowedProperty, value);
  }

  public bool NumbersOnly {
    get => (bool)GetValue(NumbersOnlyProperty);
    set => SetValue(NumbersOnlyProperty, value);
  }

  public new double HeightRequest {
    get => (double)GetValue(HeightRequestProperty);
    set => SetValue(HeightRequestProperty, value);
  }

  public new Color BackgroundColor {
    get => (Color)GetValue(BackgroundColorProperty);
    set => SetValue(BackgroundColorProperty, value);
  }

  public Color TextColor {
    get => (Color)GetValue(TextColorProperty);
    set => SetValue(TextColorProperty, value);
  }

  public Color PlaceholderColor {
    get => (Color)GetValue(PlaceholderColorProperty);
    set => SetValue(PlaceholderColorProperty, value);
  }

  public double FontSize {
    get => (double)GetValue(FontSizeProperty);
    set => SetValue(FontSizeProperty, value);
  }

  public int CornerRadius {
    get => (int)GetValue(CornerRadiusProperty);
    set => SetValue(CornerRadiusProperty, value);
  }

  public int MinValue {
    get => (int)GetValue(MinValueProperty);
    set => SetValue(MaxValueProperty, value);
  }

  public int MaxValue {
    get => (int)GetValue(MaxValueProperty);
    set => SetValue(MaxValueProperty, value);
  }

  [GeneratedRegex("[^0-9]")]
  private static partial Regex MatchNonNumbers();

  public Brush BorderColor {
    get => (Brush)GetValue(BorderColorProperty);
    set => SetValue(BorderColorProperty, value);
  }

  public double BorderWidth {
    get => (double)GetValue(BorderWidthProperty);
    set => SetValue(BorderWidthProperty, value);
      
  }

  public bool IsPassword {
    get => (bool)GetValue(IsPasswordProperty);
    set => SetValue(IsPasswordProperty, value);
  }

  public string EmptyValue {
    get => (string)GetValue(EmptyValueProperty);
    set => SetValue(EmptyValueProperty, value);
  }
}