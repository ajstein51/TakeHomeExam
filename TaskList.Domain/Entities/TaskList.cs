using System.ComponentModel.DataAnnotations;

namespace TaskList.Domain.Entities;

public class TaskList
{
    public long Id { get; set; }
    // Id of the person who made the tasklist (aka owner of the tasklist)
    public int PersonId { get; set; }
    
    [StringLength(100)]
    public required string Title { get; set; }
    public required string Description { get; set;}


    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yy")]
    public DateTime Created { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yy")]
    public DateTime? Deadline { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yy")]
    public DateTime? Completed { get; set; }

    public Priority Priority { get; set; }
    public Status Status { get; set; }

    // navigational properties
    public ICollection<ListTask> ListTasks { get; set; } = new HashSet<ListTask>();
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