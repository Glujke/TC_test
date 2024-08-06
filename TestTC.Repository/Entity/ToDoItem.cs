﻿using System.ComponentModel.DataAnnotations;
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
    public int? UserId { get; set; }

    [Display(Name = "Приоритет")]
    [ForeignKey("PriorityId")]
    public Priority? Priority { get; set; }

    [Display(Name = "Приоритет")]
    public int PriorityId { get; set; }

    [NotMapped]
    public string Date => DueDate.ToString("dd.MM.yyyy");

    public void CopyFrom(ToDoItem copyItem)
    {
        this.Title = copyItem.Title;
        this.Description = copyItem.Description;
        this.Priority = copyItem.Priority;
        this.PriorityId= copyItem.PriorityId;
        this.User = copyItem.User;
        this.UserId = copyItem.UserId;
        this.IsCompleted = copyItem.IsCompleted;
        this.DueDate = copyItem.DueDate;
    }

}
