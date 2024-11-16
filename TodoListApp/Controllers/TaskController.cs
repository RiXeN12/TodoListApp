using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using TodoListApp.Models;
using TodoListApp.Repositories.AccountRepository;
using TodoListApp.Repositories.TodoRepository;

namespace TodoListApp.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IActionResult> Index(string category, string status, string priority,
        string sortOrder, DateTime? dueDate)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var tasks = await _taskRepository.GetAllByUserIdAsync(userId.Value);

            if (!string.IsNullOrEmpty(category))
            {
                tasks = tasks.Where(t => t.Category == category);
            }

            if (!string.IsNullOrEmpty(status))
            {
                var isCompleted = status == "completed";
                tasks = tasks.Where(t => t.IsCompleted == isCompleted);
            }

            if (!string.IsNullOrEmpty(priority))
            {
                if (int.TryParse(priority, out int priorityValue))
                {
                    tasks = tasks.Where(t => t.Priority == priorityValue);
                }
            }

            if (dueDate.HasValue)
            {
                tasks = tasks.Where(t => t.DueDate?.Date == dueDate.Value.Date);
            }

            tasks = sortOrder switch
            {
                "priority_desc" => tasks.OrderByDescending(t => t.Priority),
                "priority_asc" => tasks.OrderBy(t => t.Priority),
                "due_date_desc" => tasks.OrderByDescending(t => t.DueDate),
                "due_date_asc" => tasks.OrderBy(t => t.DueDate),
                "category" => tasks.OrderBy(t => t.Category),
                _ => tasks.OrderBy(t => t.Priority)
            };

            var categories = await _taskRepository.GetAllCategoriesAsync(userId.Value);

            var viewModel = new TaskListViewModel
            {
                Tasks = tasks.ToList(),
                Categories = categories.ToList(),
                CurrentCategory = category,
                CurrentStatus = status,
                CurrentPriority = priority,
                CurrentSortOrder = sortOrder,
                CurrentDueDate = dueDate
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TaskItem());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TaskItem taskItem)
        {
            try
            {
                ModelState.Remove("TaskList");

                if (!ModelState.IsValid)
                {
                    return View(taskItem);
                }

                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var taskList = await _taskRepository.GetTaskListByUserIdAsync(userId.Value);
                if (taskList == null)
                {
                    taskList = new TaskList
                    {
                        Name = "Default List",
                        UserId = userId.Value
                    };
                    await _taskRepository.AddTaskListAsync(taskList);

                    taskList = await _taskRepository.GetTaskListByUserIdAsync(userId.Value);
                    if (taskList == null)
                    {
                        throw new Exception("Failed to create TaskList");
                    }
                }

                taskItem.TaskListId = taskList.Id;
                taskItem.TaskList = null;

                await _taskRepository.AddAsync(taskItem);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Виникла помилка при створенні завдання");
                return View(taskItem);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var taskItem = await _taskRepository.GetByIdAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            return View(taskItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskItem taskItem)
        {
            if (!ModelState.IsValid)
                return View(taskItem);

            await _taskRepository.UpdateAsync(taskItem);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var taskItem = await _taskRepository.GetByIdAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            return View(taskItem);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int id, [FromBody] bool isCompleted)
        {
            var taskItem = await _taskRepository.GetByIdAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            taskItem.IsCompleted = isCompleted;
            await _taskRepository.UpdateAsync(taskItem);
            return Ok();
        }
    }
}
