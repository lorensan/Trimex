namespace Trimex.Pages;

public partial class HomePage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public HomePage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    private async void OnAmrapClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(_serviceProvider.GetRequiredService<AmrapConfigurationPage>());
    }

    private async void OnForTimeClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(_serviceProvider.GetRequiredService<ForTimeConfigurationPage>());
    }

    private async void OnPendingClicked(object? sender, EventArgs e)
    {
        await DisplayAlertAsync("Coming next", "This section is prepared in the home screen, but the workout flow is still pending in the spec.", "OK");
    }

}
