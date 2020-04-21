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
        Task<(List<Guid> Assigned, List<Guid> NotAssigned)> CreateTask(UserTask task, List<Guid> assignTo);
        Task UpdateTask(Guid taskId, UpdateTask update);
        Task<(List<Guid> Assigned, List<Guid> NotAssigned)> AssignUsers(Guid taskId, List<Guid> usersToAssign, List<Guid> usersToUnassign);
        Task DeleteTask(Guid taskId);
        Task<List<UserTask>> GetTasksByEmail(string email);
        Task CompleteTask(Guid taskId);
    }
}
