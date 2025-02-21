using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class HomePageViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    bool showContinueButton = false;
    [ObservableProperty]
    bool showNewButton = false;

    public HomePageViewModel()
    {
        updateShow();
    }

    ///<summary>
    /// Any time the page is returned to the continue or new session button must
    /// be updated
    ///</summary>
    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        updateShow();
        attributes.Clear();

    }
    [RelayCommand]
    void NewSession()
    {
        int session_id = Preferences.Get("SessionActive", 0);
        if (session_id == 0)
        {
            Shell.Current.GoToAsync("SessionListPage");
        }
        else 
        {
            var NavigationProperty = new Dictionary<string, object> {
                {"SessionId", session_id}
            };
            Shell.Current.GoToAsync("SessionPage", NavigationProperty);
        }
    }

    [RelayCommand]
    void ContinueSession()
    {
        int session_id = Preferences.Get("SessionActive", 0);
        if (session_id == 0)
        {
            Shell.Current.GoToAsync("SessionListPage");
        }
        else 
        {
            var NavigationProperty = new Dictionary<string, object> {
                {"SessionId", session_id}
            };
            Shell.Current.GoToAsync("SessionPage", NavigationProperty);
        }
    }

    [RelayCommand]
    void RoundBuilder()
    {

        Shell.Current.GoToAsync("NewRoundPage");
    }

    private void updateShow()
    {
        if (Preferences.Get("SessionActive", 0) != 0)
        {
            ShowContinueButton = true;
            ShowNewButton = false;
        }
        else
        {
            ShowContinueButton = false;
            ShowNewButton = true;
        }
    }
}
