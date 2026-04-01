using Microsoft.Maui.ApplicationModel;

namespace Trimex.Pages;

public partial class HomePage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public HomePage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;

        ApplicationVersionLabel.Text = $"Version: {AppInfo.Current.VersionString}";
        ApplicationBuildLabel.Text = $"Build: {AppInfo.Current.BuildString}";
        ApplicationAuthorLabel.Text = "Author: Trimex";
        ApplicationLicenseLabel.Text = "License: Not specified";
        ApplicationPackageLabel.Text = $"Package: {AppInfo.Current.PackageName}";
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

    private void OnApplicationInfoTapped(object? sender, TappedEventArgs e)
    {
        ApplicationInfoDimmer.IsVisible = true;
        ApplicationInfoPopup.IsVisible = true;
    }

    private void OnApplicationInfoDimmerTapped(object? sender, TappedEventArgs e)
    {
        ApplicationInfoPopup.IsVisible = false;
        ApplicationInfoDimmer.IsVisible = false;
    }
}
