using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;
public partial class ModifyDataPageViewModel : ObservableObject
{
    public ModifyDataPageViewModel()
    {
        Options = new ObservableCollection<string>();
        AddOptions();
    }

    [ObservableProperty]
    ObservableCollection<string> options;
    [ObservableProperty]
    string name;
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty]
    int index;



    ///<summary>
    /// Adds the options to the list
    ///</summary>
    private void AddOptions()
    {
        Options.Add("Select");
        Options.Add("Firearm");
        Options.Add("Round");
        Options.Add("Location");
        Options.Add("Session");
    }

    [RelayCommand]
    async void ItemSelected()
    {
        switch (Index)
        {
            case 1:
                {
                    StatusMessage = "Opening Firearm List Page";
                    await Shell.Current.GoToAsync("FirearmListPage");
                    Index = 0;
                    break;
                }
            case 2:
                {
                    StatusMessage = "Opening Round List Page";
                    await Shell.Current.GoToAsync("RoundListPage");
                    Index = 0;
                    break;
                }
            case 3:
                {
                    StatusMessage = "Opening Location List Page";
                    await Shell.Current.GoToAsync("LocationListPage");
                    Index = 0;
                    break;
                }
            case 4:
                {
                    StatusMessage = "Opening Session List Page";
                    await Shell.Current.GoToAsync("SessionListPage");
                    Index = 0;
                    break;
                }
            default:
                {
                    StatusMessage = string.Empty;
                    break;
                }
        }
    }
}
