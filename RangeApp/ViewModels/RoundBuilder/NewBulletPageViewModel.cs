using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;
public partial class NewBulletPageViewModel : ObservableObject
{

    public NewBulletPageViewModel()
    {
        AvailableBullets = new ObservableCollection<Models.Bullet>(App.RoundRepo.GetBullets());

        StatusMessage = App.RoundRepo.StatusMessage;
    }


    [ObservableProperty]
    ObservableCollection<Models.Bullet> availableBullets;
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty]
    string name = string.Empty;
    [ObservableProperty]
    string caliber = string.Empty;
    [ObservableProperty]
    string grains = string.Empty;
    [ObservableProperty]
    string mfg = string.Empty;

    public void SetStatusMessage(string message)
    {
        StatusMessage = message;
    }
    public bool CheckName()
    {
        if (Name.Length > 50)
        {
            StatusMessage = "Name is to long";
            return false;
        }
        if (Name.Length == 0)
        {
            StatusMessage = "";
            return true;
        }
        if (AvailableBullets != null)
        {
            for (int i = 0; i < AvailableBullets.Count; i++)
            {
                if (Name == AvailableBullets[i].Name)
                {
                    StatusMessage = "Name already exists";
                    return false;
                }
            }
            StatusMessage = "";
            return true;
        }
        return false;
    }
    public bool CheckMfg()
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
    public bool CheckCaliber()
    {
        if (Caliber.Length > 20)
        {
            StatusMessage = "Caliber Name is too long";
            return false;
        }
        else
        {
            if (StatusMessage != "")
                StatusMessage = "";
            return true;
        }
    }
    public bool CheckGrains()
    {
        if (Grains.Length == 0)
            return true;
        var test = int.TryParse(Grains, out int weight);
        if (!test)
        {
            StatusMessage = "Grains is not a number";
            return false;
        }
        else
        {
            StatusMessage = "";
            return true;
        } 
            
    }
    private bool CheckAll()
    {
        if (!CheckName())
            return false;
        if (!CheckMfg())
            return false;
        if (!CheckCaliber()) 
            return false;
        if (!CheckGrains())
            return false;
        return true;
    }
    [RelayCommand]
    public void SaveButton ()
    {
        int bullet_added = 0;
        if (Name != string.Empty && CheckAll())
        {
            var test = int.TryParse(Grains, out int weight);
            if (!test)
                weight = 0;
            var bullet = new Models.Bullet
            {
                Name = this.Name,
                Caliber = this.Caliber,
                BulletManufacturer = Mfg,
                BulletGrains = weight
            };
            bullet_added = App.RoundRepo.AddNewBullet(bullet);
        }
        var NavigationParemeter = new Dictionary<string, object>
        {
            {"AddedBullet", bullet_added }
        };
        
        Shell.Current.GoToAsync("..",NavigationParemeter);
    }
    
}
