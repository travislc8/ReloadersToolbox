using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class HomePageViewModel : ObservableObject
{

    [RelayCommand]
    void NewSession()
    {
        Shell.Current.GoToAsync("SessionOptions");
    }

    [RelayCommand]
    void ContinueSession()
    {
        int session_id = Preferences.Get("SessionActive", 0);
        var NavigationProperty = new Dictionary<string, object> {
            {"SessionId", session_id}
        };
        Shell.Current.GoToAsync("SessionPage", NavigationProperty);
    }

    [RelayCommand]
    void RoundBuilder()
    {

        Shell.Current.GoToAsync("NewRoundPage");
    }

}
