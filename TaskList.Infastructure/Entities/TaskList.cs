using System.ComponentModel.DataAnnotations;

namespace TaskList.Infastructure.Entities;
public class TaskList
{
    [Key]
    public long Id { get; set; }
    // Id of the person who made the tasklist (aka owner of the tasklist)
    public int PersonId { get; set; }
    
    [StringLength(100)]
    public string Title { get; set; }
    public string Description { get; set;}


    [DataType(DataType.Date)]
    public DateTime Created { get; set; }
    [DataType(DataType.Date)]
    public DateTime? Deadline { get; set; }
    [DataType(DataType.Date)]
    public DateTime? Completed { get; set; }

    public Priority Priority { get; set; }
    public Status Status { get; set; }

    // navigational properties
    public ICollection<TaskItem> TaskItems { get; set; } = new HashSet<TaskItem>();
}

public enum Priority
{
    Low,
    Medium,
    High
};

public enum Status
{
    NotStarted,
    Started,
    Completed
};