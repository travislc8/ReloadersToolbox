using System; using SQLite;

namespace RangeApp.Models;
[Table("Powder")]

public class Powder
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(50), Unique]
    public string? Name { get; set; }
    [MaxLength (50)]
    public string? PowderManufacturer {  get; set; }
    [MaxLength (50)]
    public string? PowderType { get; set; }
}
