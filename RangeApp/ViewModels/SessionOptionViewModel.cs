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
		AvailableLocations = new ObservableCollection<Models.Location>(App.LocationRepo.GetAllLocations());
		SessionNames = new ObservableCollection<string>(App.SessionRepo.GetSessionNames());
	}
	private int LocationId = -1;
	private string SessionName;
	[ObservableProperty]
	ObservableCollection<RangeApp.Models.Firearm> firearmsInSession;
	[ObservableProperty]
	ObservableCollection<RangeApp.Models.Firearm> availableFirearms;
	[ObservableProperty]
	int availableFirearmIndex = -1;
	[ObservableProperty]
	ObservableCollection<RangeApp.Models.Location> availableLocations;
	[ObservableProperty]
	int availableLocaitonIndex = -1;
	[ObservableProperty]
	string statusMessage = string.Empty;
	[ObservableProperty]
	ObservableCollection<string> sessionNames;
	[ObservableProperty]
	string firearmPickerPlaceholder = "Select a FirearmName From Existing";


	public void SetSessionName(string sessionName)
	{
		SessionName = sessionName;
	}
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
	}
	partial void OnAvailableLocaitonIndexChanged(int value)
	{
		StatusMessage = "";
		if (AvailableLocaitonIndex != -1)
			if (AvailableLocaitonIndex != -1)
                LocationId = AvailableLocations[AvailableLocaitonIndex].Id;
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

	public bool NameDuplicateCheck(string name)
	{
		return SessionNames.Contains(name);
	}
	public void Save(string location_name, string note)
	{

		var session = new Models.Session
		{
			LocationId = LocationId,
			Note = note,
			Id = LocationId,
			Name = SessionName,
		};
		App.SessionRepo.AddSession(session);
		var session_id = App.SessionRepo.GetSessionIdFromName(SessionName);
		App.SessionRepo.AddFirearmsToSession(FirearmsInSession.ToList(), session_id);
		StatusMessage = App.SessionRepo.StatusMessage;
	}
	public void UpdateLocations()
	{
		AvailableLocations = new ObservableCollection<Models.Location>(App.LocationRepo.GetAllLocations());
	}
	public void UpdateFirearms()
	{
		AvailableFirearms = new ObservableCollection<RangeApp.Models.Firearm>(App.FirearmRepo.GetAllFirearms());
	}
}
