using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RangeApp.ViewModel;

public partial class TestPageViewModel : ObservableObject
{

	public TestPageViewModel() 
	{
		FirearmListView = new ObservableCollection<RangeApp.Models.Firearm>(App.rangeDayRepo.GetAllFirearms());

	}
	[ObservableProperty]
	ObservableCollection<RangeApp.Models.Firearm> firearmListView;


	[RelayCommand]
	void Add()
	{
		var firearm = new RangeApp.Models.Firearm {Name = "t2" };
		FirearmListView.Add(firearm);
	}

}