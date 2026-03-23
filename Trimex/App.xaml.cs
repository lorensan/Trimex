using Microsoft.Extensions.DependencyInjection;

namespace Trimex
{
    public partial class App : Application
    {
        private readonly IServiceProvider _services;

        public App(IServiceProvider services, Services.IDatabaseInitializer databaseInitializer)
        {
            InitializeComponent();
            _services = services;

            _ = InitializeDatabaseAsync(databaseInitializer);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var appShell = _services.GetRequiredService<AppShell>();
            return new Window(appShell);
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
