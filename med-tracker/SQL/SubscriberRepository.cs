using Dapper;
using medtracker.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medtracker.SQL
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly string connectionString;

        public SubscriberRepository(IConfiguration config)
        {
            connectionString = $"Data Source={config["DBPath"]}data.db";
        }

        public void CreateTableIfNotExists()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute("create table if not exists Subscribers (UserId varchar(10), TeamId varchar(10), primary key (UserId, TeamId));" +
                    "create unique index if not exists SubscribersIndex on Subscribers (UserId, TeamId)");
            }
        }
        public void SetSubscriber(string userID, string teamID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute(@"replace into Subscribers (UserId, TeamId) values (@UserId, @TeamId)", new { UserId = userID, TeamId = teamID });
            }
        }

        public IEnumerable<UserTeam> GetSubscribers()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return (connection.Query<UserTeam>("select * from Subscribers"));
            }
        }

        public void DeleteSubscriber(string userID, string teamID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute(@"delete from Subscribers where UserId = (@userID) and TeamId = (@teamID)", new { userID, teamID });
            }
        }

        public IEnumerable<UserTeam> GetSubscriber(string userId, string teamId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.Query<UserTeam>(@"select * from Subscribers where UserId = (@userId) and TeamId = (@teamId)", new { userId, teamId });
            }
        }
    }
}
