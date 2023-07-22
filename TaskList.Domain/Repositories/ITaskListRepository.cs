
namespace TaskList.Domain.Repositories;

public interface ITaskListRepository : IRepository<Entities.TaskList>
{
    public Task<Entities.TaskList?> GetAsync(int id, int personId);

    public Task<List<Entities.TaskList>> ListAsync();

    public Task CreateAsync(Entities.TaskList taskList);
    public Task UpdateAsync(Entities.TaskList taskList);
    public Task DeleteAsync(Entities.TaskList taskList);
}