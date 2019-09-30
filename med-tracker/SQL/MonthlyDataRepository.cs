using Dapper;
using medtracker.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace medtracker.SQL
{
    public class MonthlyDataRepository
    {
        private readonly string connectionString;
        
        public MonthlyDataRepository(IConfiguration config)
        {
            connectionString = $"Data Source={config["DBPath"]}data.db";
        }

        public void CreateTableIfNotExists()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute("create table if not exists MonthlyData (UserId varchar(10), TeamId varchar(10), Month varchar(10), TotalHa integer default 0, AvgMaxalt integer default 0, AvgAleve integer default 0, primary key(UserId, TeamId, Month));" +
                    "create unique index if not exists MonthlyIndex on MonthlyData (UserId, TeamId, Month)");
            }
        }

        public void StoreData(string userId, string teamId, MonthlyData data)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute(@"replace into MonthlyData (UserId, TeamId, Month, TotalHa, AvgMaxalt, AvgAleve) values (@userId, @teamId, @month, @total, @avgMaxalt, @avgAleve", new { userId, teamId, month = data.Month, total = data.TotalHa, avgMaxalt = data.AvgMaxalt, avgAleve = data.AvgAleve });
            }
        }

        public IEnumerable<MonthlyData> RetrieveData(string userId, string teamId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.Query<MonthlyData>(@"select * from MonthlyData where UserId = @userId and TeamId = @teamId order by Month", new { userId, teamId });
            }
        }
    }
}
