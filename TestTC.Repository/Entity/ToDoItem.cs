using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TC.Repository.Entity;

public class ToDoItem
{
    public int Id { get; set; }

    [Display(Name = "Заголовок")]
    public string Title { get; set; }

    [Display(Name = "Описание")]
    public string Description { get; set; }

    [Display(Name = "Готовность")]
    public bool IsCompleted { get; set; }

    [Display(Name = "Дата выполнения")]

    public DateTime DueDate { get; set; }

    [Display(Name = "Исполнитель")]
    [ForeignKey("UserId")]
    public User? User { get; set; }

    [Display(Name = "Исполнитель")]
    public int UserId { get; set; }

    [Display(Name = "Приоритет")]
    [ForeignKey("PriorityId")]
    public Priority? Priority { get; set; }

    [Display(Name = "Приоритет")]
    public int PriorityId { get; set; }

}
