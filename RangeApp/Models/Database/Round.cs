using System; using SQLite;

namespace RangeApp.Models;
[Table("Round")]

public class Round
{
    [PrimaryKey, AutoIncrement] 
    public int Id { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; }
    public int? BulletId { get; set; }
    [MaxLength(20)]
    public string? Caliber { get; set; }
    public int? PowderGrains { get; set; }
    public int? PowderId { get; set; }
    [MaxLength(50)]
    public string? CaseName { get; set; }
    [MaxLength (50)]
    public string? Primer { get; set; }
    public decimal? TotalLength { get; set; }
    public bool? InQueue { get; set; }
}
