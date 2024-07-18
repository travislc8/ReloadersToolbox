using System;
using SQLite;

namespace RangeApp.Models;
[Table("FirearmName")]
public class Firearm
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(50), Unique]
    public string? Name { get; set; }
    public int? BarrelLength { get; set; }
    [MaxLength(30)]
    public string? Manufacturer { get; set; }
    [MaxLength(20)]
    public string? Caliber { get; set; }
    [MaxLength(50)] 
    public string? ScopeID { get; set; }
}
