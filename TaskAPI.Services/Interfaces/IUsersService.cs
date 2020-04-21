using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Data.Models;

namespace TaskAPI.Services.Interfaces
{
    public interface IUsersService
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(Guid userId);
        Task AddUser(User userToAdd);
        Task UpdateUser(Guid userId, UpdateUser update);
        Task DeleteUser(Guid userId);
    }
}
