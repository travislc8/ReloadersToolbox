using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;

public partial class SessionOptionsViewModel : ObservableObject
{
	public SessionOptionsViewModel()
	{
		FirearmsInSession = new ObservableCollection<Models.Firearm>();
		AvailableFirearms = new ObservableCollection<RangeApp.Models.Firearm>(App.FirearmRepo.GetAllFirearms());

	}
	[ObservableProperty]
	ObservableCollection<RangeApp.Models.Firearm> firearmsInSession;
	[ObservableProperty]
	ObservableCollection<RangeApp.Models.Firearm> availableFirearms;
	[ObservableProperty]
	int availableFirearmIndex = -1;
	[ObservableProperty]
	string statusMessage = string.Empty;


	public void AddFirearmToSession(Models.Firearm firearm)
	{
		FirearmsInSession.Add(firearm);
		AvailableFirearms.Add(firearm);
	}
	partial void OnAvailableFirearmIndexChanged(int value)
	{
		StatusMessage = "";
		if (AvailableFirearmIndex != -1)
			FirearmsInSession.Add(AvailableFirearms[AvailableFirearmIndex]);
		StatusMessage = App.FirearmRepo.StatusMessage;
	}
	public void SetStatusMessage(string message)
	{
		StatusMessage = message;
	}
	public void AddToFirearmsInSession (string name)
	{
		var firearm = App.FirearmRepo.GetFirearmFromName(name);
		AddFirearmToSession (firearm);
	}

}
