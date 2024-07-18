using System;
using SQLite;

namespace RangeApp.Models;
[Table("FirearmInSession")]
public class FirearmInSession
{
    [PrimaryKey,AutoIncrement]
    public int FirearmInSessionId { get; set; }
    public int FirearmId { get; set; }
    public int SessionID { get; set; }
}
