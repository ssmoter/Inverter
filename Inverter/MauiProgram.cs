using Inverter.Display.Views;
using Inverter.Display.ViewsModel;

namespace Inverter;

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


        builder.Services.AddSingleton<DisplayVM>();

        builder.Services.AddSingleton<DisplayV>();

		var app = builder.Build();

		return app;

    }
}
