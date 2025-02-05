using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class SessionOptionsViewModel : ObservableObject, IQueryAttributable
{
    public SessionOptionsViewModel()
    {
        UpdateFirearms();
        UpdateLocations();
        SessionNames = new ObservableCollection<string>(App.SessionRepo.GetSessionNames());
    }
    [ObservableProperty]
    bool allowSave = false;
    [ObservableProperty]
    ViewModel.SessionData session = new();
    [ObservableProperty]
    List<ViewModel.FirearmData> availableFirearms = [];
    [ObservableProperty]
    List<ViewModel.LocationData> availableLocations = [];
    [ObservableProperty]
    ObservableCollection<ViewModel.FirearmData> filteredFirearms = [];
    [ObservableProperty]
    ObservableCollection<ViewModel.FirearmData> firearmsInSession = [];
    [ObservableProperty]
    ObservableCollection<ViewModel.LocationData> filteredLocations = [];
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty]
    ObservableCollection<string> sessionNames;
    [ObservableProperty]
    string nameStatusMessage = string.Empty;
    [ObservableProperty]
    FirearmData? selectedFirearmInSession;
    [ObservableProperty]
    FirearmData? selectedFirearm;
    [ObservableProperty]
    LocationData? selectedLocaiton;
    [ObservableProperty]
    string locationSearch = string.Empty;
    [ObservableProperty]
    bool showLocation = true;
    [ObservableProperty]
    string firearmSearch = string.Empty;
    [ObservableProperty]
    bool firearmRemoveButtonVisible = false;
    [ObservableProperty]
    bool firearmRemoveButtonClickable = false;
    [ObservableProperty]
    string addFirearmStatusMessage = string.Empty;

    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        if (attributes == null)
        {
            StatusMessage = "";
            return;
        }
        if (attributes.ContainsKey("Location"))
        {
            StatusMessage = "Added Location";
            UpdateLocations();
        }
        else if (attributes.ContainsKey("Firearm"))
        {
            StatusMessage = "Added Firearm";
            UpdateFirearms();
        }
        attributes.Clear();
    }

    [RelayCommand]
    async void NewLocation()
    {
        await Shell.Current.GoToAsync("NewLocationPage");
    }

    [RelayCommand]
    async void NewFirearm()
    {
        await Shell.Current.GoToAsync("NewFirearmPage");
    }

    [RelayCommand]
    void LocationSelected()
    {
    }

    [RelayCommand]
    void AddLocation()
    {
        if (Session.Location.Name != null && Session.Location.Name != null)
            LocationSearch = Session.Location.Name;
        ShowLocation = !ShowLocation;
    }

    [RelayCommand]
    void RemoveFirearm()
    {
        if (SelectedFirearmInSession != null)
        {
            Session.Firearms.Remove(SelectedFirearmInSession);
            FirearmsInSession.Remove(SelectedFirearmInSession);
            UpdateRemoveFirearmButton();
        }
    }

    [RelayCommand]
    void AddFirearm()
    {
        if (SelectedFirearm == null)
        {
            AddFirearmStatusMessage = "No firearm selected";
        }
        else if (Session.Firearms.Contains(SelectedFirearm))
        {
            AddFirearmStatusMessage = string.Format("{0} already in session", SelectedFirearm.Name);
        }
        else
        {
            Session.Firearms.Add(SelectedFirearm);
            FirearmsInSession.Add(SelectedFirearm);
            UpdateRemoveFirearmButton();
            AddFirearmStatusMessage = string.Format("Added {0} to session",
                    SelectedFirearm.Name);
        }
    }

    [RelayCommand]
    void NameTextChanged()
    {
        if (SessionNames.Contains(Session.Name))
            NameStatusMessage = "Name Already Used";
        else
        {
            string message = string.Empty;
            AllowSave = Model.Utils.Validate.String(Session.Name, ref message, 30);
            NameStatusMessage = message;
        }

    }

    [RelayCommand]
    async void SaveSession()
    {
        StatusMessage = "Saving Session";
        if (Session.Name == string.Empty)
        {
            StatusMessage = "Name must not be blank";
            return;
        }
        if (Session.Location.Id == 0)
        {
            StatusMessage = "A location must be selected";
            return;
        }

        var saveSession = new Models.Session
        {
            LocationId = Session.Location.Id,
            Note = Session.Note,
            Id = Session.SessionId,
            Name = Session.Name,
        };

        int result = App.SessionRepo.AddSession(saveSession);
        if (result == 0)
        {
            StatusMessage = "Error Creating " + Session.Name + App.SessionRepo.StatusMessage;
            return;
        }
        Session.SessionId = App.SessionRepo.GetSessionIdFromName(Session.Name);
        // does not try to add firearms if there are none
        if (Session.Firearms.Count != 0)
            App.SessionRepo.AddFirearmsToSession(FirearmData.GetFirearm(Session.Firearms), Session.SessionId);

        StatusMessage = "Creating Session: " + Session.Name;
        // sets the save flag that a session is being edited
        Preferences.Set("SessionActive", Session.SessionId);

        var NavigationParameter = new Dictionary<string, object>
        {
            {"SessionData", Session}
        };

        await Shell.Current.GoToAsync("SessionPage", NavigationParameter);
    }

    [RelayCommand]
    async void CancelSession()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async void FirearmSearchTextChanged()
    {
        await Task.Run(() => FilterFirearms());
    }

    [RelayCommand]
    async void LocationSearchTextChanged()
    {
        ShowLocation = true;
        FilteredLocations = await Task.Run(() => FilterLocations());
    }
    [RelayCommand]
    void FirearmSelected()
    {
        UpdateRemoveFirearmButton();
    }

    async private void UpdateLocations()
    {
        AvailableLocations = LocationData.GetData(App.LocationRepo.GetAllLocations());
        FilteredLocations = await Task.Run(() => FilterLocations());
    }

    private void UpdateFirearms()
    {
        AvailableFirearms = FirearmData.GetData(App.FirearmRepo.GetAllFirearms());
        FilterFirearms();
        UpdateRemoveFirearmButton();
    }

    ///<summary>
    /// Updates the FilteredFirearms based on the filters
    ///</summary>
    private void FilterFirearms()
    {
        FilteredFirearms.Clear();
        foreach (var firearm in AvailableFirearms)
        {
            if (firearm.Name != null && firearm.Name.ToLower().Contains(FirearmSearch.ToLower()))
                FilteredFirearms.Add(firearm);
        }
    }

    ///<summary>
    /// Updates the FilteredLocations based on the filters
    ///</summary>
    private ObservableCollection<LocationData> FilterLocations()
    {
        ObservableCollection<LocationData> filtered = new();
        foreach (var location in AvailableLocations)
        {
            if (location.Name != null && location.Name.ToLower().Contains(LocationSearch.ToLower()))
                filtered.Add(location);
        }

        return filtered;
    }

    private void UpdateRemoveFirearmButton()
    {
        if (Session.Firearms.Count == 0)
        {
            FirearmRemoveButtonVisible = false;
            FirearmRemoveButtonClickable = false;
        }
        else if (SelectedFirearmInSession == null)
        {
            FirearmRemoveButtonVisible = true;
            FirearmRemoveButtonClickable = false;
        }
        else
        {
            FirearmRemoveButtonVisible = true;
            FirearmRemoveButtonClickable = true;
        }
    }
}
