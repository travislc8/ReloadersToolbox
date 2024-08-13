namespace RangeApp.ViewModel;

public class RoundData
{
    public RoundData()
    {
        RoundId = 0;     
    }

    public int RoundId { get; set; }
    public string? Name { get; set; }
    public string? Caliber { get; set; }
    public int? PowderWeight { get; set; }
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
    public string? BulletDiameter { get; set; }
    public int? BulletGrains { get; set; }
    public string? BulletManufacturer { get; set; }
    public float? AverageVelocity { get; set; }
    public float? AverageStDev { get; set; }
   
}
