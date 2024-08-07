﻿using Microsoft.AspNetCore.Mvc;
using TC.Models;
using TC.Repository.Abstract;
using TC.Repository.Entity;

namespace TC.Controllers {
    public class UserController : Controller {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UserController> logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<IActionResult> Add()
        {
            try
            {
                var res = await userRepository.GetAll;
                return View();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return View("Error", new ErrorViewModel { RequestId = errorMessage });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([Bind("Name")] User user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Данные не прошли валидацию.");
                return View();
            }
            try
            {
                await userRepository.AddUser(user);
                return RedirectToAction(nameof(ShowAll));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return View("Error", new ErrorViewModel { RequestId = errorMessage });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var res = await userRepository.GetUser(id);
                ViewData["Id"] = id;
                return View(res);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return View("Error", new ErrorViewModel { RequestId = errorMessage });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name")] User user)
        {
            if (id != user.Id)
            {
                ModelState.AddModelError("", "ID не совпадает.");
                return View(user);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Данные не прошли валидацию.");
                return View();
            }
            try
            {
                await userRepository.EditUser(user);
                return RedirectToAction(nameof(ShowAll));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return View("Error", new ErrorViewModel { RequestId = errorMessage });
            }
        }

        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var res = await userRepository.GetUser(id);
                return View(res);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return View("Error", new ErrorViewModel { RequestId = errorMessage });
            }
        }

        [HttpPost, ActionName("Remove")]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            try
            {
                await userRepository.RemoveUser(id);
                return RedirectToAction(nameof(ShowAll));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return View("Error", new ErrorViewModel { RequestId = errorMessage });
            }
        }

        public async Task<IActionResult> Show(int id)
        {
            try
            {
                var res = await userRepository.GetUser(id);
                ViewData["Id"] = id;
                return View(res);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return View("Error", new ErrorViewModel { RequestId = errorMessage });
            }
        }

        public async Task<IActionResult> ShowAll()
        {
            try
            {
                var res = await userRepository.GetAll;
                return View(res);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return View("Error", new ErrorViewModel { RequestId = errorMessage });
            }
        }
    }
}
