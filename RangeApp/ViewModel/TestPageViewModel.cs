using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace RangeApp.ViewModel;

public partial class TestPageViewModel : ObservableObject
{
	public TestPageViewModel()
	{
		FirearmListView = new ObservableCollection<RangeApp.Models.Firearm>(App.FirearmRepo.GetAllFirearms());
		var list = App.SessionRepo.GetFirearmsInSession(6);
		count = list.Count;
	}
	[ObservableProperty]
	ObservableCollection<RangeApp.Models.Firearm> firearmListView;
	[ObservableProperty]
	public int count;


	[RelayCommand]
	void Add()
	{
		var firearm = new RangeApp.Models.Firearm { Name = "t2" };
		FirearmListView.Add(firearm);
	}
	public void TestMethod()
	{

	}

}