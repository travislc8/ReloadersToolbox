using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class NewLocationPageViewModel : ObservableObject, IQueryAttributable
{
    public NewLocationPageViewModel()
    {
        AvailableLocaitons = new ObservableCollection<string>(App.LocationRepo.GetAllLocationNames());

        StatusMessage = App.LocationRepo.StatusMessage;
    }

    [ObservableProperty]
    ObservableCollection<Models.Location>? allLocations;
    [ObservableProperty]
    ObservableCollection<string>? availableLocaitons;
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty]
    string name = string.Empty;
    [ObservableProperty]
    string nameStatus = string.Empty;
    [ObservableProperty]
    string shootingDirection = string.Empty;
    private Models.Location _Location = new Models.Location();
    private bool AllowSave = false;

    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        if (attributes == null)
            return;
        if (attributes.ContainsKey("Location"))
        {
            var Location = attributes["Location"] as Models.Location;

            if (Location == null)
                return;
            _Location.Id = Location.Id;
            if (Location.Name != null)
            {
                _Location.Name = Location.Name;
                Name = Location.Name;
            }
            if (Location.ShootingDirection != null)
            {
                _Location.ShootingDirection = Location.ShootingDirection;
                ShootingDirection = Location.ShootingDirection.ToString();
            }

        }
    }
    public void NameTextChanged()
    {
        string status = string.Empty;
        var check = Model.Utils.Validate.String(Name,  ref status, 50);
        NameStatus = status;
        AllowSave = check;
        if (_Location.Id != 0 && Name == _Location.Name)
        {
            return;
        }
        if (CheckNameIsDuplicate())
        {
            NameStatus = "Name Already Exists";
        }
    }
    public void SetStatusMessage(string message)
    {
        StatusMessage = message;
    }
    public bool CheckNameIsDuplicate()
    {
        if (AvailableLocaitons != null)
        {
            for (int i = 0; i < AvailableLocaitons.Count; i++)
            {
                if (Name == AvailableLocaitons[i])
                    return true;
            }
        }
        return false;
    }
    [RelayCommand]
    void SaveLocation()
    {
        if (Name == string.Empty)
            return;

        string status = string.Empty;
        if (!Model.Utils.Validate.IntFromString(ShootingDirection, out int shootingDirection, ref status))
        {
            StatusMessage = status;
        }

		var new_location = new Models.Location
		{
			Name = this.Name,
			ShootingDirection = shootingDirection,
		};
		if (_Location.Id != 0)
		{
			new_location.Id = _Location.Id;
		}


		App.LocationRepo.AddNewLocation(new_location);

		var NavigationParameter = new Dictionary<string, object>
		{
			{"AddedLocation", "1" }
		};
		Shell.Current.GoToAsync("..", NavigationParameter);
    }
}
