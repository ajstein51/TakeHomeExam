using System.ComponentModel.DataAnnotations;

namespace TaskList.Infastructure.Entities;
public class TaskList
{
    // Id of the tasklist
    [Key]
    public long Id { get; set; }
    // Id of the person who made the tasklist (aka owner of the tasklist)
    public int PersonId { get; set; }
    
    // title of the tasklist
    [StringLength(100)]
    public string Title { get; set; }
    // description of the tasklist
    public string Description { get; set;}

    // date of creation
    [DataType(DataType.Date)]
    public DateTime Created { get; set; }
    // date the user would like for due date
    // used in email alerts if 
    [DataType(DataType.Date)]
    public DateTime? Deadline { get; set; }
    // date of completion
    [DataType(DataType.Date)]
    public DateTime? Completed { get; set; }

    // how high of a priority this tasklist is
    public Priority Priority { get; set; }
    // current status of the task list
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