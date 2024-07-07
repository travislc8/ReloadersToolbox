using System;
using SQLite;

namespace RangeApp.Models;
[Table("Session")]

internal class Session
{
    [PrimaryKey, AutoIncrement] 
    public int Id { get; set; }
    [MaxLength(100)]
    public string? Note { get; set; }
    public double? BarrametricPressure { get; set; }
    public int? WindSpeed { get; set; }
    public int? WindDirection { get; set; }
    public int? Temperature { get; set; }
    public int? Day { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public int? Time { get; set; }
    public int LocationId { get; set; }
}
