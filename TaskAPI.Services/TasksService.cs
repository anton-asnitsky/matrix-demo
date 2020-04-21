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
    public class TasksService : ITasksService
    {
        private readonly DataContext _dataContext;
        private readonly IDataValidator _dataValidator;
        private readonly IMailer _mailer;

        public TasksService(
            DataContext dataContext,
            IDataValidator dataValidator,
            IMailer mailer
        ) {
            _dataContext = dataContext;
            _dataValidator = dataValidator;
            _mailer = mailer;
        }

        public async Task<(List<Guid> Assigned, List<Guid> NotAssigned)> AssignUsers(
            Guid taskId, 
            List<Guid> usersToAssign, 
            List<Guid> usersToUnassign
        ) {
            var nonExistingUsers = new List<Guid>();
            var usersToInform = new List<User>();

            var task = await _dataContext.UserTasks.FindAsync(taskId);

            if (task == null)
                throw new NotFoundException(new[] { "Task not found." });

            usersToUnassign.ForEach(async uid => {
                var user = await _dataContext.Users.FindAsync(uid);
                if (user == null)
                {
                    return;
                }

                var assigmentsToRemove = task.Assignments.Where(a => a.UserId == uid).ToList();

                assigmentsToRemove.ForEach(a => {
                    task.Assignments.Remove(a);
                });
            });

            usersToAssign.ForEach(async uid => {
                var user = await _dataContext.Users.FindAsync(uid);
                if (user == null)
                {
                    nonExistingUsers.Add(uid);
                    return;
                }

                if (task.Assignments.Where(a => a.UserId == uid).Any()) {
                    return;
                }

                usersToInform.Add(user);
                task.Assignments.Add(new TaskAssignment()
                {
                    UserId = uid,
                    TaskId = task.TaskId
                });
            });

            await _dataContext.SaveChangesAsync();

            usersToInform.ForEach(async u => {
                await _mailer.SendUserTasks(u.Email, $"{u.FirstName} {u.LastName}", new List<UserTask>() { task });
            });

            return (usersToAssign.Except(nonExistingUsers).ToList(), nonExistingUsers);
        }

        public async Task CompleteTask(Guid taskId)
        {
            var task = await _dataContext.UserTasks.FindAsync(taskId);

            if (task == null)
                throw new NotFoundException(new[] { "Task not found." });

            task.Done = true;

            await _dataContext.SaveChangesAsync();
        }

        public async Task<(List<Guid> Assigned, List<Guid> NotAssigned)> CreateTask(UserTask task, List<Guid> assignTo)
        {
            var nonExistingUsers = new List<Guid>();
            var usersToInform = new List<User>();

            await _dataValidator.Validate(task);

            assignTo.ForEach(async uid => {
                var user = await _dataContext.Users.FindAsync(uid);
                if (user == null) {
                    nonExistingUsers.Add(uid);
                    return;
                }

                usersToInform.Add(user);
                task.Assignments.Add(new TaskAssignment()
                {
                    UserId = uid,
                    TaskId = task.TaskId
                });
            });

            _dataContext.UserTasks.Add(task);

            await _dataContext.SaveChangesAsync();

            usersToInform.ForEach(async u => {
                await _mailer.SendUserTasks(u.Email, $"{u.FirstName} {u.LastName}", new List<UserTask>() { task });
            });

            return (assignTo.Except(nonExistingUsers).ToList(), nonExistingUsers);
        }

        public async Task DeleteTask(Guid taskId)
        {
            var assignmetsToRemove = await _dataContext.TaskAssignments.Where(a => a.TaskId == taskId).ToListAsync();

            if (assignmetsToRemove.Any()) {
                _dataContext.TaskAssignments.RemoveRange(assignmetsToRemove);
            }

            var taskToRemove = await _dataContext.UserTasks.FindAsync(taskId);

            if (taskToRemove != null)
            {
                _dataContext.UserTasks.Remove(taskToRemove);
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task<UserTask> GetTask(Guid taskId)
        {
            var task = await _dataContext.UserTasks.FindAsync(taskId);

            if (task == null) {
                throw new NotFoundException(new[] { "Task not found." });
            }

            return task;
        }

        public async Task<List<UserTask>> GetTasks()
        {
            var tasks = await _dataContext.UserTasks.OrderByDescending(t => t.TargetDate).ToListAsync();

            return tasks;
        }

        public async Task<List<UserTask>> GetTasksByEmail(string email)
        {
            var user = await _dataContext.Users.Where(u => u.Email == email).SingleOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundException(new[] { "User not found." });
            }

            var tasks = await _dataContext
                .UserTasks
                .Where(t => t.Assignments.Where(a => a.UserId == user.UserId).Any())
                .OrderByDescending(t => t.TargetDate)
                .ToListAsync()
            ;

            return tasks;
        }

        public async Task UpdateTask(Guid taskId, UpdateTask update)
        {
            var task = await _dataContext.UserTasks.FindAsync(taskId);

            if (task == null)
            {
                throw new NotFoundException(new[] { "Task not found." });
            }

            task.Name = update.Name;
            task.TargetDate = update.TargetDate;
            task.Priority = update.Priority;

            await _dataContext.SaveChangesAsync();
        }
    }
}
