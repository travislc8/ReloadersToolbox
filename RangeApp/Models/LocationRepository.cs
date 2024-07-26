using SQLite;

namespace RangeApp.Models;

public class LocationRepository
{
    string _dbPath;
    private SQLiteConnection conn;
    public string StatusMessage = "";

    public LocationRepository(string dbPath)
    {
        _dbPath = dbPath;
        Init();
    }
    private void Init()
    {
        if (conn != null)
            return;
        conn = new SQLiteConnection(_dbPath);
        conn.CreateTable<Location>();

    }
    public int AddNewLocation(Location location) {
        int result = 0;
        try
        {
            Init();

            if (location == null)
                throw new Exception("Valid nameEntry required");
            if (location.Id != 0)
                result = conn.Update(location);
            else
                result = conn.Insert(location);

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, location.Name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", location.Name, ex.Message);
        }
        return result; 

    }
    public int RemoveLocation(Location location)
    {
        int result = 0;
        try
        {
            Init();
            result = conn.Delete(location);
            StatusMessage = string.Format("{0} record(s) removed (Name: {1})", result, location.Name);
        } 
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to remove {0}. Error: {1}", location.Name, ex.Message);
        }
        return result; 
    }
    public int EditLocation(Location location)
    {
        int result = 0;
        try
        {
            Init();
            result = conn.Update(location);
                       
            StatusMessage = string.Format("{0} record(s) updated (Name: {1})", result, location.Name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to update {0}. Error: {1}", location.Name, ex.Message);
        }
        return result; 
    }
    

    public List<Location> GetAllLocations()
    {
        // TODO: Init then retrieve a list of Location objects from the database into a list
        try
        {
            Init();
            return conn.Table<Location>().ToList();

        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Location>();
    }
    public List<String> GetAllLocationNames()
    {
        try
        {
            Init();
            var choice = from c in conn.Table<Location>()
                         select c.Name;
            var list = choice.ToList();
            StatusMessage = string.Format("Found {0} Name(s)", list.Count());

            return list;

        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }
        return new List<String>();
    }
    public Location GetLocationFromName(string name)
    {
        try
        {
            Init();
            var choice = from c in conn.Table<Location>()
                         where c.Name == name
                         select c;
            StatusMessage = string.Format("Found Item {0}", name);
            return choice.FirstOrDefault();
            ;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }
        return new Location();
    }
}
