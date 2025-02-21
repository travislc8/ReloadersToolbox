using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class NewLocationPageViewModel : ObservableObject, IQueryAttributable
{
    public NewLocationPageViewModel()
    {
        Location = new ViewModel.LocationData();
        AvailableLocaitons = new();
        UpdateLocations();
    }

    [ObservableProperty]
    ObservableCollection<Models.Location>? allLocations;
    [ObservableProperty]
    ObservableCollection<string>? availableLocaitons;
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty]
    string nameStatus = string.Empty;
    [ObservableProperty]
    string directionStatus = string.Empty;

    [ObservableProperty]
    ViewModel.LocationData location;
    [ObservableProperty]
    string? shootingDirection = string.Empty;

    [ObservableProperty]
    bool allowSave = false;

    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        if (attributes == null)
            return;
        if (attributes.ContainsKey("Location"))
        {
            var loc = attributes["Location"] as ViewModel.LocationData;
            if (loc != null)
            {
                Location = loc;
                ShootingDirection = Location.ShootingDirection.ToString();
            }
            else
            {
                StatusMessage = "Could not load location to edit";
            }
        }
    }

    [RelayCommand]
    async public Task NameTextChanged()
    {
        await Task.Run(() => CheckLocationName());
    }

    private void CheckLocationName() {
        string status = string.Empty;
        if (!Model.Utils.Validate.String(Location.Name, ref status, 50))
        {
            StatusMessage = status;
            AllowSave = false;
        }
        else if (CheckNameIsDuplicate())
        {
            // if editing a location
            if (Location.Id != 0)
            {
                // if it is a name that is not it's original
                if (Location.Name != App.LocationRepo.GetLocationFromId(Location.Id).Name)
                {
                    NameStatus = "Name Already Exists";
                    AllowSave = false;
                }
                else
                {
                    NameStatus = string.Empty;
                    AllowSave = true;
                }
            }
            // new location
            else
            {
                NameStatus = "Name Already Exists";
                StatusMessage = string.Empty;
                AllowSave = false;
            }
        }
        else
        {
            NameStatus = string.Empty;
            StatusMessage = string.Empty;
            AllowSave = true;
        }
    }

    public bool CheckNameIsDuplicate()
    {
        if (Location.Name == null)
            return true;
        string search_text = Location.Name.ToLower();
        if (AvailableLocaitons != null)
        {
            foreach (var item in AvailableLocaitons)
            {
                if (Location.Name == item.ToLower())
                    return true;
            }
        }
        return false;
    }
    [RelayCommand]
    void SaveLocation()
    {
        string status = string.Empty;
        Model.Utils.Validate.IntFromString(ShootingDirection, out int direction, ref status);

        var new_location = new Models.Location
        {
            Name = Location.Name,
            ShootingDirection = direction,
        };
        if (Location.Id != 0)
        {
            new_location.Id = Location.Id;
        }


        int result = App.LocationRepo.AddNewLocation(new_location);
        if (result == 0)
        {
            StatusMessage = App.LocationRepo.StatusMessage;
            return;
        }

        var NavigationParameter = new Dictionary<string, object>
        {
            {"Location", ViewModel.LocationData.GetData(new_location) }
        };
        Shell.Current.GoToAsync("..", NavigationParameter);
    }

    [RelayCommand]
    void Cancel()
    {
        Shell.Current.GoToAsync("..");
    }

    async private Task UpdateLocations() {
        var locations = await Task.Run(() => App.LocationRepo.GetAllLocationNames());
        AvailableLocaitons = new ObservableCollection<string>(locations);
    }
}
