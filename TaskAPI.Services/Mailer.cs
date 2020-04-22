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
using MimeKit;

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
            var message = ComposeMessage(
                $"TaskAPI - Account for {fullname} was created.",
                email,
                $"{fullname} hello, your account for TaskAPI was successfully created."
            );

            await Send(message);
        }

        public async Task SendPasswordChanged(string email, string fullname)
        {
            var message = ComposeMessage(
                $"TaskAPI - Password for account {email} was changed.",
                email,
                $"{fullname} hello, your account password TaskAPI was successfully changed."
            );

            await Send(message);
        }

        public async Task SendRecoverPassword(string email, string fullname, string token)
        {
            var message = ComposeMessage(
                $"TaskAPI - Password recovery request for {email}.",
                email,
                $"{fullname} hello, TaskAPI created token for ypur password resovery request: {token}."
            );

            await Send(message);
        }

        private static string GetPriorityString(Priority priority) {
            return priority switch
            {
                Priority.High => "High priority",
                Priority.Medium => "Medium priority",
                _ => "Low priority",
            };
        }

        public async Task SendUserTasks(string email, string fullname, IEnumerable<UserTask> tasks)
        {
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

            await Send(message);
        }

        private async Task Send(MimeMessage message) {
            using var client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect(_mailerOptions.SmtpServer, _mailerOptions.SmtpPort, _mailerOptions.EnableSsl);
            client.Authenticate(_mailerOptions.SmtpUsername, _mailerOptions.SmtpPassword);

            await client.SendAsync(message);
            client.Disconnect(true);
        }

        private MimeMessage ComposeMessage(string subject, string to, string body)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("", _mailerOptions.FromAddress));
            mimeMessage.To.Add(new MailboxAddress(to));
            mimeMessage.Subject = subject;

            var bodyBuilder = new MimeKit.BodyBuilder {
                HtmlBody = body
            };

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            return mimeMessage;
        }
    }
}
