namespace RangeApp.ViewModel;

public class ShotData
{
    public int Id { get; set; }
    public string IdPre { get; set; }
    public float Velocity { get; set; }
    public string VelocityPre {  get; set; }
    public string? Note { get; set; }
    public string NotePre { get; set; }

    public ShotData()
    {

        IdPre = "Shot: ";
        Id = 0;
        Velocity = 0f;
        VelocityPre = " Velocity: ";
        Note = string.Empty;
        NotePre = string.Empty;
    }
    public ShotData(int id, float velocity, string note, string note_pre)
    {
        Id = id;
        IdPre = "Shot: ";
        Velocity = velocity;
        VelocityPre = " Velocity: ";
        Note = note;
        NotePre = note_pre;
    }
}
