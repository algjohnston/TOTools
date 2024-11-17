using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace TOTools;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Hide the status bar to achieve fullscreen
        Window.DecorView.SystemUiVisibility = (StatusBarVisibility)
            (SystemUiFlags.LayoutStable | SystemUiFlags.LayoutFullscreen | SystemUiFlags.HideNavigation);

        // Ensure fullscreen mode
        Window.AddFlags(WindowManagerFlags.Fullscreen);
        Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);
    }
}