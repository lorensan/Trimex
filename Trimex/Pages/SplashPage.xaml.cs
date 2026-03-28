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

        //await Task.Delay(300);
        //await Task.Delay(1500);

        if (Application.Current is App app)
        {
            app.NavigateToMainApp();
        }
    }
}
