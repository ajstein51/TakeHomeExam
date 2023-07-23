using System.ComponentModel.DataAnnotations;

namespace TaskList.Infastructure.Entities;

public class TaskItem
{
    // Id of the taskItem
    [Key]
    public long Id { get; set; }
    // Id of the TaskList its attached to
    public long TaskListId { get; set; }
    // Description of the taskitem (what the item is)
    public string Description { get; set; }
    // notes the user may have of the item
    public string? Note { get; set; }

    // date the item was created
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yy")]
    public DateTime Created { get; set; }
    // date the item was completed
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yy")]
    public DateTime? Completed { get; set; }

    // navigational properties
#nullable disable
    public TaskList TaskList { get; set; }
#nullable enable
}