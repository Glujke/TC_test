﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TC.Models;
using TC.Repository.Abstract;
using TC.Repository.Entity;

namespace TestTC.Api.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiPriorityController : ControllerBase
    {
        private readonly IPriorityRepository priorityRepository;

        public ApiPriorityController(IPriorityRepository priorityRepository)
        {
            this.priorityRepository = priorityRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Priority>>> GetAll()
        {
            var priorities = await priorityRepository.GetAll;
            if (priorities == null) return NotFound(new { message = $"Ничего не найдено." });
            return Ok(priorities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Priority>> Get(int id)
        {
            var priority = await priorityRepository.GetFromId(id);

            if (priority == null)
            {
                return NotFound(new { message = $"Приоритет с ID {id} не найден в базе данных." });
            }

            return Ok(priority);
        }

        [HttpPost]
        public async Task<ActionResult<Priority>> Create(Priority priority)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await priorityRepository.AddPriority(priority);
            return CreatedAtAction(nameof(Get), new { id = priority.Id }, priority);
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

            await priorityRepository.EditPriority(priority);
            return CreatedAtAction(nameof(Get), new { id = priority.Id }, priority);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var priority = await priorityRepository.GetFromId(id);

            if (priority == null)
            {
                return NotFound(new { message = $"Приоритет с ID {id} не найден в базе данных." });
            }

            await priorityRepository.RemovePriority(id);
            return Ok(new { message = $"Приоритет с ID {id} успешно удалён." });
        }
    }
}