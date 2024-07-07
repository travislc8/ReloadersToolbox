using System;
using SQLite;

namespace RangeApp.Models;
[Table("FirearmToRound")]
public class FirearmToRound
{
    [PrimaryKey]
    public int FirearmToRoundId { get; set; }  
    public int FirearmId { get; set; }
    public string RoundId { get; set; }
}
