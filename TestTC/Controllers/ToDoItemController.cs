using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using System;
using TC.Models;
using TC.Repository.Abstract;
using TC.Repository.Entity;
using TC.Repository.Implementation;
using TestTC.Repository.Enums;
using TestTC.Repository.Filters;

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
        try
        {
            var users = await userRepository.GetAll;
            var priorities = await priorityRepository.GetAll;
            ViewData["UserId"] = new SelectList(users, "Id", "Name");
            ViewData["PriorityId"] = new SelectList(priorities, "Id", "Level");
            return View();
        } catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add([Bind("Title, Description, DueDate, UserId, PriorityId")] ToDoItem toDoItem)
    {
        try
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
        } catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
    }
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var toDoItem = await toDoItemRepository.GetToDoItem(id);
            var users = await userRepository.GetAll;
            var priorities = await priorityRepository.GetAll;
            ViewData["UserId"] = new SelectList(users, "Id", "Name");
            ViewData["PriorityId"] = new SelectList(priorities, "Id", "Level");
            return View(toDoItem);
        } catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id, Title, Description, IsCompleted, DueDate, UserId, PriorityId")] ToDoItem toDoItem)
    {
        try
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
        } catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
    }
    public async Task<IActionResult> Show(int id)
    {
        try
        {
            var toDoItem = await toDoItemRepository.GetToDoItem(id);
            var users = await userRepository.GetAll;
            ViewData["UserId"] = new SelectList(users, "Id", "Name");
            return View(toDoItem);
        } catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
    }

    public async Task<IActionResult> FinishTask(int id)
    {
        try
        {
            var toDoItem = await toDoItemRepository.GetToDoItem(id);
            toDoItem.IsCompleted = true;
            await toDoItemRepository.EditToDoItem(toDoItem);

            return RedirectToAction("Show", new { id = id });
        } catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
    }
    [HttpPost]
    public async Task<IActionResult> Assign(int id, [Bind("Id, Title, Description, IsCompleted, DueDate, UserId, PriorityId")] ToDoItem toDoItem)
    {
        try
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
            return RedirectToAction("Show", new { id = id });
        } catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }

    }

    public async Task<IActionResult> ShowAll()
    {
        try
        {
            var res = await toDoItemRepository.GetAll;
            var priorities = await priorityRepository.GetAll;
            ViewBag.PrioritiesList = new SelectList(priorities, "Id", "Level");
            return View((ToDoItems: res, Priorities: priorities));
        } catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
    }
    public async Task<IActionResult> ShowWithFilter((IEnumerable<ToDoItem> toDoItems, IEnumerable<Priority> priority) result)
    {
        try
        {
            var priorities = await priorityRepository.GetAll;
            ViewBag.PrioritiesList = new SelectList(priorities, "Id", "Level");
            return View((ToDoItems: result.toDoItems, Priorities: priorities));
        } catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ShowWithFilter(DateTime? dueDate, MoreEqualsLess moreEqualsLessDueDate,
        int? idPriority, MoreEqualsLess moreEqualsLessPriority, bool? isReady)
    {
        try
        {
            var filter = new Filter(dueDate, moreEqualsLessDueDate, idPriority, moreEqualsLessPriority, isReady);
            var res = await toDoItemRepository.GetFromFilter(filter);
            var priorities = await priorityRepository.GetAll;

            return await ShowWithFilter((ToDoItems: res, Priorities: priorities));
        } catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
    }

}