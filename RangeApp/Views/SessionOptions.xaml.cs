using CommunityToolkit.Mvvm.Messaging;
using RangeApp.Models;
namespace RangeApp.Views;

//TODO: Bind Picker to view model
public partial class SessionOptions : ContentPage
{

	public SessionOptions(ViewModel.SessionOptionsViewModel vm)
	{
		InitializeComponent();
		VM = vm;
		BindingContext = vm;
		locations = [];
		WeakReferenceMessenger.Default.Register<SendItemMessage>(this, (r, m) =>
            {
				VM.AddToFirearmsInSession(m.Value.ToString());
            });  
	}
	private List<Locations> locations { get; set; }
	private ViewModel.SessionOptionsViewModel VM;
	// fix
	void NewLocation(object sender, EventArgs e)
	{
		LocationEntry.IsVisible = true;
		DirectionEntry.IsVisible = true;
	}
    private async void AddFirearm(object sender, EventArgs e)
    {
		var vm = new ViewModel.NewFirearmPageViewModel();
		await Navigation.PushAsync(new Views.NewFirearmPage(vm));
		
    }

}