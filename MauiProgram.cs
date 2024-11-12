using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace TOTools;

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
        var graphQLClient = new GraphQLHttpClient(
            "https://api.start.gg/gql/alpha", 
            new NewtonsoftJsonSerializer());
        graphQLClient.HttpClient.DefaultRequestHeaders.Add(
            "Authorization", 
            "Bearer dd3f05cd4cc1496d28bb2a406f96a4d0");
        builder.Services
            .AddSingleton(graphQLClient);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}