using TaskList.Domain.Repositories;

namespace TaskList.Domain.Services;

public class TaskListService
{
    private readonly ITaskListRepository _repository;

    public TaskListService(ITaskListRepository repository)
    {
        _repository = repository;
    }
    public Task<Entities.TaskList?> GetAsync(int id, int personId)
    {
        return _repository.GetAsync(id, personId);
    }

    public Task<List<Entities.TaskList>> ListAsync()
    {
        return _repository.ListAsync();
    }

    public Task CreateAsync(Entities.TaskList taskList)
    {
        return _repository.CreateAsync(taskList);
    }

    public Task UpdateAsync(Entities.TaskList taskList)
    {
        return (_repository.UpdateAsync(taskList));
    }

    public Task DeleteAsync(Entities.TaskList taskList)
    {
        return _repository.DeleteAsync(taskList);
    }
}