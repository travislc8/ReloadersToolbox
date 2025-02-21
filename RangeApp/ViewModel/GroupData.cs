namespace RangeApp.ViewModel;

public class GroupData
{
    public int Id { get; set; } = 0;
    /// <summary> SessionId + "-" + (Number in group)</summary>
    public string Name { get; set; } = string.Empty;
    public int? SessionId { get; set; }
    public string? SessionName { get; set; }
    public int? GroupNum { get; set; }
    public string? Note { get; set; }
    public string? FirearmName { get; set; }
    public int? FirearmId { get; set; }
    public string? RoundName { get; set; }
    public int? RoundId { get; set; }
    public float? AverageVelocity { get; set; }
    public float? StDev { get; set; }
    public decimal? GroupSize { get; set; }


    async public static Task<GroupData> GetGroupData(RangeApp.Models.Group group)
    {
        var data = new GroupData()
        {
            Id = group.Id,
            Name = group.Name,
            SessionId = group.SessionId,
            GroupNum = 0,
            Note = group.Note,
            FirearmId = group.FirearmId,
            FirearmName = await Task.Run(() => App.FirearmRepo.GetFirearmNameFromId(group.FirearmId)),
            RoundId = group.RoundId,
            RoundName = await Task.Run(() => App.RoundRepo.GetRoundNameFromId(group.RoundId)),
            GroupSize = group.GroupSize,
            StDev = group.StDev,
            AverageVelocity = group.AverageVelocity,
        };
        var session = await Task.Run(() => App.SessionRepo.GetSessionNameFromId(group.SessionId));
        if (session != null) data.SessionName = session.Name;
        return data;
    }
}

