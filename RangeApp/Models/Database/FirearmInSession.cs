using System;
using SQLite;

namespace RangeApp.Models;
[Table("FirearmInSession")]
public class FirearmInSession
{
    [PrimaryKey]
    public int FirearmInSessionId { get; set; }
    public int FirearmId { get; set; }
    public int SessionID { get; set; }
}
