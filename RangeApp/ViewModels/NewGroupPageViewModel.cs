using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Converters;

namespace RangeApp.ViewModel;

//TODO 
//Add field input check
public partial class NewGroupPageViewModel :ObservableObject, IQueryAttributable 
{
    public NewGroupPageViewModel()
    {
        Shots = [];
        GroupName = string.Empty;

    }
    //Parameters: group_id = unique id for group (session_id + "-" + group_num)
    public NewGroupPageViewModel(string group_id) 
    {
        Shots = [];
        GroupName = group_id;
    }

    [RelayCommand]
    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        var group_data = attributes["GroupData"] as GroupData;
        if (group_data == null)
            return;

        if (group_data.SessionId != null)
            Session_Id = (int)group_data.SessionId;
        if (group_data.GroupNum != null)
            GroupName = group_data.GroupNum.ToString() + group_data.SessionId.ToString();
        if (group_data.FirearmName != null)
            FirearmName = group_data.FirearmName;
        if (group_data.RoundName != null)
            RoundName = group_data.RoundName;
        if (group_data.RoundId != null)
            Round_Id = (int)group_data.RoundId;
        if (group_data.FirearmId != null)
            Firearm_Id = (int)group_data.FirearmId;
    }
    private string Unit = "FPS";
    private int ShotSelectedIndex = -1;
    public string GroupName;
    private bool EditGroupMode = false;

    [ObservableProperty]
    public string firearmName = string.Empty;
    [ObservableProperty]
    public string roundName = string.Empty;
    [ObservableProperty]
    public int firearm_Id = -1;
    [ObservableProperty]
    public int session_Id = -1;
    [ObservableProperty]
    public int round_Id = -1;
    [ObservableProperty]
    public string velocityEntry = string.Empty;
    [ObservableProperty]
    public string shotNote = string.Empty;
    [ObservableProperty]
    public string? maxVelocity;
    [ObservableProperty]
    public string? minVelocity;
    [ObservableProperty]
    public string? averageVelocity;
    [ObservableProperty]
    public string? stDev;
    [ObservableProperty]
    public string? groupNote;
    [ObservableProperty]
    ObservableCollection<ViewModel.ShotData> shots;
    
    public void UnitChanged(string unit)
    {
        Unit = unit;
    }
    public void ShotSelected(int index)
    {
        ShotSelectedIndex = index;
    }

    [RelayCommand] 
    public void AddShot()
    {
        if (VelocityEntry == string.Empty)
            return;
        float velo = 0;
        string note = string.Empty;
        string note_pre = string.Empty;
        if (VelocityEntry != string.Empty)
            velo = float.Parse(VelocityEntry);
        if (Unit == "MPS")
            velo = velo * 3.281f;
        if (ShotNote != string.Empty)
        {
            note_pre = "Note: "; 
            note = ShotNote;
        }
        var shot = new ViewModel.ShotData((Shots.Count + 1), velo, note, note_pre); 
        Shots.Add(shot);
        UpdateStats();


        VelocityEntry = string.Empty;
        ShotNote = string.Empty;

    }
    public bool EditShot()
    {

        if (Shots != null && ShotSelectedIndex != -1)
        {
            string note = Shots[ShotSelectedIndex].Note;
            VelocityEntry = Shots[ShotSelectedIndex].Velocity.ToString();
            ShotNote = note;
            UpdateStats();
            return true;
        }
        return false;
    }
    [RelayCommand]
    public void DeleteShot()
    {

        if (Shots != null && ShotSelectedIndex != -1) {
            Shots.RemoveAt(ShotSelectedIndex);
            UpdateStats();
        }
        ShotSelectedIndex = -1;
    }
    
    public void UpdateShot()
    {
        float velo = 0;
        string note = string.Empty;
        string note_pre = string.Empty;
        if (VelocityEntry != string.Empty)
            velo = float.Parse(VelocityEntry);
        if (ShotNote != string.Empty)
        {
            note_pre = "Note: "; 
            note = ShotNote;
        }
        var shot = new ViewModel.ShotData(Shots.Count, velo, note, note_pre); 
        Shots[ShotSelectedIndex] = shot;


        VelocityEntry = string.Empty;
        ShotNote = string.Empty;
        ShotSelectedIndex = -1;
    }
    public void CancelUpdateShot()
    {
        VelocityEntry = "";
        ShotNote = "";
    }
    public void UpdateStats()
    {
        if (Shots.Count == 0)
        {
            MaxVelocity = string.Empty;
            MinVelocity = string.Empty;
            StDev = string.Empty;
            return;
        }
        float start = Shots[0].Velocity;
        float max = start, min = start, sum = 0;
        foreach (var shot in Shots )
        {
            if (shot.Velocity > max)
                max = shot.Velocity;
            else if (shot.Velocity < min)
                min = shot.Velocity;
            sum += shot.Velocity;

        }
        float avg = sum / Shots.Count;

        double stdev_sum = 0;
        foreach (var shot in Shots )
        {
            stdev_sum += Math.Pow(shot.Velocity - avg, 2);
        }

        double dStdev = Math.Sqrt(stdev_sum / (Shots.Count - 1)); 
        MaxVelocity = max.ToString("F1");
        MinVelocity = min.ToString("F1");
        AverageVelocity = avg.ToString("F1");
        StDev = dStdev.ToString("F1");
    }
    [RelayCommand]
    public void SaveGroup()
    {
        //Group
        float avg = 0;
        if (AverageVelocity != string.Empty && AverageVelocity != null)
            avg = float.Parse(AverageVelocity);
        float stdev = 0;
        if (StDev != string.Empty && StDev != null)
            stdev = float.Parse(StDev);
        var group = new Models.Group
        {
            RoundId = Round_Id,
            FirearmId = Firearm_Id,
            SessionId = Session_Id,
            Note = GroupNote,
            AverageVelocity = avg,
            StDev = stdev,
            
        };
        if (GroupName != string.Empty)
            group.Name = GroupName;
        App.SessionRepo.AddGroup(group);

        var Group_Id = App.SessionRepo.GetGroupId(GroupName);
        if (Group_Id == -1)
        {
            return;
        }

        Models.GroupInSessison group_in_session = new Models.GroupInSessison
        {
            GroupId = Group_Id,
            SessisonId = Session_Id
        };

        App.SessionRepo.AddGroupToSession(group_in_session);

        int result = 0;
        foreach (var shot_data in Shots)
        {
            var shot = new Models.Shot
            {
                RoundId = Round_Id,
                GunId = Firearm_Id,
                GroupId = Group_Id,
                Velocity = shot_data.Velocity,
                Note = shot_data.Note
            };

            result = App.SessionRepo.AddShot(shot);
        }
        
        var navigationParamenter = new Dictionary<string, object>
        {
            {"ShotAdded",  result.ToString()}
        };
        Shell.Current.GoToAsync("..", navigationParamenter);
    }
}
