using Microsoft.Extensions.DependencyInjection;
using Trimex.Pages;

namespace Trimex
{
    public partial class App : Application
    {
        private readonly IServiceProvider _services;
        private Window? _window;

        public Task InitializationTask { get; private set; } = Task.CompletedTask;

        public App(IServiceProvider services, Services.IDatabaseInitializer databaseInitializer)
        {
            InitializeComponent();
            _services = services;

            UserAppTheme = AppTheme.Dark;

            InitializationTask = InitializeDatabaseAsync(databaseInitializer);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            _window = new Window(new SplashPage());
            return _window;
        }

        public void NavigateToMainApp()
        {
            if (_window is not null)
            {
                _window.Page = _services.GetRequiredService<AppShell>();
            }
        }

        private static async Task InitializeDatabaseAsync(Services.IDatabaseInitializer databaseInitializer)
        {
            try
            {
                await databaseInitializer.InitializeAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization failed: {ex}");
            }
        }
    }
}
