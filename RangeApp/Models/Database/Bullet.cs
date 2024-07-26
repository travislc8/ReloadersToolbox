using System; using SQLite;

namespace RangeApp.Models;
[Table("Bullet")]

public class Bullet
{
    [PrimaryKey, AutoIncrement] 
    public int Id { get; set; }
    [MaxLength(50),Unique]
    public string? Name { get; set; }
    [MaxLength(20)]
    public string? Caliber { get; set; }
    public int? BulletGrains { get; set; }
    [MaxLength(50)]
    public string? BulletManufacturer { get; set; }
}
