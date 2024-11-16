using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TodoListApp.Models;

namespace TodoListMain
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.TaskLists)
                .WithOne(tl => tl.User)
                .HasForeignKey(tl => tl.UserId);

            modelBuilder.Entity<TaskItem>()
               .HasOne(t => t.TaskList)
               .WithMany(tl => tl.Tasks)
               .HasForeignKey(t => t.TaskListId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskList>()
                .HasOne(tl => tl.User)
                .WithMany(u => u.TaskLists)
                .HasForeignKey(tl => tl.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
