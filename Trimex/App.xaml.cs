namespace Trimex
{
    public partial class App : Application
    {
        private readonly AppShell _appShell;

        public App(AppShell appShell, Services.IDatabaseInitializer databaseInitializer)
        {
            InitializeComponent();
            _appShell = appShell;

            databaseInitializer.InitializeAsync().GetAwaiter().GetResult();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_appShell);
        }
    }
}
