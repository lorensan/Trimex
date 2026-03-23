using Microsoft.Extensions.Logging;
using Trimex.Data;
using Trimex.Pages;
using Trimex.Services;

namespace Trimex
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<AppDatabase>();
            builder.Services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();
            builder.Services.AddSingleton<IHeroWodRepository, HeroWodRepository>();
            builder.Services.AddSingleton<IWorkoutNoteRepository, WorkoutNoteRepository>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddTransient<AmrapConfigurationPage>();
            builder.Services.AddTransient<ForTimeConfigurationPage>();

#if DEBUG
     		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
