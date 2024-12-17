using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace RangeApp.ViewModel;

public partial class DayEntryViewModel : ObservableObject
{

    public DayEntryViewModel()
    {
        text = "";
    }
    [ObservableProperty]
    string text;

}