using RangeApp.Models;
namespace RangeApp.Views;

public partial class SessionOptions : ContentPage
{
	//private string? sessionName;
	private List<Firearm> firearmsInSession { get; set; }
	private List<Locations> locations { get; set; }

	public SessionOptions()
	{
		InitializeComponent();
		locations = App.rangeDayRepo.GetAllLocations();
		firearmsInSession = new List<Firearm>();
		FirearmsPicker.ItemsSource = App.rangeDayRepo.GetAllFirearms();	
		FirearmsPicker.ItemDisplayBinding = new Binding("Name");
		LocationPicker.ItemDisplayBinding = new Binding("Name");
		FirearmList.ItemsSource = firearmsInSession;
	}
	// fix
	public void SaveFirearm(object sender, EventArgs e)
	{
		statusMessage.Text = "";
		int barrelLength = int.Parse(newFirearmBarrelLength.Text);
        var firearm = new Firearm
        {
            Name = newFirearmName.Text,
            BarrelLength = barrelLength,
            Manufacturer = newFirearmManufacturer.Text,
            Caliber = newFirearmCaliber.Text,
            ScopeID = newFirearmScope.Text
        };

        App.rangeDayRepo.AddNewFirearm(firearm);
		firearmsInSession.Add(firearm);


		statusMessage.Text = App.rangeDayRepo.StatusMessage;
		//updates the list
		FirearmsPicker.ItemsSource = firearmsInSession;	

		newFirearmName.IsVisible = false;
		newFirearmBarrelLength.IsVisible = false;
		newFirearmManufacturer.IsVisible = false;
		newFirearmCaliber.IsVisible = false;
		newFirearmScope.IsVisible = false;
		saveFirearmButton.IsVisible = false;
	}
	void GetFirearms(object sender, EventArgs e)
	{
		statusMessage.Text = "";
		
		FirearmsPicker.ItemsSource = firearmsInSession;	
		FirearmList.ItemsSource = firearmsInSession;
		statusMessage.Text = App.rangeDayRepo.StatusMessage;
	}
	void NewLocation(object sender, EventArgs e)
	{
		LocationEntry.IsVisible = true;
		DirectionEntry.IsVisible = true;
	}
	void SelectedFirearm(object sender, EventArgs e)
	{
		statusMessage.Text = "";
		var picker = (Picker)sender;
		int selectedIndex = picker.SelectedIndex;
		if (selectedIndex != -1) {
            firearmsInSession.Add(App.rangeDayRepo.GetFirearmFromName(picker.Items[selectedIndex]));
        }
		statusMessage.Text = App.rangeDayRepo.StatusMessage;
		FirearmList.ItemsSource = firearmsInSession;

    }
	void AddFirearm(object sender, EventArgs e)
	{
		newFirearmName.IsVisible = true;
		newFirearmBarrelLength.IsVisible = true;
		newFirearmManufacturer.IsVisible = true;
		newFirearmCaliber.IsVisible = true;
		newFirearmScope.IsVisible = true;
		saveFirearmButton.IsVisible = true;
	}

    async void Save(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
	async void Exit(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}