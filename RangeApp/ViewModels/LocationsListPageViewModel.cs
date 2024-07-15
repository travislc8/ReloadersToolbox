using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class LocationListPageViewModel : ObservableObject
{
    public LocationListPageViewModel()
    {
        AllLocations = new List<Models.Location>(App.LocationRepo.GetAllLocations());
        RefinedLocations = new ObservableCollection<Models.Location>(AllLocations);

        StatusMessage = App.LocationRepo.StatusMessage;
		WeakReferenceMessenger.Default.Register<Models.SendItemMessage>(this, (r, m) =>
            {
                UpdateList();
                RefineSearch();
            });  
    }
    private Models.Location? selected_location;

    List<Models.Location>? AllLocations;
    [ObservableProperty]
    ObservableCollection<Models.Location>? refinedLocations;
    
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty] 
    string selectedName = string.Empty;
    [ObservableProperty] 
    string selectedShootingDirection = string.Empty;
    [ObservableProperty] 
    string selectedId = string.Empty;
    [ObservableProperty] 
    string searchInputText =string.Empty;

    void AddAllToRefined()
    {
        RefinedLocations.Clear();
        for (int i = 0; i < AllLocations.Count; i++)
        {
            RefinedLocations.Add(AllLocations[i]);
        }
    }
    public void RefineSearch()
    {
        if (AllLocations == null)
            return;
        if (SearchInputText.Length == 0)
        {
            AddAllToRefined();
        }
        else
        {
            RefinedLocations.Clear();
            for (int i = 0; i < AllLocations.Count; i++)
            {
                if (AllLocations[i].Name.Contains(SearchInputText))
                    RefinedLocations.Add(AllLocations[i]);
            }
        }
    }
    public void UpdateList()
    {
        AllLocations = new List<Models.Location>(App.LocationRepo.GetAllLocations());
        AddAllToRefined();
    }
    public void SetStatusMessage(string message)
    {
        StatusMessage = message;
    }
    void ClearInfo()
    {
        SelectedName = string.Empty;
        SelectedShootingDirection = string.Empty;
    }
    [RelayCommand]
    private void DeleteSelected()
    {
        if (selected_location != null)
        {
            AllLocations.Remove(selected_location);
            App.LocationRepo.RemoveLocation(selected_location);
            StatusMessage = App.LocationRepo.StatusMessage;
            UpdateList();
            ClearInfo();
        }
    }
    [RelayCommand]
    private void ItemSelected(Models.Location selected)
    {
        if (selected != null)
        {
            SelectedName = selected.Name;
            SelectedShootingDirection = selected.ShootingDirection.ToString();
            SelectedId = selected.Id.ToString();
        }
        selected_location = selected;
    }
}

