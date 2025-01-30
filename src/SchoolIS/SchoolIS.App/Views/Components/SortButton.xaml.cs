using SchoolIS.App.Models;
using System.Windows.Input;

namespace SchoolIS.App.Views.Components;

public partial class SortButton : ContentView {
  public SortButton() {
    InitializeComponent();

    OnClickCommand = new Command(OnClick);
  }

  public static readonly BindableProperty TextSortAscProperty =
    BindableProperty.Create(nameof(TextSortAsc), typeof(string), typeof(SortButton), FaSolidIcons.ArrowUpAZ);

  public static readonly BindableProperty TextSortDescProperty =
    BindableProperty.Create(nameof(TextSortDesc), typeof(string), typeof(SortButton), FaSolidIcons.ArrowDownAZ);

  public static readonly BindableProperty SortAscProperty =
    BindableProperty.Create(nameof(SortAsc), typeof(bool), typeof(SortButton), true);

  public static readonly BindableProperty CommandProperty =
    BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SortButton), null);

  public static readonly BindableProperty CommandParameterProperty =
    BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(SortButton), null);

  public string TextSortAsc {
    get => (string)GetValue(TextSortAscProperty);
    set => SetValue(TextSortAscProperty, value);
  }

  public string TextSortDesc {
    get => (string)GetValue(TextSortDescProperty);
    set => SetValue(TextSortDescProperty, value);
  }

  public bool SortAsc {
    get => (bool)GetValue(SortAscProperty);
    set => SetValue(SortAscProperty, value);
  }

  public ICommand Command {
    get => (ICommand)GetValue(CommandProperty);
    set => SetValue(CommandProperty, value);
  }

  public object CommandParameter {
    get => GetValue(CommandParameterProperty);
    set => SetValue(CommandParameterProperty, value);
  }

  public ICommand OnClickCommand { get; set; }

  public void OnClick(object param) {
    SortAsc = !SortAsc;
    Command?.Execute(CommandParameter);
  }
}