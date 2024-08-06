using System.ComponentModel.DataAnnotations;

namespace TC.Repository.Entity;


public class Priority
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Приоритет")]
    [Range(1, 10)]
    [Obsolete]
    public int Level { get; set; }
    public IEnumerable<ToDoItem>? TodoItems { get; set; } = null;
}

