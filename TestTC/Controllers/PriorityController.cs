using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TC.Models;
using TC.Repository.Abstract;
using TC.Repository.Entity;

namespace TC.Controllers;

public class PriorityController : Controller
{
    private readonly IPriorityRepository priorityRepository;

    public PriorityController(IPriorityRepository priorityRepository)
    {
        this.priorityRepository = priorityRepository;
    }

    public async Task<IActionResult> Add()
    {
        var res = await priorityRepository.GetAll;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add([Bind("Level")] Priority priority)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("",
                    "Данные не прошли валидацию.");
            return View();
        }
        await priorityRepository.AddPriority(priority);
        return RedirectToAction(nameof(ShowAll));
    }
    public async Task<IActionResult> Edit(int id)
    {
        var res = await priorityRepository.GetFromId(id);
        ViewData["Id"] = id;
        return View(res);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id, Level")] Priority priority)
    {
        if (id != priority.Id)
        {
            ModelState.AddModelError("",
                    "ID не совпадает.");
            return View(priority);
        }
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("",
                    "Данные не прошли валидацию.");
            return View();
        }
        await priorityRepository.EditPriority(priority);
        return RedirectToAction(nameof(ShowAll));
    }
    public async Task<IActionResult> Remove(int id)
    {
        var res = await priorityRepository.GetAll;
        ViewData["PriorityId"] = new SelectList(res, "Id, Level");
        return View();
    }

    [HttpPost, ActionName("Remove")]
    public async Task<IActionResult> RemoveConfirmed(int id)
    {
        await priorityRepository.RemovePriority(id);
        return RedirectToAction(nameof(ShowAll));
    }

    public async Task<IActionResult> Show(int id)
    {
        var res = await priorityRepository.GetFromId(id);
        ViewData["Id"] = id;
        return View(res);
    }
    public async Task<IActionResult> ShowAll()
    {
        var res = await priorityRepository.GetAll;
        return View(res);
    }
}