using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Data.Models;

namespace TaskAPI.Data.DataContexts
{
    public class DataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }


        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserTask> UserTasks { get; set; }
        public virtual DbSet<TaskAssignment> TaskAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<User>()
                .ToTable("Users")
            ;

            builder.Entity<UserTask>()
                .ToTable("Tasks")
            ;

            builder.Entity<TaskAssignment>()
                .ToTable("UsersToTasks")
                .HasKey(a => new { a.UserId, a.TaskId })
            ;

            base.OnModelCreating(builder);
        }
    }
}
