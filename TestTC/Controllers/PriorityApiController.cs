using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TC.Models;
using TC.Repository.Abstract;
using TC.Repository.Entity;

namespace TC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityApiController : ControllerBase
    {
        private readonly IPriorityRepository priorityRepository;

        public PriorityApiController(IPriorityRepository priorityRepository)
        {
            this.priorityRepository = priorityRepository;
        }

        // GET: api/Priority
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Priority>>> GetAll()
        {
            var priorities = await priorityRepository.GetAll;
            return Ok(priorities);
        }

        // GET: api/Priority/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Priority>> Get(int id)
        {
            var priority = await priorityRepository.GetFromId(id);

            if (priority == null)
            {
                return NotFound();
            }

            return Ok(priority);
        }

        // POST: api/Priority
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

        // PUT: api/Priority/{id}
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
            return NoContent(); 
        }

        // DELETE: api/Priority/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var priority = await priorityRepository.GetFromId(id);

            if (priority == null)
            {
                return NotFound(new { message = $"Приоритет с ID {id} не найден в базе данных." });
            }

            await priorityRepository.RemovePriority(id);
            return NoContent();
        }
    }
}