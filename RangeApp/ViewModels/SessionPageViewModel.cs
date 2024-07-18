using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

//session_id must be initialized
[QueryProperty(nameof(SessionName), "name")]
public partial class SessionPageViewModel : ObservableObject 
{
    public SessionPageViewModel()
    {
        CurrentFirearm = "Test FirearmName";
        CurrentRound = "Test Round";
        GroupData = App.SessionRepo.GetGroupData(session_id);
        RefinedFirearms = [];
        RefinedRounds = [];

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
            if (value != null)
                _SessionName = value;
            session_id = App.SessionRepo.GetSessionIdFromName(SessionName);
            UpdateFirearmData();
            UpdateGroupData();
        }
    }

    private int session_id = 0;
    public GroupData? SelectedGroup;

    [ObservableProperty]
    ObservableCollection<ViewModel.GroupData>? groupData;
    [ObservableProperty]
    ObservableCollection<Models.Firearm>? refinedFirearms;
    [ObservableProperty]
    ObservableCollection<Models.Round>? refinedRounds;
    [ObservableProperty]
    string? currentFirearm;
    [ObservableProperty]
    string? currentRound;

    private void UpdateFirearmData()
    {
        RefinedFirearms = new ObservableCollection<Models.Firearm>(App.SessionRepo.GetFirearmsInSession(session_id));
    }
    public void SetSessionName(string sessionName)
    {
        SessionName = sessionName;
    }
    [RelayCommand]
    //TODO Default pick of firearm
    async public Task NewGroup()
    {
        if (CurrentFirearm == null || CurrentRound == null)
            return;
        //TODO
        int group_num = 0;
        if (GroupData != null)
            group_num = GroupData.Count();
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
        //TODO
    }
    public void DeleteGroup()
    {
        if (SelectedGroup != null)
            App.SessionRepo.DeleteGroup(SelectedGroup.Id);
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
}
