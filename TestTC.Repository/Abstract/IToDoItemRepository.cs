using TC.Repository.Entity;

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
}
