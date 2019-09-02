using Dapper;
using medtracker.DTOs;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace medtracker.SQL
{
    public class DataRepository
    {
        private readonly object dbLockObject = new object();
        private readonly string connectionString;

        public DataRepository(IConfiguration config)
        {
            connectionString = $"Data Source={config["DBPath"]}data.db";
        }

        public void CreateTableIfNotExists()
        {
            using(var connection = new SqliteConnection(connectionString))
            {
                connection.Execute("create table if not exists Data (UserId varchar(10), TeamId varchar(10), Date integer, HA_Present boolean, Num_Maxalt integer default 0, Num_Aleve integer default 0, primary key(UserId, TeamId, Date))");
            }
        }

        public void SetData(DataDTO data)
        {
            lock(dbLockObject)
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Execute("replace into Data (UserId, TeamId, Date, HA_Present, Num_Maxalt, Num_Aleve) values (@userID, @teamID, @date, @ha_present, @num_maxalt, @num_aleve)",
                        new { data.userID, data.teamID, data.date, data.ha_present, data.num_maxalt, data.num_aleve });
                }
            }
        }

        public IEnumerable<DataDTO> RetrieveUserData (string userID, string teamID)
        {
                using (var connection = new SqliteConnection(connectionString))
                {
                    return connection.Query<DataDTO>("select * from Data where UserId = (@userID) and TeamId = (@teamID)",
                        new { userID, teamID });
                }
        }
    }
}
