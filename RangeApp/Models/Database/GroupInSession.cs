using System;
using SQLite;

namespace RangeApp.Models;
[Table("GroupInSession")]
public class GroupInSessison
{
    [PrimaryKey]
    public int GroupInSessionId { get; set; }
    public int GroupId { get; set; }
    public int SesisonId { get; set; }
}
