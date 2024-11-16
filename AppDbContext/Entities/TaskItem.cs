using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoListApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва є обов'язковою")]
        [StringLength(100, ErrorMessage = "Назва не може бути довшою за 100 символів")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Опис не може бути довшим за 500 символів")]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        [Range(1, 3, ErrorMessage = "Пріоритет має бути від 1 до 3")]
        public int Priority { get; set; }

        public DateTime? DueDate { get; set; }

        [StringLength(50, ErrorMessage = "Категорія не може бути довшою за 50 символів")]
        public string Category { get; set; } = string.Empty;

        public int TaskListId { get; set; }

        [JsonIgnore]
        public virtual TaskList? TaskList { get; set; }
    }
}
