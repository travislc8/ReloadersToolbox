namespace RangeApp.ViewModel;

public class FirearmData
{
    public FirearmData()
    {
        Id = 0;
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public int? BarrelLength { get; set; }
    public string? Manufacturer { get; set; }
    public string? Caliber { get; set; }
    public string? ScopeID { get; set; }

    public static FirearmData GetData(Models.Firearm firearm)
    {
        var data = new FirearmData();

        data.Id = firearm.Id;
        data.Name = firearm.Name;
        data.BarrelLength = firearm.BarrelLength;
        data.Manufacturer = firearm.Manufacturer;
        data.Caliber = firearm.Caliber;
        data.ScopeID = firearm.ScopeID;

        return data;
    }

    public static Models.Firearm GetFirearm(FirearmData data)
    {
        var firearm = new Models.Firearm();

        firearm.Id = data.Id;
        firearm.Name = data.Name;
        firearm.BarrelLength = data.BarrelLength;
        firearm.Manufacturer = data.Manufacturer;
        firearm.Caliber = data.Caliber;
        firearm.ScopeID = data.ScopeID;

        return firearm;

    }

    public static List<FirearmData> GetData(List<Models.Firearm> firearms)
    {
        List<FirearmData> dataList = [];
        foreach (var firearm in firearms)
        {
            dataList.Add(FirearmData.GetData(firearm));
        }

        return dataList;
    }

    public static List<Models.Firearm> GetFirearm(List<FirearmData> dataList)
    {
        List<Models.Firearm> firearms = [];
        foreach (var data in dataList)
        {
            firearms.Add(FirearmData.GetFirearm(data));
        }

        return firearms;
    }
}
