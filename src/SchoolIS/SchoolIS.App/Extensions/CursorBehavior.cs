
#if WINDOWS
using SchoolIS.App.Platforms.Windows;
#endif

#if MACCATALYST
using SchoolIS.App.Platforms.MacCatalyst;
#endif


using System.Runtime.Versioning;
#if IOS
using SchoolIS.App.Platforms.iOS;
#endif



namespace SchoolIS.App.Extensions;

[SupportedOSPlatform("maccatalyst13.4")]
public class CursorBehavior {
  public static readonly BindableProperty CursorProperty = BindableProperty.CreateAttached("Cursor",
    typeof(CursorIcon), typeof(CursorBehavior), CursorIcon.Arrow, propertyChanged: CursorChanged);

  private static void CursorChanged(BindableObject bindable, object oldvalue, object newvalue) {
    if (bindable is VisualElement visualElement) {
      visualElement.SetCustomCursor((CursorIcon)newvalue,
        Application.Current?.MainPage?.Handler?.MauiContext);
    }
  }

  public static CursorIcon GetCursor(BindableObject view) =>
    (CursorIcon)view.GetValue(CursorProperty);

  public static void SetCursor(BindableObject view, CursorIcon value) =>
    view.SetValue(CursorProperty, value);
}