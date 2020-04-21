using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Data.Models;

namespace TaskAPI.Services.Interfaces
{
    public interface ITasksService
    {
        Task<List<UserTask>> GetTasks();
        Task<UserTask> GetTask(Guid taskId);
        Task CreateTask(UserTask task);
        Task UpdateTask(Guid taskId, UpdateTask update);
        Task DeleteTask(Guid taskId);
    }
}
