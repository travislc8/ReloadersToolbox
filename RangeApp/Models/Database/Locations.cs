using System; using SQLite;

namespace RangeApp.Models;
[Table("Locations")]

public class Locations
{
    [PrimaryKey, AutoIncrement]
    public int LocationId { get; set; }
    [Unique]
    public string? Name { get; set; }
    public int? ShootingDirection  { get; set; }
}
