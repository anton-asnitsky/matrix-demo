using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Data.Models;

namespace TaskAPI.Services.Interfaces
{
    public interface IMailer
    {
        Task SendAccountCreated(string email, string fullname);
        Task SendRecoverPassword(string email, string fullname, string token);
        Task SendPasswordChanged(string email, string fullname);
        Task SendUserTasks(string email, string fullanem, IEnumerable<UserTask> tasks);
    }
}
