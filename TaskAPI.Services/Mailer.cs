using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Common.Enums;
using TaskAPI.Common.Options;
using TaskAPI.Data.Models;
using TaskAPI.Services.Interfaces;

namespace TaskAPI.Services
{
    public class Mailer : IMailer
    {
        private readonly MailerOptions _mailerOptions;

        public Mailer(IOptions<MailerOptions> mailerOprtions) {
            _mailerOptions = mailerOprtions.Value;
        }

        public async Task SendAccountCreated(string email, string fullname)
        {
            var mailClient = GetSmtpClient(_mailerOptions);
            var message = ComposeMessage(
                $"TaskAPI - Account for {fullname} was created.",
                email,
                $"{fullname} hello, your account for TaskAPI was successfully created."
            );

            await mailClient.SendMailAsync(message);
        }

        public async Task SendPasswordChanged(string email, string fullname)
        {
            var mailClient = GetSmtpClient(_mailerOptions);
            var message = ComposeMessage(
                $"TaskAPI - Password for account {email} was changed.",
                email,
                $"{fullname} hello, your account password TaskAPI was successfully changed."
            );

            await mailClient.SendMailAsync(message);
        }

        public async Task SendRecoverPassword(string email, string fullname, string token)
        {
            var mailClient = GetSmtpClient(_mailerOptions);
            var message = ComposeMessage(
                $"TaskAPI - Password recovery request for {email}.",
                email,
                $"{fullname} hello, TaskAPI created token for ypur password resovery request: {token}."
            );

            await mailClient.SendMailAsync(message);
        }

        private static string GetPriorityString(Priority priority) {
            switch (priority)
            {
                case Priority.High:
                    return "High priority";
                case Priority.Medium:
                    return "Medium priority";
                default:
                    return "Low priority";
            }
        }

        public async Task SendUserTasks(string email, string fullname, IEnumerable<UserTask> tasks)
        {
            var mailClient = GetSmtpClient(_mailerOptions);
            var taskList = tasks
                .OrderByDescending(t => t.TargetDate)
                .Select(t => $"{t.Name}({GetPriorityString(t.Priority)}) should be completed by {t.TargetDate:YYYY-MM-dd}")
                .ToArray()
            ;

            var message = ComposeMessage(
                $"TaskAPI - Tasks for {email}.",
                email,
                $"{fullname} hello, tasks was shared with you: <br /> {string.Join("<br />", taskList)}."
            );

            await mailClient.SendMailAsync(message);
        }

        private static SmtpClient GetSmtpClient(MailerOptions config) {
            var client = new SmtpClient(config.SmtpServer, config.Port)
            {
                Credentials = new NetworkCredential(config.Username, config.Password)
            };

            return client;
        }

        private MailMessage ComposeMessage(string subject, string to, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_mailerOptions.From),
                BodyEncoding = Encoding.UTF8,
                Body = body,
                Subject = subject,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            return mailMessage;
        }
    }
}
