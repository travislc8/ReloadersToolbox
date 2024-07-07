using System; using SQLite;

namespace RangeApp.Models;
[Table("Group")]

internal class Group
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int? RoundId { get; set; }
    public int? FirearmId { get; set; }
}
