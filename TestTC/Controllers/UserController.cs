﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TC.Models;
using TC.Repository.Abstract;
using TC.Repository.Entity;

namespace TC.Controllers;

public class UserController : Controller
{
	private readonly IUserRepository userRepository;

	public UserController(IUserRepository userRepository)
	{
		this.userRepository = userRepository;
	}

	public async Task<IActionResult> Add()
	{
		var res = await userRepository.GetAll;
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Add([Bind("Name")] User user)
	{
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("",
                    "Данные не прошли валидацию.");
            return View();
        }

        await userRepository.AddUser(user);
        return RedirectToAction(nameof(ShowAll));
    }
	public async Task<IActionResult> Edit(int id)
	{
		var res = await userRepository.GetUser(id);
		ViewData["Id"] = id;
		return View(res);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(int id, [Bind("Id, Name")] User user)
    {
        if (id != user.Id)
        {
            ModelState.AddModelError("",
                    "ID не совпадает.");
            return View(user);
        }
        if (ModelState.IsValid)
        {
            ModelState.AddModelError("",
                    "Данные не прошли валидацию.");
            return View();
        }

        await userRepository.EditUser(user);
        return RedirectToAction(nameof(ShowAll));
    }
    public async Task<IActionResult> Remove(int id)
    {
        var res = await userRepository.GetUser(id);
        return View(res);
    }



    [HttpPost, ActionName("Remove")]
    public async Task<IActionResult> RemoveConfirmed(int id)
    {
        await userRepository.RemoveUser(id);
        return RedirectToAction(nameof(ShowAll));
    }
    public async Task<IActionResult> Show(int id)
    {
        var res = await userRepository.GetUser(id);
        ViewData["Id"] = id;
        return View(res);
    }
    public async Task<IActionResult> ShowAll()
    {
        var res = await userRepository.GetAll;
        return View(res);
    }
}