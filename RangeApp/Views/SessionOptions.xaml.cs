using CommunityToolkit.Mvvm.Messaging;
using RangeApp.Models;
namespace RangeApp.Views;

public partial class SessionOptions : ContentPage
{

	public SessionOptions(ViewModel.SessionOptionsViewModel vm)
	{
		InitializeComponent();
		VM = vm;
		BindingContext = vm;
		locations = [];
		TextColor = NameEntry.TextColor;
		WeakReferenceMessenger.Default.Register<SendItemMessage>(this, (r, m) =>
            {
				VM.AddToFirearmsInSession(m.Value.ToString());
            });  
	}
	private List<Models.Location> locations { get; set; }
	private ViewModel.SessionOptionsViewModel VM;
	private Color TextColor;
	private async void NewLocation(object sender, EventArgs e)
	{
		var vm = new ViewModel.NewLocationPageViewModel();
		await Navigation.PushAsync(new Views.NewLocationPage(vm));
		VM.UpdateLocations();
	}
    private async void AddFirearm(object sender, EventArgs e)
    {
		var vm = new ViewModel.NewFirearmPageViewModel();
		await Navigation.PushAsync(new Views.NewFirearmPage(vm));
		
    }


    private void OnNameTextChanged(object sender, TextChangedEventArgs e)
    {
		if (NameEntry.Text.Length > 0)
		{
			if (VM.NameDuplicateCheck(NameEntry.Text))
			{
				VM.SetStatusMessage("Name Already Exists");
				SaveSessionButton.IsEnabled = false;
				if (NameEntry.TextColor == TextColor)
				{
					NameEntry.TextColor = Colors.Red;
				}
			}
			else if (NameEntry.Text.Length > 30)
			{
				VM.SetStatusMessage("Name Is Too Long");
				SaveSessionButton.IsEnabled = false;
				if (NameEntry.TextColor == TextColor)
				{
					NameEntry.TextColor = Colors.Red;
				}
			}
			else
			{
				if (NameEntry.TextColor != TextColor)
					NameEntry.TextColor = TextColor;
				VM.SetStatusMessage("");
				SaveSessionButton.IsEnabled = true;

			}
		}
		else
		{
			VM.SetStatusMessage("");
		}
    }
	private void SaveButtonClicked(object sender, EventArgs args)
	{
		VM.Save(NameEntry.Text, NoteEntry.Text);
	}
}