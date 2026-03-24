namespace Trimex.Pages;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(300);
        await TitleLabel.FadeTo(1, 800, Easing.CubicIn);
        await SubtitleLabel.FadeTo(1, 600, Easing.CubicIn);
        await Task.Delay(1500);

        if (Application.Current is App app)
        {
            app.NavigateToMainApp();
        }
    }
}
