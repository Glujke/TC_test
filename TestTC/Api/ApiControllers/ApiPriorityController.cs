﻿using Microsoft.AspNetCore.Mvc;
using TC.Controllers;
using TC.Repository.Abstract;
using TC.Repository.Entity;

namespace TestTC.Api.ApiControllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ApiPriorityController : ControllerBase {
        private readonly IPriorityRepository priorityRepository;
        private readonly ILogger<ApiPriorityController> logger;

        public ApiPriorityController(IPriorityRepository priorityRepository, ILogger<ApiPriorityController> logger)
        {
            this.priorityRepository = priorityRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Priority>>> GetAll()
        {
            try
            {
                var priorities = await priorityRepository.GetAll;
                if (priorities == null)
                    return NotFound(new { message = $"Ничего не найдено." });
                return Ok(priorities);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Priority>> Get(int id)
        {
            try
            {
                var priority = await priorityRepository.GetFromId(id);

                if (priority == null)
                {
                    return NotFound(new { message = $"Приоритет с ID {id} не найден в базе данных." });
                }
                return Ok(priority);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Priority>> Create(Priority priority)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await priorityRepository.AddPriority(priority);
                return CreatedAtAction(nameof(Get), new { id = priority.Id }, priority);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Priority priority)
        {
            if (id != priority.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await priorityRepository.EditPriority(priority);
                return CreatedAtAction(nameof(Get), new { id = priority.Id }, priority);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var priority = await priorityRepository.GetFromId(id);

            if (priority == null)
            {
                return NotFound(new { message = $"Приоритет с ID {id} не найден в базе данных." });
            }
            try
            {
                await priorityRepository.RemovePriority(id);
                return Ok(new { message = $"Приоритет с ID {id} успешно удалён." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }
    }
}
