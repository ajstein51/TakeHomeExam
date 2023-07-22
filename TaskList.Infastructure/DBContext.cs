using Microsoft.EntityFrameworkCore;
using TaskList.Infastructure.Entities;

namespace TaskList.Infastructure;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options) {}
    public DbSet<Entities.TaskList> TaskList { get; set; }
    public DbSet<TaskItem> TaskItem{ get; set; }

}
