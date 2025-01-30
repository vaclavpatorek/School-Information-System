using System.Globalization;

namespace SchoolIS.App.Views.Components;

public partial class RowBorder : ContentView {
  public static readonly BindableProperty FirstBindingProperty =
    BindableProperty.Create(nameof(FirstBinding), typeof(object), typeof(RowBorder),
      propertyChanged: (bindable, _, _) => ((RowBorder)bindable).PropertiesChanged());

  public static readonly BindableProperty SecondBindingProperty =
    BindableProperty.Create(nameof(SecondBinding), typeof(object), typeof(RowBorder),
      propertyChanged: (bindable, _, _) => ((RowBorder)bindable).PropertiesChanged());

  public static readonly BindableProperty ConverterProperty =
    BindableProperty.Create(nameof(Converter), typeof(IMultiValueConverter), typeof(RowBorder),
      null);

  public static readonly BindableProperty BorderColorProperty =
    BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(RowBorder),
      Colors.Transparent);

  public object? FirstBinding {
    get => GetValue(FirstBindingProperty);
    set => SetValue(FirstBindingProperty, value);
  }

  public object? SecondBinding {
    get => GetValue(SecondBindingProperty);
    set => SetValue(SecondBindingProperty, value);
  }

  public IMultiValueConverter Converter {
    get => (IMultiValueConverter)GetValue(ConverterProperty);
    set => SetValue(ConverterProperty, value);
  }

  public Color BorderColor {
    get => (Color)GetValue(BorderColorProperty);
    set => SetValue(BorderColorProperty, value);
  }

  private void PropertiesChanged() {
    if (FirstBinding != null && SecondBinding != null) 
      BorderColor = (Color)Converter.Convert([FirstBinding, SecondBinding], typeof(Color),
        null, CultureInfo.CurrentCulture);
  }

  public Border InnerBorder => _InnerBorder;

  public RowBorder() {
    InitializeComponent();
  }
}