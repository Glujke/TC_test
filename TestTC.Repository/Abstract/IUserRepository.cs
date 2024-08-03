using TC.Repository.Entity;

namespace TC.Repository.Abstract;

public interface IUserRepository
{
    Task AddUser(User user);
    Task EditUser(User user);
    Task EditUser(int id);
    Task RemoveUser(int id);
    Task RemoveUser(User user);
	Task<User> GetUser(int id);
    Task<IEnumerable<User>> GetAll { get; } 
}
