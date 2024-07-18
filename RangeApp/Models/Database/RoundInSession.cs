using System;
using SQLite;

namespace RangeApp.Models;
[Table("RoundInSession")]
public class RoundInSession
{
    [PrimaryKey,AutoIncrement]
    public int RoundInSessionId { get; set; }
    public int RoundId { get; set; }
    public int SessionId { get; set; }
}
