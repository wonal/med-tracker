using System.Collections.Generic;
using medtracker.Models;

namespace medtracker.SQL
{
    public interface ISubscriberRepository
    {
        void CreateTableIfNotExists();
        void DeleteSubscriber(string userID, string teamID);
        IEnumerable<UserTeam> GetSubscriber(string userId, string teamId);
        IEnumerable<UserTeam> GetSubscribers();
        void SetSubscriber(string userID, string teamID);
    }
}