﻿using Microsoft.EntityFrameworkCore;

using TaskList.Infastructure.Entities;

namespace TaskList.Infastructure.Services;

public  class TaskItemService
{
    private IDbContextFactory<DBContext> _context;

    public TaskItemService(IDbContextFactory<DBContext> context) => _context = context;

    public async Task CreateAsync(List<Entities.TaskItem> taskItems)
    {
        var context = _context.CreateDbContext();
        foreach(var taskItem in taskItems)
        {
            taskItem.Created = DateTime.Now;
            context.TaskItem.Add(taskItem);
        }
        await context.SaveChangesAsync();
    }
    public async Task UpdateAsync(List<Entities.TaskItem> taskItems)
    {
        var context = _context.CreateDbContext();
        foreach (var taskItem in taskItems)
        {
            taskItem.Created = DateTime.Now;
            context.TaskItem.Update(taskItem);
        }
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Entities.TaskItem taskItem)
    {
        var context = _context.CreateDbContext();
        context.TaskItem.Remove(taskItem);
        await context.SaveChangesAsync();
    }
}
