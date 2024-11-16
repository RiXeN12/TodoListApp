namespace TodoListApp.Models
{
    public static class TaskListViewModelExtensions
    {
        public static bool HasAnyFilters(this TaskListViewModel model)
        {
            return !string.IsNullOrEmpty(model.CurrentCategory) ||
                   !string.IsNullOrEmpty(model.CurrentStatus) ||
                   !string.IsNullOrEmpty(model.CurrentPriority) ||
                   model.CurrentDueDate.HasValue ||
                   !string.IsNullOrEmpty(model.CurrentSortOrder);
        }
    }
}
