using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;

namespace TOTools;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Hide the status bar to achieve fullscreen
        var window = Window;
        if (window == null)
        {
            return;
        }
        
        WindowCompat.SetDecorFitsSystemWindows(window, false);
        var controller = new WindowInsetsControllerCompat(window, window.DecorView);
        controller.Hide(
            WindowInsetsCompat.Type.StatusBars() | 
            WindowInsetsCompat.Type.CaptionBar() | 
            WindowInsetsCompat.Type.NavigationBars() | 
            WindowInsetsCompat.Type.Ime());
        controller.SystemBarsBehavior = WindowInsetsControllerCompat.BehaviorShowTransientBarsBySwipe;
        
        // Ensure fullscreen mode
        window.AddFlags(WindowManagerFlags.Fullscreen);
        window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);
    }
    
}