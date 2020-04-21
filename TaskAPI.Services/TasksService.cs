using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Data.Models;
using TaskAPI.Services.Interfaces;

namespace TaskAPI.Services
{
    public class TasksService : ITasksService
    {
        public Task CreateTask(UserTask task)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTask(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<UserTask> GetTask(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserTask>> GetTasks()
        {
            throw new NotImplementedException();
        }

        public Task UpdateTask(Guid taskId, UpdateTask update)
        {
            throw new NotImplementedException();
        }
    }
}
