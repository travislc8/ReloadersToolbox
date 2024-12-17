using CommunityToolkit.Mvvm.Messaging;
using RangeApp.Models;
namespace RangeApp.Views;

public partial class NewFirearmPage : ContentPage
{
    public NewFirearmPage(ViewModel.NewFirearmPageViewModel vm)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
        textColor = NewFirearmName.TextColor;
    }
    public NewFirearmPage()
    {
        InitializeComponent();
        VM = new ViewModel.NewFirearmPageViewModel();
        BindingContext = VM;
        textColor = NewFirearmName.TextColor;
        NewFirearmCaliber.Text = VM.Caliber;
    }
    private ViewModel.NewFirearmPageViewModel VM;
    private Color? textColor = null;
    private bool AddFirearmParamtersPass = false;

    private bool NewFirearmNamePass = false;

    /// <summary>
    /// Checks input of name text and changes text color, sets error message, 
    /// and disables save button if it is bad input
    /// </summary>
    void NewFirearmNameTextChanged(object sender, EventArgs e)
    {
        // name to long
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
        // name to short
        else if (NewFirearmName.Text.Length <= 0)
        {
            NewFirearmName.Placeholder = "Enter Name For New FirearmName";
            VM.SetStatusMessage("");
            NewFirearmNamePass = false;
            if (AddFirearmParamtersPass)
            {
                saveFirearmButton.IsEnabled = false;
                AddFirearmParamtersPass = false;
            }
        }
        // name length is good 
        else
        {
            // duplicate name check in database
            if (VM.CheckNameIsDuplicate())
            {
                // in edit mode so check duplicate is not itself
                var name = VM.getFirearmName();
                if (name != null && name == NewFirearmName.Text)
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
                // name is a duplicate
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
            // name is good
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

    /// <summary>
    /// Checks that the BarrelLength field is valid and sets the save button to clickable
    /// if it is valid
    /// </summary>
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
    /// <summary>
    /// Checks that the Manufacture field is valid and sets the save button to clickable
    /// if it is valid
    /// </summary>
    private bool NewFirearmManufactrurerPass = false;
    void NewFirearmManufacturerTextChanged(object sender, EventArgs e)
    {
        if (NewFirearmManufacturer.Text.Length > 30)
        {
            NewFirearmManufacturer.TextColor = Colors.Red;
            VM.SetStatusMessage("Manufacture Name To Long");
            NewFirearmManufactrurerPass = false;
        }
        else if (NewFirearmManufacturer.Text.Length < 1)
        {
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

    /// <summary>
    /// Checks that the Caliber field is valid and sets the save button to clickable
    /// if it is valid
    /// </summary>
    private bool NewFirearmCaliberPass = false;
    void NewFirearmCaliberTextChanged(object sender, EventArgs e)
    {
        if (NewFirearmCaliber.Text.Length > 20)
        {
            NewFirearmCaliber.TextColor = Colors.Red;
            VM.SetStatusMessage("Diameter Name To Long");
            NewFirearmCaliberPass = false;
        }
        else if (NewFirearmCaliber.Text.Length < 1)
        {
            VM.SetStatusMessage("");
            NewFirearmCaliber.Placeholder = "Enter Diameter";
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

    /// <summary>
    /// Checks that the Scope field is valid and sets the save button to clickable
    /// if it is valid
    /// </summary>
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
}
