using TC.Repository.Entity;

namespace TC.Repository.Abstract;

public interface IPriorityRepository
{
    Task AddPriority(Priority priority);
    Task EditPriority(Priority priority);
    Task RemovePriority(Priority priority);
    Task RemovePriority(int id);
    Task<IEnumerable<Priority>> GetAll { get; }
    Task<Priority> GetFromId(int id);
}
