using Microsoft.EntityFrameworkCore;
using TC.Repository.Abstract;
using TC.Repository.Context;
using TC.Repository.Entity;

namespace TC.Repository.Implementation;

public class ToDoItemRepository : IToDoItemRepository
{
    private readonly TestTCDbContext mainContext;

    public ToDoItemRepository(TestTCDbContext mainContext)
    {
        this.mainContext = mainContext;
    }

    public Task<IEnumerable<ToDoItem>> GetAll => Task.FromResult<IEnumerable<ToDoItem>>(mainContext.ToDoItems.
        Include(td => td.User).Include(td => td.Priority));

    public Task<ToDoItem> GetToDoItem(int id) =>
        mainContext.ToDoItems.Include(td  => td.User)
        .Include(td => td.Priority).Where(p => p.Id == id).FirstOrDefaultAsync();

    public async Task AddToDoItem(ToDoItem toDoItem)
    {
        await mainContext.AddAsync(toDoItem);
        await mainContext.SaveChangesAsync();
    }

    public async Task EditToDoItem(ToDoItem toDoItem)
    {
        mainContext.Update(toDoItem);
        await mainContext.SaveChangesAsync();
    }

    public async Task EditToDoItem(int id)
    {
        var toDoItem = await GetToDoItem(id);
        mainContext.Update(toDoItem);
        await mainContext.SaveChangesAsync();
    }


    public async Task RemoveToDoItem(ToDoItem toDoItem)
    {
        mainContext.Remove(toDoItem);
        await mainContext.SaveChangesAsync();
    }

    public async Task RemoveToDoItem(int id)
    {
        var toDoItem = await GetToDoItem(id);
        mainContext.Remove(toDoItem);
        await mainContext.SaveChangesAsync();
    }
}
