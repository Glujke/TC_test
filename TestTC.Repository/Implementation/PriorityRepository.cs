using Microsoft.EntityFrameworkCore;
using TC.Repository.Abstract;
using TC.Repository.Context;
using TC.Repository.Entity;

namespace TC.Repository.Implementation;

public class PriorityRepository : IPriorityRepository
{
    private readonly TestTCDbContext mainContext;

    public PriorityRepository(TestTCDbContext mainContext)
    {
        this.mainContext = mainContext;
    }   

    public async Task AddPriority(Priority priority)
    {
        await mainContext.AddAsync(priority);
        await mainContext.SaveChangesAsync();
    }

    public async Task EditPriority(Priority priority)
    {
        mainContext.Update(priority);
        await mainContext.SaveChangesAsync();
    }

    public async Task RemovePriority(Priority priority)
    {
        mainContext.Remove(priority);
        await mainContext.SaveChangesAsync();
    }

    public async Task RemovePriority(int id)
    {
        var priority = await GetFromId(id);
        mainContext.Remove(priority);
        await mainContext.SaveChangesAsync();
    }
    public Task<IEnumerable<Priority>> GetAll => Task.FromResult<IEnumerable<Priority>>(mainContext.Priorities);
    public Task<Priority> GetFromId(int id) => 
        mainContext.Priorities.Include(td => td.TodoItems).Where(p => p.Id == id).FirstOrDefaultAsync();

}
