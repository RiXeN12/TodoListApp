﻿using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models
{
    public class TaskList
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UserId { get; set; }

        public User User { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
