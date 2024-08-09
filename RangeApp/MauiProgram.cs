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
		builder.Services.AddSingleton<Models.LocationRepository>(s => ActivatorUtilities.CreateInstance<Models.LocationRepository>(s, dbPath));
		builder.Services.AddSingleton<Models.SessionRepository>(s => ActivatorUtilities.CreateInstance<Models.SessionRepository>(s, dbPath));
		builder.Services.AddSingleton<Models.RoundRepository>(s => ActivatorUtilities.CreateInstance<Models.RoundRepository>(s, dbPath));

		//Page Routes
		Routing.RegisterRoute("NewGroupPage", typeof(Views.NewGroupPage));
		Routing.RegisterRoute("SessionPage", typeof(Views.SessionPage));
		Routing.RegisterRoute("NewFirearmPage", typeof(Views.NewFirearmPage));
		Routing.RegisterRoute("NewRoundPage", typeof(Views.NewRoundPage));
		Routing.RegisterRoute("NewPowderPage", typeof(Views.NewPowderPage));
		Routing.RegisterRoute("NewBulletPage", typeof(Views.NewBulletPage));
		Routing.RegisterRoute("NewFirearmPage", typeof(Views.NewFirearmPage));
		Routing.RegisterRoute("RoundListPage", typeof(Views.RoundListPage));

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
