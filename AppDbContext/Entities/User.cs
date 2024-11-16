using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace TodoListApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<TaskList> TaskLists { get; set; } = new List<TaskList>();
    }
}
