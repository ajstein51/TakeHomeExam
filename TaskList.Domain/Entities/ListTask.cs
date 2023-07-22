using System.ComponentModel.DataAnnotations;

namespace TaskList.Domain.Entities;

public class ListTask
{
    public long Id { get; set; }
    public long TaskListId { get; set; }

    public bool IsCompleted { get; set; }

    public string? Note { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yy")]
    public DateTime Created { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yy")]
    public DateTime Completed { get; set; }

#nullable disable
    public TaskList TaskList { get; set; }
#nullable enable
}