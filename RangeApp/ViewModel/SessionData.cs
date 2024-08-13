namespace RangeApp.ViewModel;

public class SessionData
{
    public string? Name { get; set; }
    public int SessionId { get; set; } = 0;
    public string? Note { get; set; }
    public DateTime? Date { get; set; }
    public Models.Location? Location { get; set; }
    public List<Models.Firearm> Firearms { get; set; } = [];
    public int? NumShots { get; set; }
    public int? NumGroups { get; set; }

}
