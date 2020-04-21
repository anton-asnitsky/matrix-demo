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
    public class TaskDataValidator : IDataValidator
    {
        private readonly DataContext _dataContext;

        public TaskDataValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool Handle(object dto)
        {
            return dto.GetType() == typeof(UserTask);
        }

        public async Task Validate(object dto)
        {
            var knownDto = (UserTask)dto;

            var existingTask = await _dataContext.UserTasks.Where(t => t.Name.Trim().ToLower() == knownDto.Name.Trim().ToLower()).ToListAsync();

            if (existingTask.Any()) {
                throw new ConflictException($"Task witn name '{knownDto.Name}' already exists.");
            }
        }
    }
}
