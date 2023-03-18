using CommunityToolkit.Maui;
using Inverter.Display.Views;
using Inverter.Display.ViewsModel;
using Inverter.GenerateInverter.ViewsModel;
using Inverter.Helpers;

namespace Inverter;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<Config>();


        builder.Services.AddTransient<DisplayVM>();
        builder.Services.AddTransient<DisplayV>();

        builder.Services.AddTransient<GenerateMV>();
        builder.Services.AddTransient<GenerateInverter.Views.InverterV>();

        var app = builder.Build();

        return app;

    }
}
