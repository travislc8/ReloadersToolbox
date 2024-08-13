using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using RangeApp.Models;
using System.ComponentModel.Design;

namespace RangeApp.ViewModel;

//session_id must be initialized
public partial class SessionPageViewModel : ObservableObject , IQueryAttributable
{
    public SessionPageViewModel()
    {
        CurrentFirearm = string.Empty;
        CurrentRound = string.Empty;
        GroupData = App.SessionRepo.GetGroupData(session_id);
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
    ObservableCollection<ViewModel.GroupData>? groupData;
    [ObservableProperty]
    ObservableCollection<Models.Firearm>? refinedFirearms;
    [ObservableProperty]
    ObservableCollection<Models.Round> refinedRounds = new ObservableCollection<Round>();
    [ObservableProperty]
    string? currentFirearm;
    [ObservableProperty]
    string? currentRound;
    [ObservableProperty]
    string firearmSearchEntry = string.Empty;
    [ObservableProperty]
    string roundSearchEntry = string.Empty;
    [ObservableProperty]
    bool roundsInTestQueue = false;

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
        if (attributes.ContainsKey("nameEntry"))
        {
            _SessionName = attributes["nameEntry"] as string;
            session_id = App.SessionRepo.GetSessionIdFromName(SessionName);
            UpdateAllFirearmsList();
            UpdateGroupData();
            if (RefinedFirearms != null && RefinedFirearms.Count != 0)
                CurrentFirearm = RefinedFirearms[0].Name;
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
        attributes.Clear();
    }
    [RelayCommand]
    async public Task NewGroup()
    {
        if (CurrentFirearm == null || CurrentRound == null)
            return;
        App.SessionRepo.AddFirearmToSession(CurrentFirearm, session_id);
        int group_num = 0;
        if (GroupData != null)
            group_num = GroupData.Count;
        session_id = App.SessionRepo.GetSessionIdFromName(SessionName);
        var group_data = new GroupData
        {
            SessionId = session_id,
            GroupNum = group_num,
            FirearmName = CurrentFirearm,
            FirearmId = App.FirearmRepo.GetFirearmFromName(CurrentFirearm).Id,
            RoundName = CurrentRound,
            RoundId = App.SessionRepo.GetRoundIdFromName(CurrentRound),

        };
        var navigationParamenter = new Dictionary<string, object>
        {
            {"GroupData", group_data }
        };
        await Shell.Current.GoToAsync("NewGroupPage", navigationParamenter);
        UpdateGroupData();
    }
    [RelayCommand]
    async Task RoundChangeNew()
    {
        await Shell.Current.GoToAsync("NewRoundPage");
    }
    private void UpdateGroupData()
    {
        GroupData = App.SessionRepo.GetGroupData(session_id);
    }
    public void ChangeFirearm()
    {

        //Doesn't need to do anything?
    }
    public void ChangeRound()
    {
        //Doesn't need to do anything?
    }
    async public void EditGroup()
    {
        if (CurrentFirearm == null || CurrentRound == null)
            return;
        App.SessionRepo.AddFirearmToSession(CurrentFirearm, session_id);
        int group_num = 0;
        if (GroupData != null)
            group_num = GroupData.Count;
        session_id = App.SessionRepo.GetSessionIdFromName(SessionName);
        var group_data = new GroupData
        {
            SessionId = session_id,
            GroupNum = group_num,
            FirearmName = CurrentFirearm,
            FirearmId = App.FirearmRepo.GetFirearmFromName(CurrentFirearm).Id,
            RoundName = CurrentRound,
            RoundId = App.SessionRepo.GetRoundIdFromName(CurrentRound),

        };
        var navigationParamenter = new Dictionary<string, object>
        {
            {"GroupData", group_data }
        };
        await Shell.Current.GoToAsync("NewGroupPage", navigationParamenter);
        UpdateGroupData();

    }
    public void DeleteGroup()
    {
        if (SelectedGroup != null)
            App.SessionRepo.DeleteGroup(SelectedGroup.Id);
        UpdateGroupData();
    }
    public void SetCurrentFirearm(int index)
    {
        if (RefinedFirearms != null)
            CurrentFirearm = RefinedFirearms[index].Name;
    }
    public void SetCurrentRound(int index)
    {
        if(RefinedRounds != null) 
            CurrentRound = RefinedRounds[index].Name;
    }

    public void SetSelectedGroup(int index)
    {
        if (GroupData != null)
            SelectedGroup = GroupData[index];
    }
    private void GetGroupData()
    {
                
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
}
