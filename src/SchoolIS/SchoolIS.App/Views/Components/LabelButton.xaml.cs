using System.Windows.Input;

namespace SchoolIS.App.Views.Components;

public partial class LabelButton : Label {
  public LabelButton() {
    InitializeComponent();
  }

  public static readonly BindableProperty CommandProperty =
    BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(LabelButton), null);

  public static readonly BindableProperty CommandParameterProperty =
    BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(LabelButton), null);

  public ICommand Command {
    get => (ICommand)GetValue(CommandProperty);
    set => SetValue(CommandProperty, value);
  }

  public object CommandParameter {
    get => GetValue(CommandParameterProperty);
    set => SetValue(CommandParameterProperty, value);
  }
}