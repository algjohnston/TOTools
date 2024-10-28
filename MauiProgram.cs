using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using StrawberryShake;

namespace CS341Project;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // For interacting with start.gg
        builder.Services
            .AddGraphQLClient(ExecutionStrategy.CacheAndNetwork)
            .ConfigureHttpClient(
                client =>
                    client.BaseAddress = new Uri(
                        "https://api.start.gg/gql/alpha"
                    )
            );

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}