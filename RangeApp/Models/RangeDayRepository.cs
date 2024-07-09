
using SQLite;
using RangeApp.Models;

namespace RangeApp.Models;

public class RangeDayRepository
{
    string _dbPath;
    private SQLiteConnection conn;


    public string StatusMessage { get; set; }

    // TODO: Add variable for the SQLite connection

    private void Init()
    {
        // TODO: Add code to initialize the repository         
        if (conn != null)
            return;
        conn = new SQLiteConnection(_dbPath);
        conn.CreateTable<Firearm>();
        conn.CreateTable<FirearmInSession>();
        conn.CreateTable<FirearmToRound>();
        conn.CreateTable<Group>();
        conn.CreateTable<GroupInSessison>();
        conn.CreateTable<Round>();
        conn.CreateTable<RoundInSession>();
        conn.CreateTable<Session>();
        conn.CreateTable<Shot>();
    }

    public RangeDayRepository(string dbPath)
    {
        _dbPath = dbPath;
        StatusMessage = "";
    }

    public int AddNewFirearm(Firearm firearm)
    {
        int result = 0;
        try
        {
            Init();

            if (firearm == null)
                throw new Exception("Valid name required");

            result = conn.Insert(new Firearm
            {
                Name = firearm.Name,
                BarrelLength = firearm.BarrelLength,
                Manufacturer = firearm.Manufacturer,
                Caliber = firearm.Caliber,
                ScopeID = firearm.ScopeID
            });

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, firearm.Name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", firearm.Name, ex.Message);
        }
        return result; 

    }

    public List<Firearm> GetAllFirearms()
    {
        // TODO: Init then retrieve a list of Firearm objects from the database into a list
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
    public List<Locations> GetAllLocations()
    {

        try
        {
            Init();
            return conn.Table<Locations>().ToList();

        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Locations>();
    }
    public List<Firearm> GetFirearmsInSession(int session_id)
    {
        try
        {
            Init();
            var command = new SQLiteCommand(conn);
            command.CommandText = "SELECT Firearm FROM Firearm ,Sessions, FirearmInSession WHERE Session.Id=session_id AND Firearm.Id=FirearmInSession.FirearmId";
            return command.ExecuteQuery<Firearm>();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Firearm>();
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
            ;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }
        return new Firearm();
    }
}
