﻿using SQLite;
using System.Collections.ObjectModel;

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
        conn.CreateTable<Group>();
        conn.CreateTable<Round>();
        conn.CreateTable<Shot>();
        conn.CreateTable<Firearm>();
    }

    public int AddSession(Session session)
    {
        int result = 0;
        try
        {
            Init();

            if (session == null)
                throw new Exception("Valid Session Required");
            if (session.Id != -1)
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
    public int AddFirearmsToSession(List<Firearm> firearms, int session_id)
    {
        int result = 0;
        try
        {
            Init();
            if (firearms == null || firearms.Count == 0 || session_id == -1)
                throw new Exception("No Firearms To Add or bad session_id");
            for (int i = 0; i < firearms.Count; i++)
            {
                var fis = new FirearmInSession {
                    FirearmId = firearms[i].Id,
                    SessionID = session_id,
                };
                result = conn.Insert(fis);
            }
            StatusMessage = string.Format("{0} record(s) added", result);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed To Add Firearms To Session. Error {0}", ex.Message);
        }

        return result;
    }
    public int AddGroup(Group group)
    {
        int result = 0;
        try
        {
            Init();
            if (group == null)
                throw new Exception("Invalid Group");
            if (group.Id == 0)
            {

                result = conn.Insert(group);
                StatusMessage = string.Format("{0} Group Added.", result);
            }
            else
            {
                result = conn.Update(group);
                StatusMessage = string.Format("{0} Group Updated.", result);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add/update Group. Error {0}", ex.Message);
        }
        return result;
    }
    public int GetGroupId(string group_name)
    {
        int group_id = -1;
        try
        {
            Init();
            var result = from c in conn.Table<Group>()
                           where c.Name == group_name
                           select c.Id;
            StatusMessage = string.Format("Retrieved Group Id: {0}, With Group Name {1}.", result.FirstOrDefault(), group_name);
            return result.FirstOrDefault();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Could Not Get Id For Group. Error {0}", ex.Message);
        }

        return group_id;
    }
    public int AddGroupToSession(Models.GroupInSessison group_in_session)
    {
        var result = 0;
        try
        {
            Init();
            result = conn.Insert(group_in_session);

            StatusMessage = string.Format("Added {0} GroupInSession.", result);
            return result;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Could not add GroupInSession. Error {0}", ex.Message);
        }
        return result;

    }
    public int AddShot(Models.Shot shot)
    {
        int result = 0;
        try
        {
            Init();
            if (shot == null)
                throw new Exception("Invalid Shot");
            if (shot.Id == 0)
            {
                result = conn.Insert(shot);
                StatusMessage = string.Format("{0} Shot Added.", result);
            }
            else
            {
                result = conn.Update(shot);
                StatusMessage = string.Format("{0} Shot Updated.", result);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add/update Shot. Error {0}", ex.Message);
        }
        return result;
    }
    public int DeleteShot(Models.Shot shot)
    {
        int result = 0;
        try
        {
            Init();
            if (shot == null)
                throw new Exception("Invalid Shot");
            result = conn.Delete(shot);
            StatusMessage = string.Format("Removed {0} items.", result);
            return result;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to remove group. Error {0}", ex.Message);
        }
        return result;
    }
    public int DeleteGroup(int id)
    {
        int result = 0;

        try
        {
            Init();
            result = conn.Delete(GetGroupFromId(id));
            StatusMessage = string.Format("Removed {0} items.", result);
            return result;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to remove group. Error {0}", ex.Message);
        }
        return result;
    }
    public ObservableCollection<ViewModel.GroupData> GetGroupData(int session_id)
    {

        try
        {
            var group_data_list = new ObservableCollection<ViewModel.GroupData>(); 

            var choice = from c in conn.Table<Group>()
                         where c.SessionId == session_id
                         select c;
            var group_list = choice.ToList();
            for (int i = 0; i < choice.Count(); i++)
            {
                var temp = new ViewModel.GroupData
                {
                    Id = group_list[i].Id,
                    GroupNum = i,
                    SessionId = session_id,
                    FirearmName = App.FirearmRepo.GetFirearmNameFromId(group_list[i].FirearmId),
                    FirearmId = group_list[i].FirearmId,
                    RoundName = GetRoundNameFromId(group_list[i].RoundId),
                    RoundId = group_list[i].RoundId,
                    AverageVelocity = group_list[i].AverageVelocity,
                    StDev = group_list[i].StDev
                };
                group_data_list.Add(temp); 
                
            }
            return group_data_list;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. Error {0}", ex.Message);
        }
        return new ObservableCollection<ViewModel.GroupData>();
    }
    public string GetRoundNameFromId(int? id)
    {
        try
        {
            if (id == null)
                return string.Empty;
            Init();
            var choice = from c in conn.Table<Round>()
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
    public int GetRoundIdFromName(string name)
    {
        int Id = 0;
        try
        {
            if (name == string.Empty)
                return 0;
            Init();
            var choice = from c in conn.Table<Round>()
                         where c.Name == name
                         select c.Id;
            StatusMessage = string.Format("Found Item {0}", name);
            return choice.FirstOrDefault();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }
        return Id;

    }
    public Group GetGroupFromId(int id)
    {
        try
        {
            Init();
            var choice = from c in conn.Table<Group>()
                         where c.Id == id
                         select c;
            StatusMessage = string.Format("Found Item {0}", id);
            return choice.FirstOrDefault();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }
        return new Group();
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
    public List<Firearm> GetFirearmsInSession(int session_id)
    {
        try
        {
            Init();
            var choice = from c in conn.Table<FirearmInSession>() 
                         where c.SessionID == session_id
                         from f in conn.Table<Firearm>()
                         where f.Id == c.FirearmId
                         select f;
            var list = choice.ToList();
            StatusMessage = string.Format("Found {0} Name(s)", list.Count());

            return list;

        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }
        return new List<Firearm>();
    }
    public int GetSessionIdFromName(string name)
    {
        int Id = -1;
        try
        {
            if (name == string.Empty)
                return 0;
            Init();
            var choice = from c in conn.Table<Session>()
                         where c.Name == name
                         select c.Id;
            StatusMessage = string.Format("Found Item {0}", name);
            return choice.FirstOrDefault();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }
        return Id;
    }
}
