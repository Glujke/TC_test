using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace TC.Repository.Entity;


public class Priority
{
    private string[] priorities = {"Не важно", "Важно","Очеьн важно","Срочно","Очень срочно"};

    [Key]
    public int Id { get; set; }

    [Display(Name = "Приоритет")]
    [Range(1, 5)]
    [Obsolete]
    public int Level { get; set; }
    public IEnumerable<ToDoItem>? TodoItems { get; set; } = null;

    public string LevelString(int idStr) => priorities[idStr];
}

