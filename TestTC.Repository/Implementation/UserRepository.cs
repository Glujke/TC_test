using Microsoft.EntityFrameworkCore;
using TC.Repository.Abstract;
using TC.Repository.Context;
using TC.Repository.Entity;

namespace TC.Repository.Implementation;

public class UserRepository : IUserRepository
{
	private readonly TestTCDbContext mainContext;

	public UserRepository(TestTCDbContext mainContext)
	{
		this.mainContext = mainContext;
	}

	public Task<IEnumerable<User>> GetAll => Task.FromResult<IEnumerable<User>>(mainContext.Users);

	public async Task AddUser(User user)
	{
		await mainContext.AddAsync(user);
		await mainContext.SaveChangesAsync();
	}

    public async Task RemoveUser(int id)
	{
		var user = await GetUser(id);
		mainContext.Users.Remove(user);
		await mainContext.SaveChangesAsync();
	}
	public async Task RemoveUser(User user)
	{
		mainContext.Users.Remove(user);
		await mainContext.SaveChangesAsync();
	}

	public async Task EditUser(User user)
	{
		mainContext.Users.Update(user);
		await mainContext.SaveChangesAsync();
	}

    public async Task EditUser(int id)
	{
		var user = await GetUser(id);
		mainContext.Users.Update(user);
		await mainContext.SaveChangesAsync();
	}

	public async Task<User> GetUser(int id) => await mainContext.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

}
