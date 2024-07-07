using Microsoft.Extensions.Logging;

namespace RangeApp;

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

		string dbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "rangeData.db3");
		builder.Services.AddSingleton<Models.RangeDayRepository>(s => ActivatorUtilities.CreateInstance<Models.RangeDayRepository>(s, dbPath));

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
