using Microsoft.EntityFrameworkCore;

using TaskList.Infastructure.Entities;

namespace TaskList.Infastructure.Services;

public class TaskListService
{
    private IDbContextFactory<DBContext> _context;

    public TaskListService(IDbContextFactory<DBContext> context) => _context = context;

    public async Task<Entities.TaskList?> GetAsync(int Id)
    {
        var context = _context.CreateDbContext();
        return context.TaskList
            .Include(tl => tl.TaskItems)
            .FirstOrDefault(tl =>  tl.Id == Id);
    }
    public async Task<Entities.TaskList?> GetAsync(Entities.TaskList taskList)
    {
        var context = _context.CreateDbContext();
        return await context.TaskList.FirstOrDefaultAsync(tl 
            => tl.PersonId == taskList.PersonId
            && tl.Title == taskList.Title);
    }

    public async Task<List<Entities.TaskList>> ListAsync()
    {
        var context = _context.CreateDbContext();
        return await context.TaskList.ToListAsync();
    }

    public async Task CreateAsync(Entities.TaskList taskList)
    {
        var context = _context.CreateDbContext();
        context.TaskList.Add(taskList);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Entities.TaskList taskList)
    {
        var context = _context.CreateDbContext();
        // get status
        if (taskList.TaskItems.Any(ti => ti.Completed != null))
            taskList.Status = Status.Started;
        else
            taskList.Status = Status.NotStarted;
        // check if completed
        if (taskList.TaskItems.Count() == taskList.TaskItems.Where(ti => ti.Completed != null).Count())
        {
            taskList.Completed = DateTime.Now;
            taskList.Status = Status.Completed;
        }
        // they removed or unchecked that a task was completed so it no longer is done
        else if (taskList.Completed != null)
            taskList.Completed = null;
        context.TaskList.Update(taskList);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Entities.TaskList taskList)
    {
        var context = _context.CreateDbContext();
        context.TaskList.Remove(taskList);
        await context.SaveChangesAsync();
    }
}
