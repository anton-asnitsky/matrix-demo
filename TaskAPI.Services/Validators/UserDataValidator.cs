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

namespace TaskAPI.Services.Validators
{
    public class UserDataValidator : IDataValidator
    {
        private readonly DataContext _dataContext;

        public UserDataValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Validate(object dto)
        {
            var knownDto = (User)dto;

            var existingUser = await _dataContext.Users.Where(u => u.Email == knownDto.Email).ToListAsync();

            if (existingUser.Any()) {
                throw new ConflictException($"User with email {knownDto.Email} already exists.");
            }
        }

        public bool Handle(object dto)
        {
            return dto.GetType() == typeof(User);
        }
    }
}
