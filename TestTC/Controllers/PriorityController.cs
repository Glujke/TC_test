using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
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
        try
        {
            var res = await priorityRepository.GetAll;
            return View();
        }catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
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
        try
        {
            await priorityRepository.AddPriority(priority);
            return RedirectToAction(nameof(ShowAll));
        }catch(Exception ex)
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
            var res = await priorityRepository.GetFromId(id);
            ViewData["Id"] = id;
            return View(res);
        }catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
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
        try
        {
            await priorityRepository.EditPriority(priority);
            return RedirectToAction(nameof(ShowAll));
        }catch(Exception ex)
        {
            Log.Error(ex.Message);
            Log.Error(ex.InnerException?.Message);
            Log.Error(ex.StackTrace);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return View("Error", new ErrorViewModel { RequestId = errorMessage });
        }
    }
    public async Task<IActionResult> Remove(int id)
    {
        try
        {
            var res = await priorityRepository.GetAll;
            ViewData["PriorityId"] = new SelectList(res, "Id, Level");
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

    [HttpPost, ActionName("Remove")]
    public async Task<IActionResult> RemoveConfirmed(int id)
    {
        try
        {
            await priorityRepository.RemovePriority(id);
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
            var res = await priorityRepository.GetFromId(id);
            ViewData["Id"] = id;
            return View(res);
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
            var res = await priorityRepository.GetAll;
            return View(res);
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