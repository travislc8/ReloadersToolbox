namespace RangeApp;

public partial class App : Application
{
    public static Models.FirearmRepository FirearmRepo{ get; private set; }
    public static Models.LocationRepository LocationRepo{ get; private set; }
    public static Models.SessionRepository SessionRepo{ get; private set; }
	public App(Models.FirearmRepository firearmRepoIn, Models.LocationRepository locationRepoIn, Models.SessionRepository sessionRepoIn)
	{
		InitializeComponent();

		MainPage = new AppShell();

		FirearmRepo = firearmRepoIn;
		LocationRepo = locationRepoIn;
		SessionRepo = sessionRepoIn;

	}
}
