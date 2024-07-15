using System;
using SQLite;

namespace RangeApp.Models;
[Table("Session")]

public class Session
{
    [PrimaryKey, AutoIncrement] 
    public int Id { get; set; }
    [MaxLength(30)]
    public string? Name { get; set; }
    [MaxLength(200)]
    public string? Note { get; set; }
    public double? BarrametricPressure { get; set; }
    public int? WindSpeed { get; set; }
    public int? WindDirection { get; set; }
    public int? Temperature { get; set; }
    public DateTime? Date_Time { get; set; }
    public int LocationId { get; set; }
}
