using CommunityToolkit.Maui;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using TOTools.Database;
using TOTools.EventMap;
using TOTools.Scheduler;
using TOTools.Seeding;

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
                fonts.AddFont("Gobold-Bold.ttf", "GoboldBold");
                fonts.AddFont("Raleway-Regular.ttf", "RalewayRegular");
            })
            // Register all the singletons with the dependency injection service
            .RegisterGraphQLClient()
            .RegisterDatabases()
            .RegisterBusinessLogic()
            .UseMauiCommunityToolkit();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }

    private static MauiAppBuilder RegisterGraphQLClient(this MauiAppBuilder builder)
    {
        // For interacting with start.gg
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add(
            "Authorization",
            "Bearer dd3f05cd4cc1496d28bb2a406f96a4d0");
        var graphQLClient = new GraphQLHttpClient(
            "https://api.start.gg/gql/alpha",
            new NewtonsoftJsonSerializer(),
            httpClient: client);

        builder.Services
            .AddSingleton(graphQLClient);
        return builder;
    }

    private static MauiAppBuilder RegisterDatabases(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<PlayerTable>()
            .AddSingleton<MatchTable>()
            .AddSingleton<EventTable>();
        return builder;
    }

    private static MauiAppBuilder RegisterBusinessLogic(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<EventBusinessLogic>()
            .AddSingleton<SchedulerBusinessLogic>()
            .AddSingleton<SeedingBusinessLogic>();
        return builder;
    }
}