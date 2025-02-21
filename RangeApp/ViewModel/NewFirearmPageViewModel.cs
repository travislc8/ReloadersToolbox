using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class NewFirearmPageViewModel : ObservableObject, IQueryAttributable
{
    public NewFirearmPageViewModel()
    {
        GetAvailableFirearms();
    }

    async private Task GetAvailableFirearms() {
        var firearms = await Task.Run(() => App.FirearmRepo.GetAllFirearmNames());
        AvailableFirearms = new ObservableCollection<string>(firearms);
        StatusMessage = App.FirearmRepo.StatusMessage;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        if (attributes == null)
            return;

        // catches return from adding/updating a firearm
        // sets the selected firearm to the one that was added/updated
        if (attributes.ContainsKey("Firearm"))
        {
            var result = attributes["Firearm"] as ViewModel.FirearmData;
            if (result != null)
            {
                firearm = result;
                setData();
            }

        }
    }

    [ObservableProperty]
    ObservableCollection<Models.Firearm>? allFirearms;
    [ObservableProperty]
    ObservableCollection<string>? availableFirearms;
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty]
    string? name = string.Empty;
    [ObservableProperty]
    string barrelLength = string.Empty;
    [ObservableProperty]
    string? manufacture = string.Empty;
    [ObservableProperty]
    string? caliber = string.Empty;
    [ObservableProperty]
    string? scopeId = string.Empty;
    ViewModel.FirearmData? firearm;

    /// <summary>
    /// Sets the data fields based on the firearm data
    /// </summary>
    private void setData()
    {
        if (firearm == null)
            return;
        if (firearm.Name != null)
            Name = firearm.Name;
        var length_temp = firearm.BarrelLength.ToString();
        if (length_temp != null)
            BarrelLength = length_temp;
        if (firearm.Manufacturer != null)
            Manufacture = firearm.Manufacturer;
        if (firearm.Caliber != null)
            Caliber = firearm.Caliber;
        if (firearm.ScopeID != null)
            ScopeId = firearm.ScopeID;
    }
    /// <summary>
    /// Sets the status message for errors
    /// </summary>
    public void SetStatusMessage(string message)
    {
        StatusMessage = message;
    }

    /// <summary>
    /// Checks that the name field is not the same name as a firearm already in
    /// the database
    /// </summary>
    public bool CheckNameIsDuplicate()
    {
        if (AvailableFirearms != null)
        {
            for (int i = 0; i < AvailableFirearms.Count; i++)
            {
                if (Name == AvailableFirearms[i])
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns the firearm name that is being edited. If not editing a firearm
    /// then null string is returned.
    /// </summary>
    public string? getFirearmName()
    {
        if (firearm != null)
        {
            // if the name is null, return an empty string
            if (firearm.Name == null)
                return string.Empty;
            else
                return firearm.Name;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Adds the firearm to the database and returns to the calling page. 
    /// Passes the new Firearm to the calling page
    /// </summary>
    [RelayCommand]
    void SaveFirearm()
    {
        if (Name == string.Empty)
        {
            StatusMessage = "Name must not be blank";
            return;
        }
        else
        {
            StatusMessage = "Saving Firearm";
        }
        // creates the Firearm object to save to the database
        int barrel_length;
        if (BarrelLength != string.Empty)
            barrel_length = int.Parse(BarrelLength);
        else
            barrel_length = 0;
        var new_firearm = new Models.Firearm();
        new_firearm.Name = Name;
        new_firearm.BarrelLength = barrel_length;
        new_firearm.Manufacturer = Manufacture;
        new_firearm.Caliber = Caliber;
        new_firearm.ScopeID = ScopeId;
        if (firearm != null)
        {
            new_firearm.Id = firearm.Id;
        }

        int result = App.FirearmRepo.AddNewFirearm(new_firearm);
        if (result == 0)
        {
            StatusMessage = "Could Not Save Firearm";
        }

        // Returns to the calling page
        // if the page is editing a firearm
        var return_data = ViewModel.FirearmData.GetData(new_firearm);
        if (firearm != null)
        {
            StatusMessage = string.Format("Edited {0}", new_firearm.Name);
            var navigationParamenter = new Dictionary<string, object>
            {
                {"Firearm", return_data }
            };
            Shell.Current.GoToAsync("..", navigationParamenter);
        }
        // if the page is creating a firearm
        else
        {
            StatusMessage = string.Format("Created {0}", new_firearm.Name);
            var navigationParamenter = new Dictionary<string, object>
            {
                {"Firearm", return_data }
            };
            Shell.Current.GoToAsync("..", navigationParamenter);
        }

    }

    /// <summary>
    /// Cancels the creation of a new Firearm 
    /// </summary>
    [RelayCommand]
    void Cancel()
    {
        Shell.Current.GoToAsync("..");
    }
}
