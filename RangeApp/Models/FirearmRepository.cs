using SQLite;

namespace RangeApp.Models;

public class FirearmRepository
{
    string _dbPath;
    private SQLiteConnection conn;
    public string StatusMessage = "";

    public FirearmRepository(string dbPath)
    {
        _dbPath = dbPath;
        conn = new SQLiteConnection(_dbPath);
        conn.CreateTable<Firearm>();
    }
    private void Init()
    {
        if (conn != null)
            return;
        conn = new SQLiteConnection(_dbPath);
        conn.CreateTable<Firearm>();

    }

    /// <summary>
    /// Adds a firearm to the database, if the firearm already exists then it 
    /// is edited 
    /// </summary>
    public int AddNewFirearm(Firearm firearm)
    {
        int result = 0;
        try
        {
            Init();

            if (firearm == null)
                throw new Exception("Valid nameEntry required");
            if (firearm.Id != 0)
                result = conn.Update(firearm);
            else
                result = conn.Insert(firearm);

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, firearm.Name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", firearm.Name, ex.Message);
        }
        return result;

    }

    public int RemoveFirearm(ViewModel.FirearmData firearmData)
    {
        int result = 0;
        var firearm = ViewModel.FirearmData.GetFirearm(firearmData);
        try
        {
            Init();
            result = conn.Delete(firearm);
            StatusMessage = string.Format("{0} record(s) removed (Name: {1})", result, firearm.Name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to remove {0}. Error: {1}", firearm.Name, ex.Message);
        }
        return result;
    }

    public int RemoveFirearm(Firearm firearm)
    {
        int result = 0;
        try
        {
            Init();
            result = conn.Delete(firearm);
            StatusMessage = string.Format("{0} record(s) removed (Name: {1})", result, firearm.Name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to remove {0}. Error: {1}", firearm.Name, ex.Message);
        }
        return result;
    }
    public int EditFirearm(Firearm firearm)
    {
        int result = 0;
        try
        {
            Init();
            result = conn.Update(firearm);

            StatusMessage = string.Format("{0} record(s) updated (Name: {1})", result, firearm.Name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to update {0}. Error: {1}", firearm.Name, ex.Message);
        }
        return result;
    }


    public List<ViewModel.FirearmData> GetAllFirearmData()
    {

        try
        {
            Init();
            var firearms = conn.Table<Firearm>().ToList();
            var firearmData = new List<ViewModel.FirearmData>();
            foreach (var firearm in firearms)
            {
                firearmData.Add(ViewModel.FirearmData.GetData(firearm));
            }
            return firearmData;

        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<ViewModel.FirearmData>();
    }
    public List<Firearm> GetAllFirearms()
    {
        try
        {
            Init();
            return conn.Table<Firearm>().ToList();

        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Firearm>();
    }
    public List<String> GetAllFirearmNames()
    {
        try
        {
            Init();
            var choice = from c in conn.Table<Firearm>()
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
    public List<Location> GetAllLocations()
    {

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
    public List<Firearm> GetFirearmsInSession(int session_id)
    {
        List<Firearm> list = [];
        try
        {
            Init();
            var result = from c in conn.Table<FirearmInSession>()
                         from f in conn.Table<Firearm>()
                         where c.SessionID == session_id
                         where c.FirearmId == f.Id
                         select f;
            list = result.ToList();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return list;
    }
    public Firearm GetFirearmFromName(string name)
    {
        try
        {
            Init();
            var choice = from c in conn.Table<Firearm>()
                         where c.Name == name
                         select c;
            StatusMessage = string.Format("Found Item {0}", name);
            return choice.FirstOrDefault();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }
        return new Firearm();
    }
    public string? GetFirearmNameFromId(int? id)
    {
        try
        {
            if (id == null)
                return string.Empty;
            Init();
            var choice = from c in conn.Table<Firearm>()
                         where c.Id == id
                         select c.Name;
            StatusMessage = string.Format("Found Item {0}", id);
            return choice.FirstOrDefault();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }
        return string.Empty;

    }
}
