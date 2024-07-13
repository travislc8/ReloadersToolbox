using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace RangeApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp
			.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		string dbPath = System.IO.Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "rangeData.db3");

		builder.Services.AddSingleton<Models.RangeDayRepository>(s => ActivatorUtilities.CreateInstance<Models.RangeDayRepository>(s, dbPath));
		builder.Services.AddSingleton<Models.FirearmRepository>(s => ActivatorUtilities.CreateInstance<Models.FirearmRepository>(s, dbPath));

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
