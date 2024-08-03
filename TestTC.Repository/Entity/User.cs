using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

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

