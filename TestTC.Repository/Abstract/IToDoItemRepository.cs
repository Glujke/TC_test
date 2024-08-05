using TC.Repository.Entity;
using TestTC.Repository.Filters;

namespace TC.Repository.Abstract;

public interface IToDoItemRepository
{
    Task AddToDoItem(ToDoItem toDoItem);
    Task EditToDoItem(ToDoItem toDoItem);
    Task EditToDoItem(int id);
    Task RemoveToDoItem (ToDoItem toDoItem);
    Task RemoveToDoItem(int id);
    Task<ToDoItem> GetToDoItem(int id);
    Task<IEnumerable<ToDoItem>> GetAll { get; }
    Task<IEnumerable<ToDoItem>> GetFromFilter(Filter filter);
}
