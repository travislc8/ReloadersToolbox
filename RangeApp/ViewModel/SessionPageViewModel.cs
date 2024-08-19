using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using RangeApp.Models;
using System.ComponentModel.Design;

namespace RangeApp.ViewModel;

//session_id must be initialized
public partial class SessionPageViewModel : ObservableObject, IQueryAttributable
{
    public SessionPageViewModel()
    {
        groups = [];
        RefinedFirearms = [];
        UpdateAllRoundsList();
        RefinedRounds = [];
        UpdateAllFirearmsList();
    }

    private string _SessionName;
    public string SessionName
    {
        get
        {
            return _SessionName;
        }
        set
        {
            _SessionName = value;
        }
    }

    private int session_id = 0;
    public GroupData? SelectedGroup;
    private List<Firearm> AllFirearms;
    private List<Round> AllRounds;
    private bool FirearmsInSessionChecked = true;

    [ObservableProperty]
    ObservableCollection<ViewModel.GroupData> groups;
    [ObservableProperty]
    ObservableCollection<Models.Firearm> refinedFirearms = new ObservableCollection<Firearm>();
    [ObservableProperty]
    ObservableCollection<Models.Round> refinedRounds = new ObservableCollection<Round>();
    [ObservableProperty]
    Models.Firearm? selectedFirearm;
    [ObservableProperty]
    Models.Round? selectedRound;
    [ObservableProperty]
    string firearmSearchEntry = string.Empty;
    [ObservableProperty]
    string roundSearchEntry = string.Empty;
    [ObservableProperty]
    bool roundsInTestQueue = false;
    [ObservableProperty]
    string groupStatusMessage = string.Empty;

    public void SetSessionName(string sessionName)
    {
        SessionName = sessionName;
    }
    [RelayCommand]
    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        if (attributes == null)
            return;
        if (attributes.ContainsKey("firearm"))
        {
            var firearm = attributes["firearm"] as Models.Firearm;
            if (firearm == null)
                return;
            App.SessionRepo.AddFirearmToSession(firearm, session_id);
            UpdateAllFirearmsList();
        }
        if (attributes.ContainsKey("NameEntry"))
        {
            _SessionName = attributes["NameEntry"] as string;
            session_id = App.SessionRepo.GetSessionIdFromName(SessionName);
            Preferences.Set("SessionActive", session_id);
            UpdateGroupData();
            if (RefinedFirearms != null && RefinedFirearms.Count != 0)
                SelectedFirearm = RefinedFirearms[0];
        }

