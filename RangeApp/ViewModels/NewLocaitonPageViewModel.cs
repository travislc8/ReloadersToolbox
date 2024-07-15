using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class NewLocationPageViewModel : ObservableObject
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

    public void SetStatusMessage(string message)
    {
        StatusMessage = message;
    }
    public bool CheckNameIsDuplicate(string name)
    {
        if (AvailableLocaitons != null)
        {
            for (int i = 0; i < AvailableLocaitons.Count; i++)
            {
                if (name == AvailableLocaitons[i])
                    return true;
            }
        }
        return false;
    }
}
