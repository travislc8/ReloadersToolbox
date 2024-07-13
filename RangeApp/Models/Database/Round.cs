using System; using SQLite;

namespace RangeApp.Models;
[Table("Round")]

internal class Round
{
    [PrimaryKey, AutoIncrement] 
    public int Id { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(20)]
    public string? Caliber { get; set; }
    public int? BulletGrains { get; set; }
    [MaxLength(30)]
    public string? BulletManufacturer { get; set; }
    public int? PowderGrains { get; set; }
    [MaxLength(30)]
    public string? PowderManufacturer {  get; set; }
    [MaxLength(50)]
    public string? CaseName { get; set; }
    [MaxLength (50)]
    public string? Primer { get; set; }
    public Decimal? Jump { get; set; }
    public bool? Tested { get; set; }
}
