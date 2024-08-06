using System.ComponentModel.DataAnnotations;

namespace TC.Repository.Entity;
public class User
{
    [Key]
    public int Id { get; set; }

	[Required]
	[Display(Name = "Имя исполнителя")]
    public string Name { get; set; }

    public IEnumerable<ToDoItem>? TodoItems { get; set; } = null;
}

