using System; using SQLite;

namespace RangeApp.Models;
[Table("Group")]

public class Group
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Unique]
    public string Name { get; set; }
    public int? RoundId { get; set; }
    public int? FirearmId { get; set; }
    [MaxLength(100)]
    public string? Note { get; set; }
    public int? SessionId { get; set; }
    public float? AverageVelocity { get; set; }
    public float? StDev { get; set; }

    public Group()
    {
        Name = Id.ToString();
    }
    public Group(string name)
    {
        Name = name;
    }
}


