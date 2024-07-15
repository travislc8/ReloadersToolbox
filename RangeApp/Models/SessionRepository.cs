using SQLite;

namespace RangeApp.Models;

public class SessionRepository
{
    string _dbPath;
    private SQLiteConnection conn;
    public string StatusMessage = "";

    public SessionRepository(string dbPath)
    {
        _dbPath = dbPath;
    }
    private void Init()
    {
        if (conn != null)
            return;
        conn = new SQLiteConnection(_dbPath);
        conn.CreateTable<Session>();
        conn.CreateTable<FirearmInSession>();
    }

    public int AddSession(Session session)
    {
        int result = 0;
        try
        {
            Init();

            if (session == null)
                throw new Exception("Valid Session Required");
            if (session.Id != 0)
                result = conn.Update(session);
            else
                result = conn.Insert(session);

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, session.Name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", session.Name, ex.Message);
        }
        return result; 

    }
    public int AddFirearmsToSession(List<Firearm> firearms)
    {
        int result = 0;
        try
        {
            Init();
            if (firearms == null || firearms.Count == 0)
                throw new Exception("No Firearms To Add");
            for (int i = 0; i < firearms.Count; i++)
            {
                if (firearms[i].Id == 0)
                    result += conn.Insert(firearms[i]);
                else
                    result += conn.Update(firearms[i]);
            }
            StatusMessage = string.Format("{0} record(s) added", result);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed To Add Firearms To Session. Error {0}", ex.Message);
        }

        return result;
    }
    public List<string> GetSessionNames()
    {
        try
        {
            Init();
            var choice = from c in conn.Table<Session>()
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
}
