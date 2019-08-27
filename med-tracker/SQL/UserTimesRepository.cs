using Dapper;
using medtracker.DTOs;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medtracker.SQL
{
    public class UserTimesRepository
    {
        private readonly object dbLockObject = new object();
        private readonly string connectionString;

        public UserTimesRepository(IConfiguration config)
        {
            connectionString = $"Data Source={config["DBPath"]}data.db";
        }


        public void CreateTableIfNotExists()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute("create table if not exists UserTimes (UserId varchar(10), TeamId varchar (10), Time varchar(6), primary key (UserId, TeamId));" +
                                   "create unique index if not exists UserIndex on UserTimes (UserId, TeamId)");
            }
        }

        public void SetUserTime(string userID, string teamID, string time)
        {
            lock(dbLockObject)
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Execute(@"replace into UserTimes (UserId, TeamId, Time) values (@UserId, @TeamId, @Time)", new { UserId = userID, TeamId = teamID, Time = time });
                }
            }
        }

        public IEnumerable<string> GetUsers(string time)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.Query<string>(@"select UserId from UserTimes where Time = (@Time)", new { Time = time });
            }
        }
    }
}
