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

    public static LocationData GetData(Models.Location? location)
    {
        var data = new LocationData();
        if (location == null) return data;

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

    public static List<LocationData> GetData(List<Models.Location> locations)
    {
        List<LocationData> dataList = [];
        foreach (var location in locations)
        {
            dataList.Add(LocationData.GetData(location));
        }

        return dataList;
    }

    public static List<Models.Location> GetLocation(List<LocationData> dataList)
    {
        List<Models.Location> locations = [];
        foreach (var data in dataList)
        {
            locations.Add(LocationData.GetLocation(data));
        }

        return locations;
    }
}
