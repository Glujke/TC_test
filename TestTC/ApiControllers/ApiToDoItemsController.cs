using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TC.Repository.Abstract;
using TC.Repository.Entity;
using TC.Repository.Implementation;

namespace TestTC.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiToDoItemController : ControllerBase
    {
        private readonly IToDoItemRepository _toDoItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPriorityRepository _priorityRepository;

        public ApiToDoItemController(IToDoItemRepository toDoItemRepository, IUserRepository userRepository,
            IPriorityRepository priorityRepository)
        {
            _toDoItemRepository = toDoItemRepository;
            _userRepository = userRepository;
            _priorityRepository = priorityRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetAll()
        {
            var toDoItems = await _toDoItemRepository.GetAll;
            return Ok(toDoItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> Get(int id)
        {
            var toDoItem = await _toDoItemRepository.GetToDoItem(id);

            if (toDoItem == null)
            {
                return NotFound();
            }

            return Ok(toDoItem);
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItem>> Add([FromBody] ToDoItem toDoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _toDoItemRepository.AddToDoItem(toDoItem);
            return CreatedAtAction(nameof(Get), new { id = toDoItem.Id }, toDoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] ToDoItem toDoItem)
        {
            var finishToDoItem = await _toDoItemRepository.GetToDoItem(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            finishToDoItem.Priority = toDoItem.Priority;
            finishToDoItem.IsCompleted = toDoItem.IsCompleted;
            finishToDoItem.PriorityId = toDoItem.PriorityId;
            finishToDoItem.Description = toDoItem.Description;
            finishToDoItem.Title = toDoItem.Title;
            finishToDoItem.User = toDoItem.User;
            finishToDoItem.UserId = toDoItem.UserId;
            finishToDoItem.DueDate = toDoItem.DueDate;

            await _toDoItemRepository.EditToDoItem(finishToDoItem);
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var toDoItem = await _toDoItemRepository.GetToDoItem(id);

            if (toDoItem == null)
            {
                return NotFound();
            }

            await _toDoItemRepository.RemoveToDoItem(toDoItem.Id);
            return NoContent();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Complete(int id)
        {
            var toDoItem = await _toDoItemRepository.GetToDoItem(id);

            if (toDoItem == null)
            {
                return NotFound();
            }
            toDoItem.IsCompleted = true;
            await _toDoItemRepository.EditToDoItem(toDoItem);
            return NoContent();
        }
    }
}