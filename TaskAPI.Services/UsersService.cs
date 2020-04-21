using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Common.Exceptions;
using TaskAPI.Data.DataContexts;
using TaskAPI.Data.Models;
using TaskAPI.Services.Interfaces;

namespace TaskAPI.Services
{
    public class UsersService : IUsersService
    {
        private readonly DataContext _dataCotext;
        private readonly ICompositeDataValidator _dataValidator;

        public UsersService(
            DataContext dataCotext,
            ICompositeDataValidator dataValidator
        ) {
            _dataCotext = dataCotext;
            _dataValidator = dataValidator;
        }

        public async Task<List<User>> GetUsers() {
            var users = await _dataCotext.Users.ToListAsync();

            return users;
        }

        public async Task<User> GetUser(Guid userId)
        {
            var user = await _dataCotext.Users.FindAsync(userId);

            if (user == null)
                throw new NotFoundException(new[] { "User not found." });

            return user;
        }

        public async Task AddUser(User userToAdd) {
            await _dataValidator.Validate(userToAdd);
            await _dataCotext.Users.AddAsync(userToAdd);
            await _dataCotext.SaveChangesAsync();
        }

        public async Task UpdateUser(Guid userId, UpdateUser update) {
            var userToUpdate = await _dataCotext.Users.FindAsync(userId);

            if (userToUpdate == null)
                throw new NotFoundException(new[] { "User not found" });

            userToUpdate.FirstName = update.FirstName;
            userToUpdate.LastName = update.LastName;
            userToUpdate.Address = update.Address;
            userToUpdate.Sex = update.Sex;
            userToUpdate.Phone = update.Phone;

            await _dataCotext.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid userId) {
            var userToRemove = await _dataCotext.Users.FindAsync(userId);

            if(userToRemove == null)
            {
                return;
            }

            var userTasks = await _dataCotext.UserTasks.Where(t => t.UserId == userId).ToArrayAsync();

            if (userTasks.Any()) {
                _dataCotext.UserTasks.RemoveRange(userTasks);
            }

            _dataCotext.Users.Remove(userToRemove);

            await _dataCotext.SaveChangesAsync();
        }
    }
}
