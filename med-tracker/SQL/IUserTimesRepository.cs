using System.Collections.Generic;
using medtracker.DTOs;

namespace medtracker.SQL
{
    public interface IUserTimesRepository
    {
        void CreateTableIfNotExists();
        IEnumerable<UserTeamDTO> GetUsers(int time1, int time2);
        void SetUserTime(string userID, string teamID, int time);
    }
}