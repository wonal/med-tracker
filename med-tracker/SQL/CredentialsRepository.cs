﻿using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using medtracker.DTOs;
using Newtonsoft.Json;

namespace medtracker.SQL
{
    public class CredentialsRepository 
    {
        private readonly object dbLockObject = new object();
        private readonly string connectionString;

        public CredentialsRepository(IConfiguration config)
        {
            connectionString = $"Data Source={config["DBPath"]}data.db";
        }

        public void CreateTableIfNotExists()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Execute(@"create table if not exists TeamCredentials (Id integer primary key, TeamID varchar(10), Credentials varchar(200))");
            }
        }
        public AuthResponseDTO GetCredentials(string teamID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return JsonConvert.DeserializeObject<AuthResponseDTO>(connection.Query<string>(@"select Credentials from TeamCredentials where TeamID = (@Key)", new { Key = teamID }).First());
            }
        }

        public void SetCredentials(string teamID, AuthResponseDTO response)
        {
            lock(dbLockObject)
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Execute(@"insert into TeamCredentials (TeamID, Credentials) values (@TeamID, @Credentials)", new { TeamID = teamID, Credentials = JsonConvert.SerializeObject(response) });
                }
            }
        }
    }
}