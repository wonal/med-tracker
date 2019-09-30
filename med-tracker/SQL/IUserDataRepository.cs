using System.Collections.Generic;
using medtracker.DTOs;

namespace medtracker.SQL
{
    public interface IUserDataRepository
    {
        void CreateTableIfNotExists();
        IEnumerable<DataDTO> RetrieveUserData(string userID, string teamID);
        IEnumerable<DataDTO> GetLastThirtyRecords(string userID, string teamID, long dayInSeconds);
        void SetData(DataDTO data);
        IEnumerable<DataDTO> GetCurrentMonthRecords(string userId, string teamId, long startDateInSeconds);
    }
}