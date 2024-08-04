using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TC.Repository.Entity;

public class Priority
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Приоритет")]
    [Range(1,5)]
    [Obsolete]
    public int Level { get; set; }
    public IEnumerable<ToDoItem>? TodoItems { get; set; } = null;
}

