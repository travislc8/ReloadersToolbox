using System;
using SQLite;

namespace RangeApp.Models;
[Table("Shot")]

public class Shot
{
    [PrimaryKey, AutoIncrement]
    public int? Id { get; set; }
    public int? RoundId { get; set; }
    public int? GunId { get; set; }
    public int? GroupId { get; set; }
    public int? Velocity { get; set; }
    public Double? X_Value { get; set; }
    public Double? Y_Value { get; set; }
    [MaxLength(100)]
    public string? Note { get; set; }
}
