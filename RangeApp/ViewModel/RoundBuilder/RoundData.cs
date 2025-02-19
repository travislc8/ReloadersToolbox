using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RangeApp.ViewModel;
public partial class RoundData : ObservableObject
{
    public RoundData()
    {
        RoundId = 0;
    }

    public int RoundId { get; set; }
    public string? Name { get; set; }
    public string? Caliber { get; set; }
    public decimal? PowderWeight { get; set; }
    public string? CaseName { get; set; }
    public string? Primer { get; set; }
    public decimal? TotalLength { get; set; }
    public bool? InQueue { get; set; } = false;
    public int? PowderId { get; set; }
    public string? PowderName { get; set; }
    public string? PowderManufacturer { get; set; }
    public string? PowderType { get; set; }
    public int? BulletId { get; set; }
    public string? BulletName { get; set; }
    public string? BulletCaliber { get; set; }
    public int? BulletGrains { get; set; }
    public string? BulletManufacturer { get; set; }
    public float? AverageVelocity { get; set; }
    public float? AverageStDev { get; set; }

    [RelayCommand]
    public void Test()
    {
        if (InQueue != null)
            App.RoundRepo.UpdateQueue(RoundId, (bool)InQueue);
    }

    public static RoundData GetData(Models.Round round)
    {
        RoundData data = new();

        data.RoundId = round.Id;
        data.Name = round.Name;
        data.Caliber = round.Caliber;
        data.PowderWeight = round.PowderGrains;
        data.CaseName = round.CaseName;
        data.Primer = round.Primer;
        data.TotalLength = round.TotalLength;
        data.InQueue = round.InQueue;
        data.PowderId = round.PowderId;
        data.BulletId = round.BulletId;
        return data;
    }
    public static List<RoundData> GetData(List<Models.Round> rounds)
    {
        List<RoundData> list = [];
        foreach (var round in rounds)
            list.Add(RoundData.GetData(round));
        return list;
    }
}

