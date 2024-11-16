using TodoListApp.Models;

namespace TodoListApp.Repositories.TodoRepository
{
    public interface ITaskRepository
    {
        Task AddAsync(TaskItem taskItem);
        Task AddTaskListAsync(TaskList taskList);
        Task DeleteAsync(int id);
        Task<IEnumerable<TaskItem>> GetAllByUserIdAsync(int userId);
        Task<IEnumerable<string>> GetAllCategoriesAsync(int userId);
        Task<TaskItem> GetByIdAsync(int id);
        Task<TaskList> GetTaskListByUserIdAsync(int userId);
        Task UpdateAsync(TaskItem taskItem);
    }
}