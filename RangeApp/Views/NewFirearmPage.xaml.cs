using CommunityToolkit.Mvvm.Messaging;
using RangeApp.Models;
namespace RangeApp.Views;

public partial class NewFirearmPage : ContentPage
{
	public NewFirearmPage(ViewModel.NewFirearmPageViewModel vm)
	{
		InitializeComponent();
		VM = vm;
		BindingContext = vm;
		textColor = NewFirearmName.TextColor;	
		WeakReferenceMessenger.Default.Register<Models.SendItemMessage>(this, (r, m) =>
            {
				PopulateWithFirearm(m.Value);	

            });  
	}
	private ViewModel.NewFirearmPageViewModel VM;
	private Color? textColor = null;
	private bool AddFirearmParamtersPass = false;
	private bool edit_mode = false;
	private string? edit_firearm_name;
	private Models.Firearm? firearm;

	private void PopulateWithFirearm(string name)
	{
        edit_firearm_name = name;
        edit_mode = true;
        firearm = App.FirearmRepo.GetFirearmFromName(edit_firearm_name);
        NewFirearmName.Text = firearm.Name;
		NewFirearmBarrelLength.Text = firearm.BarrelLength.ToString();
		NewFirearmManufacturer.Text = firearm.Manufacturer;
		NewFirearmCaliber.Text = firearm.Caliber;
		NewFirearmScope.Text = firearm.ScopeID;
	}
	private bool NewFirearmNamePass = false;
	void NewFirearmNameTextChanged(object sender, EventArgs e)
	{
		if (NewFirearmName.Text.Length > 50)
		{

			VM.SetStatusMessage("Name To Long");
            NewFirearmName.TextColor = Colors.Red;
            NewFirearmNamePass = false;
			if (AddFirearmParamtersPass)
			{
				saveFirearmButton.IsEnabled = false;
				AddFirearmParamtersPass = false;
			}
		}
		else if (NewFirearmName.Text.Length <= 0)
		{
			NewFirearmName.Placeholder = "Enter Name For New Firearm";
			VM.SetStatusMessage("");
			NewFirearmNamePass = false;
			if (AddFirearmParamtersPass)
			{
				saveFirearmButton.IsEnabled = false;
				AddFirearmParamtersPass = false;
			}
		}
		else
		{
			if (VM.CheckNameIsDuplicate(NewFirearmName.Text))
			{
				// in edit mode so check duplicate is not itself
				if (edit_mode && edit_firearm_name == NewFirearmName.Text)
				{
					if (NewFirearmName.TextColor != textColor)
						NewFirearmName.TextColor = textColor;
					NewFirearmNamePass = true;
					VM.SetStatusMessage("");
					if (!AddFirearmParamtersPass && !saveFirearmButton.IsEnabled)
					{
						if (AddFirearmPararmetersPassCheck())
							saveFirearmButton.IsEnabled = true;
					}
				}
				else
				{
					VM.SetStatusMessage("Name Already Exists");
					NewFirearmName.TextColor = Colors.Red;
					NewFirearmNamePass = false;
					if (AddFirearmParamtersPass)
					{
						saveFirearmButton.IsEnabled = false;
						AddFirearmParamtersPass = false;
					}
				}
			}
			else
			{
				if (NewFirearmName.TextColor != textColor)
					NewFirearmName.TextColor = textColor;
				NewFirearmNamePass = true;
				VM.SetStatusMessage("");
				if (!AddFirearmParamtersPass && !saveFirearmButton.IsEnabled)
				{
					if (AddFirearmPararmetersPassCheck())
						saveFirearmButton.IsEnabled = true;
				}
			}
        }
	}
	private bool NewFirearmBarrelLengthPass = false;
	void NewFirearmBarrelLengthTextChanged(object sender, EventArgs e)
	{
		if (NewFirearmBarrelLength.Text.Length > 0)
		{
			try
			{
				var barrelLength = int.Parse(NewFirearmBarrelLength.Text);
			}
			catch 
			{
				VM.SetStatusMessage("Barrel Length Must Be a Number");
				NewFirearmBarrelLength.TextColor = Colors.Red;
				NewFirearmBarrelLengthPass = false;
				return;
			}
			VM.SetStatusMessage("");
			if (NewFirearmBarrelLength.TextColor != textColor)
				NewFirearmBarrelLength.TextColor = textColor;
			NewFirearmBarrelLengthPass = true;
		}
		else
		{
			NewFirearmBarrelLengthPass = false;
			NewFirearmBarrelLength.Placeholder = "Enter Barrel Length"; 
			VM.SetStatusMessage("");
		}
		return;
    }
	private bool NewFirearmManufactrurerPass = false;
    void NewFirearmManufacturerTextChanged(object sender, EventArgs e)
	{
		if (NewFirearmManufacturer.Text.Length > 30)
		{
			NewFirearmManufacturer.TextColor = Colors.Red;
			VM.SetStatusMessage("Manufacture Name To Long");
			NewFirearmManufactrurerPass = false;
		}
		else if (NewFirearmManufacturer.Text.Length < 1){
			VM.SetStatusMessage("");
			NewFirearmManufacturer.Placeholder = "Enter Manufacturer";
			NewFirearmManufactrurerPass = false;
		}
		else
		{
			VM.SetStatusMessage("");
			if (NewFirearmManufacturer.TextColor != textColor)
                NewFirearmManufacturer.TextColor = textColor;
			NewFirearmManufactrurerPass = true;
        }
		
	}
	private bool NewFirearmCaliberPass = false;
	void NewFirearmCaliberTextChanged(object sender, EventArgs e)
	{
		if (NewFirearmCaliber.Text.Length > 20)
		{
			NewFirearmCaliber.TextColor = Colors.Red;
			VM.SetStatusMessage("Caliber Name To Long");
			NewFirearmCaliberPass = false;
		}
		else if (NewFirearmCaliber.Text.Length < 1)
		{
			VM.SetStatusMessage("");
			NewFirearmCaliber.Placeholder = "Enter Caliber";
			NewFirearmCaliberPass = false;
		}
		else
		{
			VM.SetStatusMessage("");
			if (NewFirearmCaliber.TextColor != textColor)
                NewFirearmCaliber.TextColor = textColor;
			NewFirearmCaliberPass = true;
        }
	}
	private bool NewFirearmScopePass = false;
	void NewFirearmScopeTextChanged(object sender, EventArgs e)
	{
		if (NewFirearmScope.Text.Length > 50)
		{
			NewFirearmScope.TextColor = Colors.Red;
			VM.SetStatusMessage("Scope Name To Long");
			NewFirearmScopePass = false;
		}
		else if (NewFirearmScope.Text.Length < 1)
		{
			VM.SetStatusMessage("");
			NewFirearmScope.Placeholder = "Enter Scope Name";
			NewFirearmScopePass = false;
		}
		else
		{
			VM.SetStatusMessage("");
			if (NewFirearmScope.TextColor != textColor)
                NewFirearmScope.TextColor = textColor;
			NewFirearmScopePass = true;
        }

    }
	private bool AddFirearmPararmetersPassCheck()
	{
		if (NewFirearmNamePass)
		{ 
			AddFirearmParamtersPass = true;
			return true; 
		}
		else
		{
			AddFirearmParamtersPass = false;
			return false;
		} 
	}
	private void SaveFirearm(object sender, EventArgs e)
	{
		if (!AddFirearmPararmetersPassCheck())
			return;

		int barrelLength;
		if (NewFirearmBarrelLength.Text != null)
            barrelLength = int.Parse(NewFirearmBarrelLength.Text);
        else
            barrelLength = 0;
		var new_firearm = new Firearm
		{
			Name = NewFirearmName.Text,
			BarrelLength = barrelLength,
			Manufacturer = NewFirearmManufacturer.Text,
			Caliber = NewFirearmCaliber.Text,
			ScopeID = NewFirearmScope.Text,
		};
		if (firearm != null)
		{
			new_firearm.Id = firearm.Id;
		}


        App.FirearmRepo.AddNewFirearm(new_firearm);

        WeakReferenceMessenger.Default.Send(new SendItemMessage(NewFirearmName.Text));	
		Navigation.PopAsync();
	}
	private void CancelSaveFirearm(object sender, EventArgs e)
	{
		Navigation.PopAsync();
	}
}