using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

//session_id must be initialized
public partial class SessionPageViewModel : ObservableObject, IQueryAttributable
{
    public SessionPageViewModel()
    {
        UpdateAllRoundsList();
        UpdateAllFirearmsList();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        ClearStatusMessages();
        if (attributes == null)
            return;
        else if (attributes.ContainsKey("SessionData"))
        {
            var data = attributes["SessionData"] as ViewModel.SessionData;
            if (data == null)
            {
                StatusMessage = "Error: Session not loaded";
            }
            else
            {
                SessionData = data;
                UpdateAllFirearmsList();
                UpdateAllRoundsList();
                UpdateGroupData();
            }
        }
        else if (attributes.ContainsKey("Firearm"))
        {
            var firearm = attributes["Firearm"] as ViewModel.FirearmData;
            if (firearm == null)
                return;
            App.SessionRepo.AddFirearmToSession(FirearmData.GetFirearm(firearm), SessionData.SessionId);
            SessionData.Firearms.Add(firearm);
            AllFirearms.Add(firearm);
            FilterFirearms();
        }

        else if (attributes.ContainsKey("ShotAdded"))
        {
            if ((attributes["ShotAdded"] as string) != "0")
                UpdateGroupData();
        }
        else if (attributes.ContainsKey("AddedRound"))
        {
            UpdateAllRoundsList();
        }
        else if (attributes.ContainsKey("SessionId"))
        {
            var id_string = attributes["SessionId"].ToString();
            if (id_string != null)
            {
                var id = int.Parse(id_string);
                SessionData.SessionId = id;
                FillDataFromId(id);
                UpdateAllFirearmsList();
                UpdateAllRoundsList();
                UpdateGroupData();
            }
        }
        else { }
        attributes.Clear();
    }

    private SessionData SessionData = new();
    private List<FirearmData> AllFirearms = [];
    private List<RoundData> AllRounds = [];

    // firearm selection
    [ObservableProperty]
    ObservableCollection<FirearmData> refinedFirearms = new ObservableCollection<FirearmData>();
    [ObservableProperty]
    bool firearmsInSessionCheckBox = true;
    [ObservableProperty]
    bool showFirearms = false;
    [ObservableProperty]
    string firearmStatusMessage = string.Empty;
    [ObservableProperty]
    string firearmSearchText = string.Empty;
    [ObservableProperty]
    FirearmData? selectedFirearm;
    [ObservableProperty]
    string firearmSearchEntry = string.Empty;

    [RelayCommand]
    void ChangeFirearm()
    {
        ShowFirearms = !ShowFirearms;
    }
    [RelayCommand]
    void SaveFirearm()
    {
        ShowFirearms = false;
    }
    [RelayCommand]
    async void NewFirearm()
    {
        await Shell.Current.GoToAsync("NewFirearmPage");
    }
    [RelayCommand]
    void FirearmsInSessionCheckChanged()
    {
        FilterFirearms();
    }
    [RelayCommand]
    void FirearmSelected()
    {
        if (SelectedFirearm != null && SelectedFirearm.Name != null)
            FirearmSearchEntry = SelectedFirearm.Name;
    }
    [RelayCommand]
    async void FirearmSearchTextChanged()
    {
        await Task.Run(() => FilterFirearms());
    }

    // round selection
    [ObservableProperty]
    ObservableCollection<RoundData> refinedRounds = new ObservableCollection<RoundData>();
    [ObservableProperty]
    bool roundsInQueueCheckBox = true;
    [ObservableProperty]
    bool showRounds = false;
    [ObservableProperty]
    string roundStatusMessage = string.Empty;
    [ObservableProperty]
    string roundSearchText = string.Empty;
    [ObservableProperty]
    RoundData? selectedRound;
    [ObservableProperty]
    string roundSearchEntry = string.Empty;

    [RelayCommand]
    void ChangeRound()
    {
        ShowRounds = !ShowRounds;
    }
    [RelayCommand]
    void SaveRound()
    {
        ShowRounds = false;
    }
    [RelayCommand]
    async void NewRound()
    {
        await Shell.Current.GoToAsync("NewRoundPage");
    }
    [RelayCommand]
    void RoundsInQueueCheckChanged()
    {
        FilterRounds();
    }
    [RelayCommand]
    void RoundSelected()
    {
        if (SelectedRound != null && SelectedRound.Name != null)
            RoundSearchEntry = SelectedRound.Name;
    }
    [RelayCommand]
    async void RoundSearchTextChanged()
    {
        await Task.Run(() => FilterRounds());
    }

    // group section
    [ObservableProperty]
    ObservableCollection<ViewModel.GroupData> groups = [];
    [ObservableProperty]
    bool allowEditGroup = false;
    [ObservableProperty]
    bool allowDeleteGroup = false;
    [ObservableProperty]
    string groupStatusMessage = string.Empty;
    [ObservableProperty]
    GroupData? selectedGroup;

    [RelayCommand]
    async void NewGroup()
    {
        if (SelectedFirearm == null || SelectedRound == null)
        {
            GroupStatusMessage = "Must select a round and a firearm";
            return;
        }
        GroupStatusMessage = "Creating New Group";
        if (!SessionData.Firearms.Contains(SelectedFirearm))
        {
            App.SessionRepo.AddFirearmToSession(
                    FirearmData.GetFirearm(SelectedFirearm), SessionData.SessionId);
        }
        int group_num = Groups.Count + 1;
        string group_name = SessionData.SessionId.ToString() + "-" + (Groups.Count + 1).ToString();
        var group_data = new GroupData
        {
            SessionId = SessionData.SessionId,
            Name = group_name,
            GroupNum = group_num,
            FirearmName = SelectedFirearm.Name,
            FirearmId = SelectedFirearm.Id,
            RoundName = SelectedRound.Name,
            RoundId = SelectedRound.RoundId,

        };
        var navigationParamenter = new Dictionary<string, object>
        {
            {"GroupData", group_data }
        };
        await Shell.Current.GoToAsync("NewGroupPage", navigationParamenter);
    }

