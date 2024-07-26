using System;
using SQLite;

namespace RangeApp.Models;
[Table("FirearmToRound")]
public class FirearmToRound
{
    [PrimaryKey, AutoIncrement]
    public int FirearmToRoundId { get; set; }  
    public int FirearmId { get; set; }
    public int RoundId { get; set; }
}
