using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TC.Repository.Abstract;
using TC.Repository.Entity;
using TC.Repository.Implementation;

namespace TC.Controllers;

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
        if (ModelState.IsValid)
        {
            await toDoItemRepository.AddToDoItem(toDoItem);
            return RedirectToAction(nameof(ShowAll));
        }
        return View("Error");
    }
    public async Task<IActionResult> ShowAll()
    {
        var res = await toDoItemRepository.GetAll;
        return View(res);
    }
}