    [RelayCommand]
    async void EditGroup()
    {
        if (SelectedGroup == null)
        {
            GroupStatusMessage = "No Group Selected to Edit";
        }
        else
        {
            GroupStatusMessage = string.Format("Editing {0}", SelectedGroup.Name);
            var navigationParamenter = new Dictionary<string, object>
            {
                {"GroupData", SelectedGroup }
            };
            await Shell.Current.GoToAsync("NewGroupPage", navigationParamenter);
        }
    }

    [RelayCommand]
    async void DeleteGroup()
    {
        if (SelectedGroup != null)
        {
            string question = string.Format("Delete {0}?", SelectedGroup.Name);
            // displays a pop up to make sure the user wishes to delete the entry
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                bool response = await Application.Current.MainPage.DisplayAlert(
                        "Alert", question, "Yes", "No");
                if (!response) return;
            }
            App.SessionRepo.DeleteGroup(SelectedGroup.Id);
            Groups.Remove(SelectedGroup);
            GroupStatusMessage = string.Format("Deleted {0}", SelectedGroup.Name);
        }
        else
        {
            GroupStatusMessage = "No Group Selected To Delete";
        }
    }

    [RelayCommand]
    void GroupSelected()
    {
        AllowEditGroup = true;
        AllowDeleteGroup = true;
    }

    // Session Saving
    [ObservableProperty]
    string statusMessage = string.Empty;

    [RelayCommand]
    async void SaveSession()
    {
        try
        {
            StatusMessage = "Saving";
            Preferences.Set("SessionActive", 0);
            await Shell.Current.Navigation.PopToRootAsync();
        }
        catch (Exception e)
        {
            StatusMessage = e.Message;
        }
    }

    [RelayCommand]
    async void DeleteSession()
    {
        string question = "Delete Group";
        // displays a pop up to make sure the user wishes to delete the entry
        if (Application.Current != null && Application.Current.MainPage != null)
        {
            bool response = await Application.Current.MainPage.DisplayAlert(
                    "Alert", question, "Yes", "No");
            if (!response) return;
        }

        var result = App.SessionRepo.DeleteSession(SessionData.SessionId);
        if (result == 0)
        {
            StatusMessage = "Error Deleting " + App.SessionRepo.StatusMessage;
            return;
        }
        Preferences.Set("SessionActive", 0);
        await Shell.Current.GoToAsync("HomePage");

    }

    [RelayCommand]
    async void SaveForLater()
    {
        await Shell.Current.GoToAsync("HomePage");
    }


    [RelayCommand]
    async Task RoundChangeNew()
    {
        await Shell.Current.GoToAsync("NewRoundPage");
    }

    ///<summary>
    /// Gets the group data from the database
    ///</summary>
    private void UpdateGroupData()
    {
        Groups = App.SessionRepo.GetGroupData(SessionData.SessionId);
    }


    ///<summary>
    /// Clears all the status messages
    ///</summary>
    private void ClearStatusMessages()
    {
        GroupStatusMessage = string.Empty;
        FirearmStatusMessage = string.Empty;
        RoundStatusMessage = string.Empty;
        StatusMessage = string.Empty;

        SelectedGroup = null;
        AllowEditGroup = false;
        AllowDeleteGroup = false;

    }

    ///<summary>
    /// Filters the firearm data
    ///</summary>
    private void FilterFirearms()
    {
        RefinedFirearms.Clear();
        if (FirearmsInSessionCheckBox)
        {
            foreach (var firearm in SessionData.Firearms)
            {
                if (firearm.Name != null &&
                        firearm.Name.ToLower().Contains(FirearmSearchEntry.ToLower()))
                    RefinedFirearms.Add(firearm);
            }
        }
        else
        {
            foreach (var firearm in AllFirearms)
            {
                if (firearm.Name != null &&
                        firearm.Name.ToLower().Contains(FirearmSearchEntry.ToLower()))
                    RefinedFirearms.Add(firearm);
            }

        }
    }


    ///<summary>
    /// Filters the round data
    ///</summary>
    private void FilterRounds()
    {
        RefinedRounds.Clear();
        foreach (var round in AllRounds)
        {
            if (round.Name != null && round.Name.ToLower().Contains(RoundSearchEntry.ToLower()))
            {
                if (RoundsInQueueCheckBox)
                {
                    if (round.InQueue == true)
                        RefinedRounds.Add(round);
                }
                else
                {
                    RefinedRounds.Add(round);
                }
            }
        }
    }

    private void UpdateAllRoundsList()
    {
        AllRounds = RoundData.GetData(App.RoundRepo.GetRounds());
        FilterRounds();
    }

    private void UpdateAllFirearmsList()
    {
        AllFirearms = FirearmData.GetData(App.FirearmRepo.GetAllFirearms());
        FilterFirearms();
    }

    ///<summary>
    /// Fills the SessionData object from the database
    ///</summary>
    private void FillDataFromId(int id)
    {
        SessionData = App.SessionRepo.GetSessionData(id);
        if (SessionData.SessionId == -1)
        {
            StatusMessage = "Error Loading Session" + App.SessionRepo.StatusMessage;
        }
        else
        {
            Groups = App.SessionRepo.GetGroupData(SessionData.SessionId);
            StatusMessage = string.Format("Loaded {0} from memory", SessionData.Name);
        }
    }
}
