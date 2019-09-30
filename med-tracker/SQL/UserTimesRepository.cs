using Dapper;
using medtracker.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace medtracker.SQL
{
    public class UserTimesRepository : IUserTimesRepository
    {
        private readonly string connectionString;

        public UserTimesRepository(IConfiguration config)
        {
            connectionString = $"Data Source={config["DBPath"]}data.db";
        }


        public void CreateTableIfNotExists()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute("create table if not exists UserTimes (UserId varchar(10), TeamId varchar (10), Time integer, primary key (UserId, TeamId));" +
                                   "create unique index if not exists UserIndex on UserTimes (UserId, TeamId)");
            }
        }

        public void SetUserTime(string userID, string teamID, int time)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute(@"replace into UserTimes (UserId, TeamId, Time) values (@UserId, @TeamId, @Time)", new { UserId = userID, TeamId = teamID, Time = time });
            }
        }

        public IEnumerable<UserTeam> GetUsers(int time1, int time2)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return (connection.Query<UserTeam>(@"select UserId, TeamId from UserTimes where Time >= (@Time1) and Time < (@Time2)", new { Time1 = time1, Time2 = time2 }));
            }
        }

        public IEnumerable<int> GetUserTime(string userID, string teamID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.Query<int>(@"select Time from UserTimes where UserId = (@userID) and TeamId = (@teamID)", new { userID, teamID });
            }
        }

        public IEnumerable<UserTeam> GetUniqueUsers()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.Query<UserTeam>(@"select distinct (UserId, TeamId) from UserTimes");
            }
        }
        public void DeleteUserTime(string userID, string teamID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute(@"delete from UserTimes where UserId = (@userID) and TeamId = (@teamID)", new { userID, teamID });
            }
        }
    }
}
