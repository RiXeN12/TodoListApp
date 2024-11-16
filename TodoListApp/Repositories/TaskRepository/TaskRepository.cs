using Library_NPR321.Repositories;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Models;
using TodoListMain;

namespace TodoListApp.Repositories.TodoRepository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TodoDbContext _context;

        public TaskRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllByUserIdAsync(int userId)
        {
            return await _context.TaskItems
                .Include(t => t.TaskList)
                .Where(t => t.TaskList.UserId == userId)
                .ToListAsync();
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            return await _context.TaskItems
                .Include(t => t.TaskList)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskList> GetTaskListByUserIdAsync(int userId)
        {
            return await _context.TaskLists
                .Include(tl => tl.Tasks)
                .FirstOrDefaultAsync(tl => tl.UserId == userId);
        }

        public async Task AddTaskListAsync(TaskList taskList)
        {
            await _context.TaskLists.AddAsync(taskList);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(TaskItem taskItem)
        {
            var taskList = await _context.TaskLists.FindAsync(taskItem.TaskListId);
            if (taskList == null)
            {
                throw new InvalidOperationException($"TaskList with ID {taskItem.TaskListId} not found");
            }

            var newTaskItem = new TaskItem
            {
                Title = taskItem.Title,
                Description = taskItem.Description,
                IsCompleted = taskItem.IsCompleted,
                Priority = taskItem.Priority,
                DueDate = taskItem.DueDate,
                Category = taskItem.Category,
                TaskListId = taskItem.TaskListId
            };

            await _context.TaskItems.AddAsync(newTaskItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            var existingTask = await _context.TaskItems.FindAsync(taskItem.Id);
            if (existingTask != null)
            {
                _context.Entry(existingTask).CurrentValues.SetValues(taskItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<string>> GetAllCategoriesAsync(int userId)
        {
            return await _context.TaskItems
                .Include(t => t.TaskList)
                .Where(t => t.TaskList.UserId == userId)
                .Select(t => t.Category)
                .Distinct()
                .Where(c => !string.IsNullOrEmpty(c))
                .ToListAsync();
        }

    }
}