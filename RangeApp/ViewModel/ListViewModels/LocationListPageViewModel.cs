using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class LocationListPageViewModel : ObservableObject, IQueryAttributable
{
    public LocationListPageViewModel()
    {
        AllLocations = new List<ViewModel.LocationData>(App.LocationRepo.GetAllLocationData());
        RefinedLocations = new ObservableCollection<ViewModel.LocationData>(AllLocations);
    }

    [ObservableProperty]
    ViewModel.LocationData? selectedLocation;
    List<ViewModel.LocationData> AllLocations;
    [ObservableProperty]
    ObservableCollection<ViewModel.LocationData> refinedLocations;
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty]
    string searchInputText = string.Empty;

    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        StatusMessage = string.Empty;
        if (attributes == null)
            return;
        if (attributes.ContainsKey("AddedLocation"))
        {
            SelectedLocation = (attributes["AddedLocation"] as ViewModel.LocationData);
            UpdateList();
        }
        attributes.Clear();
    }

    void AddAllToRefined()
    {
        RefinedLocations = new ObservableCollection<ViewModel.LocationData>(AllLocations);
    }


    private void FilterLocations()
    {
        if (SearchInputText.Length == 0)
        {
            AddAllToRefined();
        }
        else
        {
            RefinedLocations.Clear();
            string search_text = SearchInputText.ToLower();
            foreach (var item in AllLocations)
            {
                if (item.Name != null)
                    if (item.Name.ToLower().Contains(search_text))
                        RefinedLocations.Add(item);
            }
        }
    }
    public void UpdateList()
    {
        SearchInputText = string.Empty;
        AllLocations = new List<ViewModel.LocationData>(App.LocationRepo.GetAllLocationData());
        FilterLocations();
    }
    public void SetStatusMessage(string message)
    {
        StatusMessage = message;
    }

    [RelayCommand]
    public void SearchTextChanged()
    {
        FilterLocations();
    }

    [RelayCommand]
    async private void DeleteSelected()
    {
        // checks that an item is selected
        if (SelectedLocation != null)
        {
            string question = string.Format("Delete {0}?", SelectedLocation.Name);
            // displays a pop up to make sure the user wishes to delete the entry
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                bool response = await Application.Current.MainPage.DisplayAlert(
                        "Alert", question, "Yes", "No");
                if (!response)
                    return;
            }

            int result = App.LocationRepo.RemoveLocation(SelectedLocation);
            if (result == 0)
            {
                StatusMessage = App.LocationRepo.StatusMessage;
            }
            else
            {
                StatusMessage = string.Format("Removed {0}.", SelectedLocation.Name);
                AllLocations.Remove(SelectedLocation);
                UpdateList();
                SelectedLocation = null;
            }
        }
        // if a location is not selected
        else
        {
            StatusMessage = "No Location Selected to remove";
        }
    }

    [RelayCommand]
    void NewLocation()
    {
        StatusMessage = "Creating New Location";
        Shell.Current.GoToAsync("NewLocationPage");
    }

    [RelayCommand]
    async Task EditSelected()
    {
        if (SelectedLocation == null)
            return;

        StatusMessage = string.Format("Editing {0}", SelectedLocation.Name);
        var NavigationParameter = new Dictionary<string, object>
        {
            {"Location", SelectedLocation }
        };
        await Shell.Current.GoToAsync("NewLocationPage", NavigationParameter);
    }
}

