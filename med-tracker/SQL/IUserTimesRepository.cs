using System.Collections.Generic;
using medtracker.Models;

namespace medtracker.SQL
{
    public interface IUserTimesRepository
    {
        void CreateTableIfNotExists();
        IEnumerable<UserTeam> GetUsers(int time1, int time2);
        void SetUserTime(string userID, string teamID, int time);
        IEnumerable<int> GetUserTime(string userID, string TeamID);
        void DeleteUserTime(string userID, string teamID);
        IEnumerable<UserTeam> GetUniqueUsers();
    }
}