using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class FirearmListPageViewModel : ObservableObject
{
    public FirearmListPageViewModel()
    {
        AllFirearms = new List<Models.Firearm>(App.FirearmRepo.GetAllFirearms());
        RefinedFirearms = new ObservableCollection<Models.Firearm>(AllFirearms);

        StatusMessage = App.FirearmRepo.StatusMessage;
		WeakReferenceMessenger.Default.Register<Models.SendItemMessage>(this, (r, m) =>
            {
                UpdateList();
                RefineSearch();
            });  
    }
    private Models.Firearm? selected_firearm;

    List<Models.Firearm>? AllFirearms;
    [ObservableProperty]
    ObservableCollection<Models.Firearm>? refinedFirearms;
    
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty] 
    string selectedName = string.Empty;
    [ObservableProperty] 
    string selectedBarrelLength = string.Empty;
    [ObservableProperty] 
    string selectedManufacture = string.Empty;
    [ObservableProperty] 
    string selectedCaliber = string.Empty;
    [ObservableProperty] 
    string selectedScope = string.Empty;
    [ObservableProperty] 
    string selectedId = string.Empty;
    [ObservableProperty] 
    string searchInputText =string.Empty;

    void AddAllToRefined()
    {
        RefinedFirearms.Clear();
        for (int i = 0; i < AllFirearms.Count; i++)
        {
            RefinedFirearms.Add(AllFirearms[i]);
        }
    }
    public void RefineSearch()
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
            for (int i = 0; i < AllFirearms.Count; i++)
            {
                if (AllFirearms[i].Name.Contains(SearchInputText))
                    RefinedFirearms.Add(AllFirearms[i]);
            }
        }
    }
    public void UpdateList()
    {
        AllFirearms = new List<Models.Firearm>(App.FirearmRepo.GetAllFirearms());
        AddAllToRefined();
    }
    public void SetStatusMessage(string message)
    {
        StatusMessage = message;
    }
    void ClearInfo()
    {
        SelectedName = string.Empty;
        SelectedBarrelLength = string.Empty;
        SelectedManufacture = string.Empty;
        SelectedCaliber = string.Empty;
        SelectedScope = string.Empty;
        SelectedId = string.Empty;
    }
    [RelayCommand]
    private void DeleteSelected()
    {
        if (selected_firearm != null)
        {
            AllFirearms.Remove(selected_firearm);
            App.FirearmRepo.RemoveFirearm(selected_firearm);
            StatusMessage = App.FirearmRepo.StatusMessage;
            UpdateList();
            ClearInfo();
        }
    }
    [RelayCommand]
    private void ItemSelected(Models.Firearm selected)
    {
        if (selected != null)
        {
            SelectedName = selected.Name;
            SelectedBarrelLength = selected.BarrelLength.ToString();
            SelectedManufacture = selected.Manufacturer;
            SelectedCaliber = selected.Caliber;
            SelectedScope = selected.ScopeID;
            SelectedId = selected.Id.ToString();
        }
        selected_firearm = selected;
    }
}

