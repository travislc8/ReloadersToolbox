using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class NewFirearmPageViewModel : ObservableObject
{
    public NewFirearmPageViewModel()
    {
        AvailableFirearms = new ObservableCollection<string>(App.FirearmRepo.GetAllFirearmNames());

        StatusMessage = App.FirearmRepo.StatusMessage;
    }

    [ObservableProperty]
    ObservableCollection<Models.Firearm>? allFirearms;
    [ObservableProperty]
    ObservableCollection<string>? availableFirearms;
    [ObservableProperty]
    string statusMessage = string.Empty;

    public void SetStatusMessage(string message)
    {
        StatusMessage = message;
    }
    public bool CheckNameIsDuplicate(string name)
    {
        if (AvailableFirearms != null)
        {
            for (int i = 0; i < AvailableFirearms.Count; i++)
            {
                if (name == AvailableFirearms[i])
                    return true;
            }
        }
        return false;
    }
}
