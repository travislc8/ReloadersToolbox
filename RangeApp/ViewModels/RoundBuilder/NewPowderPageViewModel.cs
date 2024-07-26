using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;
public partial class NewPowderPageViewModel : ObservableObject
{

    public NewPowderPageViewModel()
    {
        AvailablePowders = new ObservableCollection<Models.Powder>(App.RoundRepo.GetPowders());

        StatusMessage = App.RoundRepo.StatusMessage;
    }

    [ObservableProperty]
    ObservableCollection<Models.Powder> availablePowders;
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty]
    string name = string.Empty;
    [ObservableProperty]
    string mfg = string.Empty;
    [ObservableProperty]
    string type = string.Empty;

    public void SetStatusMessage(string message)
    {
        StatusMessage = message;
    }
    public bool CheckName()
    {
        if (AvailablePowders != null)
        {
            if (Name.Length > 50)
            {
                StatusMessage = "Name is to long";
                return false;
            }
            else if (Name.Length == 0)
            {
                StatusMessage = "";
                return true;
            }
            else
            {
                for (int i = 0; i < AvailablePowders.Count; i++)
                {
                    if (Name == AvailablePowders[i].Name)
                    {
                        StatusMessage = "Name already exists";
                        return false;
                    }
                }
                StatusMessage = "";
                return true;
            }
        }
        return false;
    }
    public bool CheckMfg()
    {
        if (AvailablePowders != null)
        {
            if (Mfg.Length > 50)
            {
                StatusMessage = "Manufacturer Name is too long";
                return false;
            }
            else
            {
                if (StatusMessage != "")
                    StatusMessage = "";
                return true;
            }
        }
        else
        {
            if (StatusMessage != "")
                StatusMessage = "";
            return true;
        }
    }
    public bool CheckType()
    {
        if (AvailablePowders != null)
        {
            if (Type.Length > 50)
            {
                StatusMessage = "Type Name is too long";
                return false;
            }
            else
            {
                if (StatusMessage != "")
                    StatusMessage = "";
                return true;
            }
        }
        else
        {
            if (StatusMessage != "")
                StatusMessage = "";
            return true;
        }
    }
    [RelayCommand]
    public void SaveButton ()
    {
        int powder_added = 0;
        if (Name != string.Empty && Name.Length < 51)
        {
            var powder = new Models.Powder
            {
                Name = this.Name,
                PowderType = Type,
                PowderManufacturer = Mfg
            };
            powder_added = App.RoundRepo.AddNewPowder(powder);
        }
        var NavigationParemeter = new Dictionary<string, object>
        {
            {"AddedPowder", powder_added }
        };
        
        Shell.Current.GoToAsync("..",NavigationParemeter);
    }
    
}
