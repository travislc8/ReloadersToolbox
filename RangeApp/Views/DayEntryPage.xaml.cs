namespace RangeApp.Views;

public partial class DayEntryPage : ContentPage
{

	public DayEntryPage()
	{
		InitializeComponent();
	}
	async void NewSalvo(Object sender, EventArgs e)
	{
		await Navigation.PushAsync(new SessionOptions());
	}
}
