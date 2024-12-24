namespace RangeApp.ViewModel;

public class SessionData
{
    public string Name { get; set; }
    public int SessionId { get; set; }
    public string? Note { get; set; }
    public DateTime? Date { get; set; }
    public ViewModel.LocationData Location { get; set; }
    public List<ViewModel.FirearmData> Firearms { get; set; } = [];
    public int? NumShots { get; set; }
    public int? NumGroups { get; set; }

    public SessionData()
    {
        Name = string.Empty;
        SessionId = -1;
        Location = new LocationData();
    }

    public static SessionData GetData(Models.Session session)
    {
        SessionData data = new();

        if (session.Name == null) data.Name = string.Empty;
        else data.Name = session.Name;
        if (session.LocationId == -1) data.Location = new LocationData();
        else
            data.Location = LocationData.GetData(App.LocationRepo.GetLocationFromId(session.LocationId));

        data.SessionId = session.Id;
        data.Note = session.Note;
        data.Date = session.Date_Time;
        data.Firearms = FirearmData.GetData(App.SessionRepo.GetFirearmsInSession(session.Id));
        data.NumShots = App.SessionRepo.GetSessionShotCount(session.Id);
        data.NumGroups = App.SessionRepo.GetGroupCount(session.Id);

        return data;
    }
}
