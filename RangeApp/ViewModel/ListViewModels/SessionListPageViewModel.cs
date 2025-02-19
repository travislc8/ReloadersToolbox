using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class SessionListPageViewModel : ObservableObject, IQueryAttributable
{

    public SessionListPageViewModel()
    {
        UpdateAllSessionData();
    }
    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        if (attributes == null)
            return;
        UpdateAllSessionData();
        attributes.Clear();
    }

    List<ViewModel.SessionData> AllSessionData = [];

    [ObservableProperty]
    string sessionSearchText = string.Empty;
    [ObservableProperty]
    ObservableCollection<ViewModel.SessionData> refinedSessionData = [];
    [ObservableProperty]
    ViewModel.SessionData? selectedSession;
    [ObservableProperty]
    string statusMessage = string.Empty;

    [RelayCommand]
    void SessionSelected(int index)
    {
        SelectedSession = AllSessionData[index];
    }
    [RelayCommand]
    async Task DeleteSelected()
    {
        if (SelectedSession != null)
        {
            // displays a pop up to make sure the user wishes to delete the entry
            string question = string.Format("Delete {0}?", SelectedSession.Name);
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                bool response = await Application.Current.MainPage.DisplayAlert(
                        "Alert", question, "Yes", "No");
                if (!response) return;
            }

            // deletes the session
            int result = App.SessionRepo.DeleteSession(SelectedSession.SessionId);
            if (result != 0)
            {
                StatusMessage = string.Format("Deleted {0}.", SelectedSession.Name);
                UpdateAllSessionData();
            }
            else
            {
                StatusMessage = string.Format("Could not delete {0}.", SelectedSession.Name);
            }
        }
        else
        {
            StatusMessage = "No Session Selected";
        }
    }

    [RelayCommand]
    async Task NewSessionSelected()
    {
        await Shell.Current.GoToAsync("SessionOptions");
    }
    [RelayCommand]
    async Task EditSessionSelected()
    {
        if (SelectedSession == null)
        {
            StatusMessage = "No Session Selected";
            return;
        }
        StatusMessage = string.Format("Editing {0}", SelectedSession.SessionId);
        Preferences.Set("SessionActive", SelectedSession.SessionId);
        var NavigationParameter = new Dictionary<string, object> {
            {"SessionData", SelectedSession}
        };
        await Shell.Current.GoToAsync("SessionPage", NavigationParameter);
    }
    [RelayCommand]
    void ViewFirearmSelected()
    {
        //TODO
        StatusMessage = "Not Implemented";
    }
    [RelayCommand]
    async Task SessionSearchTextChanged()
    {
        await Task.Run(() => UpdateRefinedSessionData());
    }
    private void UpdateAllSessionData()
    {
        AllSessionData = App.SessionRepo.GetSessionDataList();
        if (AllSessionData.Count == 0)
        {
            StatusMessage = "No Sessions to View";
        }
        else
        {
            UpdateRefinedSessionData();
        }
    }
    private void UpdateRefinedSessionData()
    {
        RefinedSessionData.Clear();
        foreach (var session in AllSessionData)
        {
            if (session.Name.Contains(SessionSearchText))
                RefinedSessionData.Add(session);
        }
    }
}