        if (attributes.ContainsKey("ShotAdded"))
        {
            if ((attributes["ShotAdded"] as string) != "0")
                UpdateGroupData();
        }
        if (attributes.ContainsKey("AddedRound"))
        {
            UpdateAllRoundsList();
        }
        if (attributes.ContainsKey("SessionId"))
        {
            var id_string = attributes["SessionId"].ToString();
            if (id_string != null)
            {
                var id = int.Parse(id_string);
                session_id = id;
                Preferences.Set("SessionActive", session_id);
                FillDataFromId(id);
                UpdateGroupData();
                App.SessionRepo.GetGroupCount(session_id);
            }
        }
        attributes.Clear();
    }
    [RelayCommand]
    async public Task NewGroup()
    {
        if (SelectedFirearm == null || SelectedRound == null)
        {
            GroupStatusMessage = "Must select a round and a firearm";
            return;
        }
        GroupStatusMessage = "";
        App.SessionRepo.AddFirearmToSession(SelectedFirearm, session_id);
        int group_num = Groups.Count + 1;
        string group_name = session_id.ToString() + "-" + (Groups.Count + 1).ToString();
        var group_data = new GroupData
        {
            SessionId = session_id,
            Name = group_name,
            GroupNum = group_num,
            FirearmName = SelectedFirearm.Name,
            FirearmId = SelectedFirearm.Id,
            RoundName = SelectedRound.Name,
            RoundId = SelectedRound.Id,

        };
        var navigationParamenter = new Dictionary<string, object>
        {
            {"GroupData", group_data }
        };
        await Shell.Current.GoToAsync("NewGroupPage", navigationParamenter);
    }
    [RelayCommand]
    async Task RoundChangeNew()
    {
        await Shell.Current.GoToAsync("NewRoundPage");
    }
    [RelayCommand]
    void FinishSession()
    {

        Preferences.Set("SessionActive", 0);
        var NavigationParameter = new Dictionary<string, object> {
            {"SessionId", session_id}
        };
        Shell.Current.GoToAsync("..", NavigationParameter);
    }
    private void UpdateGroupData()
    {
        Groups = App.SessionRepo.GetGroupData(session_id);
    }
    public void ChangeFirearm()
    {

        //Doesn't need to do anything?
    }
    async public void EditGroup()
    {
        if (SelectedGroup == null)
            return;
        var navigationParamenter = new Dictionary<string, object>
        {
            {"GroupData", SelectedGroup }
        };
        await Shell.Current.GoToAsync("NewGroupPage", navigationParamenter);
    }
    public void DeleteGroup()
    {
        if (SelectedGroup != null)
            App.SessionRepo.DeleteGroup(SelectedGroup.Id);
        UpdateGroupData();
    }
    public void SetSelectedGroup(int index)
    {
        SelectedGroup = Groups[index];
    }
    public void FirearmSearchEntryTextChanged()
    {
        UpdateRefinedFirearmData();
    }
    public void FirearmsInSessionCheckBoxChanged(bool is_checked)
    {
        FirearmsInSessionChecked = is_checked;
        UpdateAllFirearmsList();
    }
    public void RoundSearchEntryTextChanged()
    {
        UpdateRefinedRoundData();
    }
    public void RoundsInTestQueueChanged()
    {
        UpdateRefinedRoundData();
    }

    private void UpdateAllRoundsList()
    {
        AllRounds = App.RoundRepo.GetRounds();
        UpdateRefinedRoundData();
    }
    private void UpdateRefinedRoundData()
    {
        if (RoundSearchEntry == string.Empty && RoundsInTestQueue == false)
        {
            RefinedRounds = new ObservableCollection<Round>(AllRounds);
            return;
        }

        RefinedRounds.Clear();
        foreach (var round in AllRounds)
        {
            if (RoundsInTestQueue == true)
            {
                if (round.InQueue != true)
                    continue;
            }
            if (round.Name != null && round.Name.Contains(RoundSearchEntry))
                RefinedRounds.Add(round);
        }
    }
    public void UpdateAllFirearmsList()
    {
        if (FirearmsInSessionChecked)
        {
            AllFirearms = App.SessionRepo.GetFirearmsInSession(session_id);
        }
        else
        {
            AllFirearms = App.FirearmRepo.GetAllFirearms();
        }
        UpdateRefinedFirearmData();
    }

    private void UpdateRefinedFirearmData()
    {
        if (RefinedFirearms == null)
        {
            throw new Exception("Refined Firearm List is not initiated");
        }
        if (AllFirearms == null)
        {
            throw new Exception("All FirearmList is not initiated");
        }
        if (FirearmSearchEntry == string.Empty)
        {
            RefinedFirearms = new ObservableCollection<Firearm>(AllFirearms);
        }
        else
        {
            RefinedFirearms.Clear();
            foreach (Firearm firearm in AllFirearms)
            {
                if (firearm.Name != null && firearm.Name.Contains(FirearmSearchEntry))
                {
                    RefinedFirearms.Add(firearm);
                }
            }
        }
    }
    private void FillDataFromId(int id)
    {
        SessionName = App.SessionRepo.GetSessionNameFromId(id);
        UpdateAllFirearmsList();
        UpdateAllRoundsList();
        Groups = App.SessionRepo.GetGroupData(session_id);
    }
}
