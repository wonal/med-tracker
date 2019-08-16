using medtracker.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medtracker
{
    interface ISlackAPI
    {
        Task<AuthResponseDTO> Authorize(string code);
        Task SendMessage(string token, string channelID, string message);
    }
    public interface IRepository<T>
    {
        void CreateTableIfNotExists();
        void SetValue(string key, T value);
        T GetValue(string key);
    }
}
