using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Converters;

namespace RangeApp.ViewModel;

//TODO 
//Add field input check
public partial class NewGroupPageViewModel : ObservableObject, IQueryAttributable
{
    public NewGroupPageViewModel()
    {
        Shots = [];
        Group = new GroupData();

    }

    private string Unit = "FPS";
    private int ShotSelectedIndex = -1;

    [ObservableProperty]
    string groupStatusMessage = string.Empty;
    [ObservableProperty]
    public string velocityEntry = string.Empty;
    [ObservableProperty]
    public string shotNote = string.Empty;
    [ObservableProperty]
    public string? maxVelocity;
    [ObservableProperty]
    public string? minVelocity;
    [ObservableProperty]
    ObservableCollection<ViewModel.ShotData> shots;
    [ObservableProperty]
    GroupData group;
    [RelayCommand]
    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        if (!attributes.ContainsKey("GroupData"))
            return;
        var group_data = attributes["GroupData"] as GroupData;
        if (group_data != null)
        {
            Group = group_data;
            if (Group.Id != 0)
                Shots = new ObservableCollection<ShotData>(App.SessionRepo.GetGroupShotData(Group.Id));
            UpdateStats();
        }
        else
        {
            Shell.Current.GoToAsync("..");
        }
    }

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
        var shot = new ViewModel.ShotData(0, (Shots.Count + 1), velo, note, note_pre);
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

        if (Shots != null && ShotSelectedIndex != -1)
        {
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
        var shot = new ViewModel.ShotData(Shots[ShotSelectedIndex].Id, Shots.Count, velo, note, note_pre);
        Shots[ShotSelectedIndex] = shot;


        VelocityEntry = string.Empty;
        ShotNote = string.Empty;
        ShotSelectedIndex = -1;
        UpdateStats();
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
            Group.StDev = 0f;
            return;
        }
        float start = Shots[0].Velocity;
        float max = start, min = start, sum = 0;
        foreach (var shot in Shots)
        {
            if (shot.Velocity > max)
                max = shot.Velocity;
            else if (shot.Velocity < min)
                min = shot.Velocity;
            sum += shot.Velocity;

        }
        float avg = sum / Shots.Count;

        double stdev_sum = 0;
        foreach (var shot in Shots)
        {
            stdev_sum += Math.Pow(shot.Velocity - avg, 2);
        }

        Group.StDev = (float)Math.Sqrt(stdev_sum / (Shots.Count - 1));
        Group.AverageVelocity = avg;
        MaxVelocity = max.ToString("F1");
        MinVelocity = min.ToString("F1");
    }
    [RelayCommand]
    public void SaveGroup()
    {
        var group = new Models.Group
        {
            Id = Group.Id,
            Name = Group.Name,
            RoundId = Group.RoundId,
            FirearmId = Group.FirearmId,
            Note = Group.Note,
            SessionId = Group.SessionId,
            AverageVelocity = Group.AverageVelocity,
            StDev = Group.StDev,

        };
        var check = App.SessionRepo.AddGroup(group);
        if (check == 0)
        {
            GroupStatusMessage = App.SessionRepo.StatusMessage;
            return;
        }

        var Group_Id = App.SessionRepo.GetGroupId(Group.Name);
        if (Group_Id == -1)
        {
            GroupStatusMessage = App.SessionRepo.StatusMessage;
            return;
        }

        Models.GroupInSessison group_in_session = new Models.GroupInSessison
        {
            GroupId = Group_Id,
            SessisonId = Group.SessionId
        };

        App.SessionRepo.AddGroupToSession(group_in_session);

        foreach (var shot_data in Shots)
        {
            var shot = new Models.Shot
            {
                RoundId = Group.RoundId,
                GunId = Group.FirearmId,
                GroupId = Group_Id,
                Velocity = shot_data.Velocity,
                Note = shot_data.Note
            };

            App.SessionRepo.AddShot(shot);
        }

        var navigationParamenter = new Dictionary<string, object>
        {
            {"ShotAdded",  check.ToString()}
        };
        Shell.Current.GoToAsync("..", navigationParamenter);
    }
}
