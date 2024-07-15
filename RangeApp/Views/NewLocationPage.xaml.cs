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
		if (NewLocationName.Text.Length > 50)
		{

			VM.SetStatusMessage("Name To Long");
			NewLocationNamePass = false;
			if (AddLocationParamtersPass)
			{
				saveLocationButton.IsEnabled = false;
				AddLocationParamtersPass = false;
			}
		}
		else if (NewLocationName.Text.Length <= 0)
		{
			NewLocationName.Placeholder = "Enter Name For New Location";
			VM.SetStatusMessage("");
			NewLocationNamePass = false;
			if (AddLocationParamtersPass)
			{
				saveLocationButton.IsEnabled = false;
				AddLocationParamtersPass = false;
			}
		}
		else
		{
			if (VM.CheckNameIsDuplicate(NewLocationName.Text))
			{
				// in edit mode so check duplicate is not itself
				if (edit_mode && edit_location_name == NewLocationName.Text)
				{
					if (NewLocationName.TextColor != textColor)
						NewLocationName.TextColor = textColor;
					NewLocationNamePass = true;
					VM.SetStatusMessage("");
					if (!AddLocationParamtersPass && !saveLocationButton.IsEnabled)
					{
						if (AddLocationPararmetersPassCheck())
							saveLocationButton.IsEnabled = true;
					}
				}
				else
				{
					VM.SetStatusMessage("Name Already Exists");
					NewLocationName.TextColor = Colors.Red;
					NewLocationNamePass = false;
					if (AddLocationParamtersPass)
					{
						saveLocationButton.IsEnabled = false;
						AddLocationParamtersPass = false;
					}
				}
			}
			else
			{
				if (NewLocationName.TextColor != textColor)
					NewLocationName.TextColor = textColor;
				NewLocationNamePass = true;
				VM.SetStatusMessage("");
				if (!AddLocationParamtersPass && !saveLocationButton.IsEnabled)
				{
					if (AddLocationPararmetersPassCheck())
						saveLocationButton.IsEnabled = true;
				}
			}
        }
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
	private void SaveLocation(object sender, EventArgs e)
	{
		if (!AddLocationPararmetersPassCheck())
			return;

		int shootingDirection;
		if (NewShootingDirection.Text != null)
            shootingDirection = int.Parse(NewShootingDirection.Text);
        else
            shootingDirection = 0;
		var new_location = new Models.Location
		{
			Name = NewLocationName.Text,
			ShootingDirection = shootingDirection,
		};
		if (location != null)
		{
			new_location.Id = location.Id;
		}


        App.LocationRepo.AddNewLocation(new_location);

        WeakReferenceMessenger.Default.Send(new SendItemMessage(NewLocationName.Text));	
		Navigation.PopAsync();
	}
	private void CancelSaveLocation(object sender, EventArgs e)
	{
		Navigation.PopAsync();
	}
}
