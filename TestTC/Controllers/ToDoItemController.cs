using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using TC.Repository.Abstract;
using TC.Repository.Entity;
using TC.Repository.Implementation;

namespace TC.Controllers;

public enum MoreEqualsLess
{
    Less,
    Equals,
    More
}

public class ToDoItemController : Controller
{
    private readonly IToDoItemRepository toDoItemRepository;
    private readonly IUserRepository userRepository;
    private readonly IPriorityRepository priorityRepository;

    public ToDoItemController(IToDoItemRepository toDoItemRepository, IUserRepository userRepository,
        IPriorityRepository priorityRepository)
    {
        this.toDoItemRepository = toDoItemRepository;
        this.userRepository = userRepository;
        this.priorityRepository = priorityRepository;
    }

    public async Task<IActionResult> Add()
    {
        var users = await userRepository.GetAll;
        var priorities = await priorityRepository.GetAll;
        ViewData["UserId"] = new SelectList(users, "Id", "Name");
        ViewData["PriorityId"] = new SelectList(priorities, "Id", "Level");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add([Bind("Title, Description, DueDate, UserId, PriorityId")] ToDoItem toDoItem)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("",
                    "Не все данные заполнены");
            var users = await userRepository.GetAll;
            var priorities = await priorityRepository.GetAll;
            ViewData["UserId"] = new SelectList(users, "Id", "Name");
            ViewData["PriorityId"] = new SelectList(priorities, "Id", "Level");
            return View(toDoItem);
        }
        if (toDoItem.DueDate < DateTime.Now)
        {
            ModelState.AddModelError("",
                    "Дата выполнения не может быть раньше текущей.");
            var users = await userRepository.GetAll;
            var priorities = await priorityRepository.GetAll;
            ViewData["UserId"] = new SelectList(users, "Id", "Name");
            ViewData["PriorityId"] = new SelectList(priorities, "Id", "Level");
            return View(toDoItem);
        }
        await toDoItemRepository.AddToDoItem(toDoItem);
        return RedirectToAction(nameof(ShowAll));
    }
    public async Task<IActionResult> Edit(int id)
    {
        var toDoItem = await toDoItemRepository.GetToDoItem(id);
        var users = await userRepository.GetAll;
        var priorities = await priorityRepository.GetAll;
        ViewData["UserId"] = new SelectList(users, "Id", "Name");
        ViewData["PriorityId"] = new SelectList(priorities, "Id", "Level");
        return View(toDoItem);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id, Title, Description, DueDate, UserId, PriorityId")] ToDoItem toDoItem)
    {
        if (id != toDoItem.Id)
        {
            ModelState.AddModelError("",
                    "ID не совпадает.");
            return View(toDoItem);
        }
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("",
                    "Не все данные заполнены");
            var users = await userRepository.GetAll;
            var priorities = await priorityRepository.GetAll;
            ViewData["UserId"] = new SelectList(users, "Id", "Name");
            ViewData["PriorityId"] = new SelectList(priorities, "Id", "Level");
            return View(toDoItem);
        }
        if (toDoItem.DueDate < DateTime.Now) 
        {
            ModelState.AddModelError("",
                    "Дата выполнения не может быть раньше текущей.");
            var users = await userRepository.GetAll;
            var priorities = await priorityRepository.GetAll;
            ViewData["UserId"] = new SelectList(users, "Id", "Name");
            ViewData["PriorityId"] = new SelectList(priorities, "Id", "Level");
            return View(toDoItem);
        }
        await toDoItemRepository.EditToDoItem(toDoItem);
        return RedirectToAction(nameof(ShowAll));
    }
    public async Task<IActionResult> Show(int id)
    {
        var toDoItem = await toDoItemRepository.GetToDoItem(id);
        return View(toDoItem);
    }

    public async Task<IActionResult> ShowAll()
    {
        var res = await toDoItemRepository.GetAll;
        var priorities = await priorityRepository.GetAll; 
        ViewBag.PrioritiesList = new SelectList(priorities, "Id", "Level");
        return View((ToDoItems: res, Priorities: priorities));
    }

    [HttpPost]
    public async Task<IActionResult> ShowWithFilter(DateTime? dueDate, MoreEqualsLess? moreEqualsLessDueDate, int? idPriority, MoreEqualsLess? moreEqualsLessPriority)
    {
        var res = await toDoItemRepository.GetAll;
        return View(res);
    }

}