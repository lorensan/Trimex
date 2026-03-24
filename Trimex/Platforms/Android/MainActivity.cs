using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Trimex
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Window is not null)
            {
                Window.SetStatusBarColor(Android.Graphics.Color.Black);
                Window.SetNavigationBarColor(Android.Graphics.Color.Black);

                // Ensure light icons (white) on dark status bar
                if (OperatingSystem.IsAndroidVersionAtLeast(30))
                {
                    Window.InsetsController?.SetSystemBarsAppearance(0,
                        (int)WindowInsetsControllerAppearance.LightStatusBars |
                        (int)WindowInsetsControllerAppearance.LightNavigationBars);
                }
                else
                {
#pragma warning disable CA1422 // Validate platform compatibility
                    Window.DecorView.SystemUiFlags &= ~(SystemUiFlags.LightStatusBar | SystemUiFlags.LightNavigationBar);
#pragma warning restore CA1422
                }
            }
        }
    }
}
