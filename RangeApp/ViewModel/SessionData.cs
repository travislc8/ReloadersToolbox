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
}
