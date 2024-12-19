namespace RangeApp.ViewModel;

public class LocationData
{
    public LocationData()
    {
        Id = 0;
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public int? ShootingDirection { get; set; }

    public static LocationData GetData(Models.Location location)
    {
        var data = new LocationData();

        data.Id = location.Id;
        data.Name = location.Name;
        data.ShootingDirection = location.ShootingDirection;

        return data;

    }

    public static Models.Location GetLocation(LocationData data)
    {
        var location = new Models.Location();

        location.Id = data.Id;
        location.Name = data.Name;
        location.ShootingDirection = data.ShootingDirection;

        return location;
    }
}
