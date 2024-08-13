using CommunityToolkit.Mvvm.Messaging;
using RangeApp.Models;
namespace RangeApp.Views;

public partial class NewLocationPage : ContentPage
{
	public NewLocationPage(ViewModel.NewLocationPageViewModel vm)
	{
		InitializeComponent();
		VM = vm;
		BindingContext = vm;
		textColor = NewLocationName.TextColor;	
		WeakReferenceMessenger.Default.Register<Models.SendItemMessage>(this, (r, m) =>
            {
				PopulateWithLocation(m.Value);	

            });  
	}
	public NewLocationPage()
	{
		InitializeComponent();
		var vm = new ViewModel.NewLocationPageViewModel();
		VM = vm;
		BindingContext = vm;
		textColor = NewLocationName.TextColor;	
	}
	private ViewModel.NewLocationPageViewModel VM;
	private Color? textColor = null;
	private bool AddLocationParamtersPass = false;
	private bool edit_mode = false;
	private string? edit_location_name;
	private Models.Location? location;

	private void PopulateWithLocation(string name)
	{
        edit_location_name = name;
        edit_mode = true;
        location = App.LocationRepo.GetLocationFromName(edit_location_name);
        NewLocationName.Text = location.Name;
		NewShootingDirection.Text = location.ShootingDirection.ToString();
	}
	private bool NewLocationNamePass = false;
	void NewLocationNameTextChanged(object sender, EventArgs e)
	{
		VM.NameTextChanged();
	}
	private bool NewShootingDirectionParamterPass = false;
	void NewShootingDirectionTextChanged(object sender, EventArgs e)
	{
		if (NewLocationName.Text.Length > 0)
		{
			try
			{
				var locationName = int.Parse(NewShootingDirection.Text);
			}
			catch 
			{
				VM.SetStatusMessage("Shooting Direction Must Be a Number");
				NewShootingDirection.TextColor = Colors.Red;
				NewShootingDirectionParamterPass = false;
				return;
			}
			VM.SetStatusMessage("");
			if (NewShootingDirection.TextColor != textColor)
				NewShootingDirection.TextColor = textColor;
			NewShootingDirectionParamterPass = true;
		}
		else
		{
			NewShootingDirectionParamterPass = false;
			NewLocationName.Placeholder = "Enter Shooting Direction Length"; 
			VM.SetStatusMessage("");
		}
		return;
    }
	private bool AddLocationPararmetersPassCheck()
	{
		if (NewLocationNamePass)
		{ 
			AddLocationParamtersPass = true;
			return true; 
		}
		else
		{
			AddLocationParamtersPass = false;
			return false;
		} 
	}
	private void CancelSaveLocation(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("..");
	}

}
