using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Model.Utils;

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

    List<ViewModel.SessionData> AllSessionData;

    [ObservableProperty]
    ObservableCollection<ViewModel.SessionData> refinedSessionData;
    [ObservableProperty]
    ViewModel.SessionData? selectedSession;
    [ObservableProperty]
    string buttonStatusMessage = "";
    [ObservableProperty]
    string statusMessage = string.Empty;

    [RelayCommand]
    void SessionSelected(int index)
    {
        SelectedSession = AllSessionData[index];
    }
    [RelayCommand]
    void DeleteSelected()
    {
        if (SelectedSession != null)
        {
            int result = App.SessionRepo.DeleteSession(SelectedSession.SessionId);
            if (result != 0)
            {
                ButtonStatusMessage = string.Format("Deleted {0}.", SelectedSession.Name);
                UpdateAllSessionData();
            }
            else
            {
                ButtonStatusMessage = string.Format("Could not delete {0}.", SelectedSession.Name);
            }
        }
        else
        {
            ButtonStatusMessage = "No Session Selected";
        }
    }
    [RelayCommand]
    async Task EditSessionSelected()
    {
        if (SelectedSession == null)
        {
            StatusMessage = "No Session Selected";
            return;
        }
        StatusMessage = string.Format("Editing {0}", SelectedSession.Name);
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
    async void SessionSearchTextChanged()
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
        RefinedSessionData = new ObservableCollection<SessionData>(AllSessionData);
    }
}
