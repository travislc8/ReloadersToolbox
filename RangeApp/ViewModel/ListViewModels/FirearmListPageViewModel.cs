using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

/// View Model for the Firearm List Page
public partial class FirearmListPageViewModel : ObservableObject, IQueryAttributable
{
    /// Default Constructor for the ViewModel. Retrieves data from the database 
    /// populate the page.
    public FirearmListPageViewModel()
    {
        AllFirearms = new List<ViewModel.FirearmData>(App.FirearmRepo.GetAllFirearmData());
        RefinedFirearms = new ObservableCollection<ViewModel.FirearmData>(AllFirearms);

        StatusMessage = App.FirearmRepo.StatusMessage;
    }

    List<ViewModel.FirearmData> AllFirearms;
    [ObservableProperty]
    ViewModel.FirearmData? selectedFirearm;
    [ObservableProperty]
    ObservableCollection<ViewModel.FirearmData> refinedFirearms;
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty]
    string searchInputText = string.Empty;

    /// <summary> 
    /// Adds all of the Firearms in the AllFirearms List to the RefinedFirearms 
    /// collection 
    /// </summary>
    void AddAllToRefined()
    {
        RefinedFirearms.Clear();
        for (int i = 0; i < AllFirearms.Count; i++)
        {
            RefinedFirearms.Add(AllFirearms[i]);
        }
    }

    /// <summary>
    /// Updates Refined firearms list based on the search text
    /// </summary>
    [RelayCommand]
    public void SearchTextChanged()
    {
        if (AllFirearms == null)
            return;
        if (SearchInputText.Length == 0)
        {
            AddAllToRefined();
        }
        else
        {
            RefinedFirearms.Clear();
            foreach (var firearm in AllFirearms)
            {
                if (firearm.Name != null && firearm.Name.Contains(SearchInputText))
                    RefinedFirearms.Add(firearm);
            }
        }
    }
    /// <summary>
    /// Updates the AllFirearms List from the database
    /// </summary>
    public void UpdateFirearmList()
    {
        AllFirearms = new List<ViewModel.FirearmData>(App.FirearmRepo.GetAllFirearmData());
        AddAllToRefined();
    }

    public void SetStatusMessage(string message)
    {
        StatusMessage = message;
    }

    /// <summary>
    /// Starts a new instance of the page to add a firearm
    /// </summary>
    [RelayCommand]
    void NewFirearm()
    {
        Shell.Current.GoToAsync("NewFirearmPage");
    }

    [RelayCommand]
    void EditFirearm()
    {
        if (SelectedFirearm == null)
            return;
        var NavigationParameter = new Dictionary<string, object>
        {
            {"Firearm", SelectedFirearm}
        };
        Shell.Current.GoToAsync("NewFirearmPage", NavigationParameter);
    }

    [RelayCommand]
    async public void DeleteFirearm()
    {
        if (SelectedFirearm != null)
        {
            string question = string.Format("Delete {0}?", SelectedFirearm.Name);
            // displays a pop up to make sure the user wishes to delete the entry
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                bool response = await Application.Current.MainPage.DisplayAlert(
                        "Alert", question, "Yes", "No");
                if (!response)
                    return;
            }
            AllFirearms.Remove(SelectedFirearm);
            App.FirearmRepo.RemoveFirearm(SelectedFirearm);
            StatusMessage = App.FirearmRepo.StatusMessage;
            UpdateFirearmList();
        }
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
                UpdateFirearmList();
                SelectedFirearm = null;
                SelectedFirearm = result;
            }

        }
    }
}

