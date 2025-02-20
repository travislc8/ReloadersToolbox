using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Model.Utils;

namespace RangeApp.ViewModel;

public partial class RoundListPageViewModel : ObservableObject, IQueryAttributable
{
    public RoundListPageViewModel()
    {
        UpdataRoundData();
    }

    //groups
    //selected Group
    //GroupSelectedCommand

    List<ViewModel.RoundData> AllRoundData { get; set; } = new List<ViewModel.RoundData>();

    [ObservableProperty]
    ObservableCollection<ViewModel.RoundData> refinedRoundData = new ObservableCollection<ViewModel.RoundData>();
    [ObservableProperty]
    ViewModel.RoundData? selectedRoundData;
    [ObservableProperty]
    string roundSearchEntry = string.Empty;
    [ObservableProperty]
    string searchStatus = string.Empty;
    [ObservableProperty]
    string statusMessage = string.Empty;
    [ObservableProperty]
    string roundStatusMessage = string.Empty;

    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        RoundStatusMessage = "";
        StatusMessage = "";
        if (attributes == null)
            return;
        // catches return from NewRoundPage
        if (attributes.ContainsKey("AddedRound"))
        {
            var result = attributes["AddedRound"] as int?;
            if (result != 0)
            {
                UpdataRoundData();
                SelectedRoundData = null;
                foreach (var item in RefinedRoundData)
                {
                    if (item.RoundId == result)
                        SelectedRoundData = item;
                }
            }
        }
        if (attributes.ContainsKey("ShotAdded"))
        {
            string result = attributes["ShotAdded"] as string;
            if (result == "0")
            {
                GroupStatusMessage = "";
            }
            else
            {
                GetGroups();
                GroupStatusMessage = "Edited Group";
            }
        }
    }

    ///<summary>
    /// Gets the Rounds from the database and updates the displayed list
    ///</summary>
    private void UpdataRoundData()
    {
        AllRoundData = App.RoundRepo.GetRoundData();
        FilterRoundData();
    }

    ///<summary>
    /// Filters the displayed list of rounds based on search
    ///</summary>
    private void FilterRoundData()
    {
        if (RoundSearchEntry == string.Empty)
        {
            RefinedRoundData = new ObservableCollection<RoundData>(AllRoundData);
            return;
        }
        RefinedRoundData.Clear();
        foreach (var item in AllRoundData)
        {
            if (item.Name != null && item.Name.Contains(RoundSearchEntry))
            {
                RefinedRoundData.Add(item);
            }
        }
    }
    private Task FilterRoundDataAsync()
    {
        RefinedRoundData.Clear();
        string search_text = RoundSearchEntry.ToLower();
        if (RoundSearchEntry != string.Empty)
        {
            foreach (var item in AllRoundData)
            {
                if (item.Name != null && item.Name.ToLower().Contains(search_text))
                {
                    RefinedRoundData.Add(item);
                }
            }
        }
        else
        {
            RefinedRoundData = new ObservableCollection<RoundData>(AllRoundData);
        }
        return Task.CompletedTask;
    }

    [RelayCommand]
    void NewRound()
    {
        RoundStatusMessage = "Creating a new round.";
        Shell.Current.GoToAsync("NewRoundPage");
    }

    [RelayCommand]
    void EditRound()
    {
        if (SelectedRoundData == null)
            return;
        RoundStatusMessage = "Editing " + SelectedRoundData.Name;
        var NavigationParemeter = new Dictionary<string, object>
        {
            {"RoundData", SelectedRoundData }
        };
        Shell.Current.GoToAsync("NewRoundPage", NavigationParemeter);

    }

    [RelayCommand]
    async void DeleteRound()
    {
        if (SelectedRoundData == null)
        {
            RoundStatusMessage = "No round selected to delete";
            return;
        }
        string question = "Delete " + SelectedRoundData.Name + "?";

        // displays a pop up to make sure the user wishes to delete the entry
        if (Application.Current != null && Application.Current.MainPage != null)
        {
            bool response = await Application.Current.MainPage.DisplayAlert(
                    "Alert", question, "Yes", "No");
            if (!response)
                return;
        }

        int result = App.RoundRepo.DeleteRound(SelectedRoundData.RoundId);
        if (result == 0)
        {
            RoundStatusMessage = "Could not Delete " + SelectedRoundData.Name;
            return;
        }

        RoundStatusMessage = "Deleted " + SelectedRoundData.Name;
        UpdataRoundData();
        SelectedRoundData = null;
    }

    [RelayCommand]
    async public Task SearchTextChanged()
    {
        string status = string.Empty;
        Validate.String(RoundSearchEntry, ref status, 10);
        SearchStatus = status;

        await FilterRoundDataAsync();
        return;
    }

    [RelayCommand]
    async public Task RoundDetailedView()
    {
        GetGroups();
        ShowGroups = true;
    }

    [RelayCommand]
    public void InQueueChanged()
    {
        foreach (var item in RefinedRoundData)
        {
            if (item.InQueue != null)
            {
                App.RoundRepo.UpdateQueue(item.RoundId, (bool)item.InQueue);
            }
        }
    }


    // group section
    [ObservableProperty]
    ObservableCollection<ViewModel.GroupData> groups = [];

    [ObservableProperty]
    bool showGroups = false;
    [ObservableProperty]
    bool allowEditGroup = false;
    [ObservableProperty]
    bool allowDeleteGroup = false;
    [ObservableProperty]
    string groupStatusMessage = string.Empty;
    [ObservableProperty]
    GroupData? selectedGroup;
    [ObservableProperty]
    int groupCount = 0;
    [ObservableProperty]
    decimal avgGroupSize = 0;


    [RelayCommand]
    async Task EditGroup()
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
    async Task DeleteGroup()
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

    void GetGroups()
    {
        if (SelectedRoundData == null) return;

        Groups = App.SessionRepo.GetGroupsFromRound(SelectedRoundData.RoundId);
        GroupCount = Groups.Count;
        if (GroupCount < 1) return;
        decimal sum = 0;
        foreach (var group in Groups)
        {
            if (group.GroupSize != null)
                sum += (decimal)group.GroupSize;
        }
        AvgGroupSize = sum / GroupCount;
    }
}
