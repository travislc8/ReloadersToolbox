namespace RangeApp;

public partial class App : Application
{
    public static Models.RangeDayRepository rangeDayRepo { get; private set; }
	public App(Models.RangeDayRepository firearmRepo)
	{
		InitializeComponent();

		MainPage = new AppShell();

		rangeDayRepo = firearmRepo;
	}
}
