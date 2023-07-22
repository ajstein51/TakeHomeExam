using Microsoft.EntityFrameworkCore;
using TaskList.Infastructure.Entities;

namespace TaskList.Infastructure.Services;

public class TaskListService
{
    private IDbContextFactory<DBContext> _context;

    public TaskListService(IDbContextFactory<DBContext> context) => _context = context;

    //public Task<Data.Models.TaskList?> GetAsync(int id, int personId)
    //{
    //    return _repository.GetAsync(id, personId);
    //}

    //public Task<List<Data.Models.Entities.TaskList>> ListAsync()
    //{
    //    return _repository.ListAsync();
    //}

    public async Task CreateAsync(Entities.TaskList taskList)
    {
        //using (var context = _context.CreateDbContext())
        //{
        //    context.TaskList.Add(taskList);
        //    context.SaveChanges();
        //}
        var context = _context.CreateDbContext();
        context.TaskList.Add(taskList);
        await context.SaveChangesAsync();
    }

    //    public Task UpdateAsync(Data.Models.TaskList taskList)
    //    {
    //        return (_repository.UpdateAsync(taskList));
    //    }

    //    public Task DeleteAsync(Data.Models.TaskList taskList)
    //    {
    //        return _repository.DeleteAsync(taskList);
    //    }
}
