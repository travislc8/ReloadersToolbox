using System; using SQLite;

namespace RangeApp.Models;
[Table("Location")]

public class Location
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Unique]
    public string? Name { get; set; }
    public int? ShootingDirection  { get; set; }
}
