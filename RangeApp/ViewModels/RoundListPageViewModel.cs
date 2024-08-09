using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
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

    List<ViewModel.RoundData> AllRoundData { get; set; }
    [ObservableProperty]
    ObservableCollection<ViewModel.RoundData> refinedRoundData;
    [ObservableProperty]
    ViewModel.RoundData? selectedRoundData;
    [ObservableProperty]
    string roundSearchEntry = string.Empty;
    [ObservableProperty]
    string searchStatus = string.Empty;

    public void ApplyQueryAttributes(IDictionary<string, object> attributes)
    {
        if (attributes == null)
            return;
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
    }
    private void UpdataRoundData()
    {
        AllRoundData = App.RoundRepo.GetRoundData();
        FilterRoundData();
    }
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
        if (RoundSearchEntry != string.Empty)
        {
            foreach (var item in AllRoundData)
            {
                if (item.Name != null && item.Name.Contains(RoundSearchEntry))
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
        Shell.Current.GoToAsync("NewRoundPage");
    }
    [RelayCommand]
    void EditRound()
    {

        if (SelectedRoundData == null)
            return;
        var NavigationParemeter = new Dictionary<string, object>
        {
            {"RoundData", SelectedRoundData }
        };
        Shell.Current.GoToAsync("NewRoundPage",NavigationParemeter);

    }
    [RelayCommand]
    void DeleteRound()
    {
        if (SelectedRoundData != null)
            App.RoundRepo.DeleteRound(SelectedRoundData.RoundId);
        UpdataRoundData();
        SelectedRoundData = null;
    }
    [RelayCommand]
    async public Task SearchTextChanged()
    {
        string status = new string("");
        Validate.String(RoundSearchEntry, ref status, 10);
        SearchStatus = status;

        await FilterRoundDataAsync();
        return;
    }
    [RelayCommand]
    public void RoundDetailedView()
    {
        //TODO
    }
}
