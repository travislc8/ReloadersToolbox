using SQLite;

namespace RangeApp.Models;

public class RoundRepository
{
    string _dbPath;
    private SQLiteConnection? conn;
    public string StatusMessage = "";

    public RoundRepository(string dbPath)
    {
        _dbPath = dbPath;
    }
    private void Init()
    {
        if (conn != null)
            return;
        conn = new SQLiteConnection(_dbPath);
        conn.CreateTable<Round>();
        conn.CreateTable<FirearmToRound>();
        conn.CreateTable<Powder>();
        conn.CreateTable<Bullet>();
    }

    public int GetRoundId(Round round)
    {
        int id = -1;
        try
        {
            Init();
            if (round == null)
                return id;
            var result = from c in conn.Table<Round>()
                         where c.Name == round.Name
                         where c.BulletId == round.BulletId
                         where c.Caliber == round.Caliber
                         where c.PowderGrains == round.PowderGrains
                         where c.PowderId == round.PowderId
                         where c.CaseName == round.CaseName
                         where c.Primer == round.Primer
                         where c.OverallLength == round.OverallLength
                         select c.Id;
            id = result.FirstOrDefault(-1);
            StatusMessage = string.Format("Retrieved round id: {0}, for round {1}.", id, round.Name);
            return id;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. Error {0}", ex.Message);
        }
        return id;
    }
    public int AddNewRound(Round round) {
        int result = 0;
        try
        {
            Init();

            if (round == null)
                throw new Exception("Valid round required");
            if (round.Id != 0)
                result = conn.Update(round);
            else
                result = conn.Insert(round);

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, round.Name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", round.Name, ex.Message);
        }
        return result;

    }
    public int AddNewPowder(Powder powder) {
        int result = 0;
        try
        {
            Init();

            if (powder == null)
                throw new Exception("Valid bulletEntry required");
            if (powder.Id != 0)
                result = conn.Update(powder);
            else
                result = conn.Insert(powder);

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, powder.Name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", powder.Name, ex.Message);
        }
        return result;
    }
    public int AddNewBullet(Bullet bullet) {
        int result = 0;
        try
        {
            Init();

            if (bullet == null)
                throw new Exception("Valid bulletEntry required");
            if (bullet.Id != 0)
                result = conn.Update(bullet);
            else
                result = conn.Insert(bullet);

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, bullet.Name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", bullet.Name, ex.Message);
        }
        return result;
    }
    public int AddRoundToFirearm(int firearm_id, int round_id) 
    {
        int result = 0;

        try
        {
            Init();
            if (firearm_id == 0)
                throw new Exception("Invalid firearm id");
            if (round_id == 0)
                throw new Exception("Invalid round id");
            var temp = new FirearmToRound
            {
                FirearmId = firearm_id,
                RoundId = round_id
            };

            var check = from c in conn.Table<FirearmToRound>()
                        where c.FirearmId == temp.FirearmId
                        where c.RoundId == temp.RoundId
                        select c.FirearmToRoundId;
            var id = check.FirstOrDefault(-1);
            if (id == -1)
            {
                result = conn.Insert(temp);
            }
            StatusMessage = string.Format("{0} record added (round id: {1}, firearm id: {2})", result, round_id, firearm_id);
            return result;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add Round to Firearm. Error: {0}", ex.Message);
        }
        return result;

    }
    public List<Round> GetRounds()
    {
        try
        {
            Init();
            var result = conn.Table<Round>().ToList();
            StatusMessage = string.Format("Retrieved {0} Rounds.", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieved rounds.", ex.Message);
        }
        return new List<Round>();
    }
    public List<Powder> GetPowders()
    {
        try
        {
            Init();
            var result = conn.Table<Powder>().ToList();
            StatusMessage = string.Format("Retrieved {0} Powders.", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieved Powders.", ex.Message);
        }
        return new List<Powder>();
    }
    public List<Bullet> GetBullets()
    {
        try
        {
            Init();
            var result = conn.Table<Bullet>().ToList();
            StatusMessage = string.Format("Retrieved {0} Bullets.", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieved Bullets.", ex.Message);
        }
        return new List<Bullet>();
    }

    public int DeleteRound(Round round)
    {
        int result = 0;
        try
        {
            Init();
            if (round == null)
                throw new Exception("Invalid round");
            result = conn.Delete(round);
            StatusMessage = string.Format("Removed {0} items.", result);
            return result;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to remove round. Error {0}", ex.Message);
        }
        return result;
    }
    public int DeleteBullet(Bullet bullet)
    {
        int result = 0;
        try
        {
            Init();
            if (bullet == null)
                throw new Exception("Invalid round");
            result = conn.Delete(bullet);
            StatusMessage = string.Format("Removed {0} items.", result);
            return result;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to remove bulletEntry. Error {0}", ex.Message);
        }
        return result;
    }
    public int DeletePowder(Powder powder)
    {
        int result = 0;
        try
        {
            Init();
            if (powder == null)
                throw new Exception("Invalid round");
            result = conn.Delete(powder);
            StatusMessage = string.Format("Removed {0} items.", result);
            return result;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to remove powderEntry. Error {0}", ex.Message);
        }
        return result;
    }
}
