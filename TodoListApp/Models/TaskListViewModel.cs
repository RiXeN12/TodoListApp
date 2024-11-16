namespace TodoListApp.Models
{
    public class TaskListViewModel
    {
        public List<TaskItem> Tasks { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public string CurrentCategory { get; set; }
        public string CurrentStatus { get; set; }
        public string CurrentPriority { get; set; }
        public string CurrentSortOrder { get; set; }
        public DateTime? CurrentDueDate { get; set; }
    }
}
