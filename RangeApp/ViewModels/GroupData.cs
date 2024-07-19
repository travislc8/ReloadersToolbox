namespace RangeApp.ViewModel;

public class GroupData
{
    public int Id { get; set; }
    public int? SessionId { get; set; }
    public int? GroupNum { get; set; }
    public string? FirearmName { get; set; }
    public int? FirearmId { get; set; }
    public string? RoundName {  get; set; }
    public int? RoundId {  get; set; }
    public float? AverageVelocity { get; set; }
    public float? StDev { get; set; }
}
