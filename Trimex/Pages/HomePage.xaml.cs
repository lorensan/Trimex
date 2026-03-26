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

    private async void OnEmomClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(_serviceProvider.GetRequiredService<EmomConfigurationPage>());
    }

    private async void OnTabataClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(_serviceProvider.GetRequiredService<TabataConfigurationPage>());
    }

    private async void OnHeroWodsClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(_serviceProvider.GetRequiredService<HeroWodsPage>());
    }

}
