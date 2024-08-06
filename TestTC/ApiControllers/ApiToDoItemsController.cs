using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            if (toDoItems == null) return NotFound(new { message = $"Ничего не найдено." });
            return Ok(toDoItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> Get(int id)
        {
            var toDoItem = await _toDoItemRepository.GetToDoItem(id);

            if (toDoItem == null)
            {
                return NotFound(new { message = $"Задача с ID {id} не найдена в базе данных."});
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
            if (toDoItem.DueDate < DateTime.Now)
            {
                return BadRequest(new { message = $"Дата выполнения не может быть раньше текущей." });
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
            if (toDoItem.DueDate < DateTime.Now)
            {
                return BadRequest(new { message = $"Дата выполнения не может быть раньше текущей." });
            }
            finishToDoItem.CopyFrom(toDoItem);

            await _toDoItemRepository.EditToDoItem(finishToDoItem);
            return Ok(new { message = $"Задача с ID {id} успешно обновлена." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var toDoItem = await _toDoItemRepository.GetToDoItem(id);

            if (toDoItem == null)
            {
                return NotFound(new { message = $"Задача с ID {id} не найдена в базе данных." });
            }

            await _toDoItemRepository.RemoveToDoItem(toDoItem.Id);
            return Ok(new { message = $"Задача с ID {id} успешно удалёна." });
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var toDoItem = await _toDoItemRepository.GetToDoItem(id);

            if (toDoItem == null)
            {
                return NotFound(new { message = $"Задача с ID {id} не найдена в базе данных." });
            }
            toDoItem.IsCompleted = true;
            await _toDoItemRepository.EditToDoItem(toDoItem);
            return Ok(new { message = $"Задача с ID {id} успешно выполнена." });
        }

        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignUser(int id, [FromBody] UserAssignmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var toDoItem = await _toDoItemRepository.GetToDoItem(id);

            if (toDoItem == null)
            {
                return NotFound(new { message = $"Задача с ID {id} не найдена в базе данных." });
            }

            if (request.UserId <= 0)
            {
                return BadRequest(new { message = "ID пользователя должен быть положительным числом." });
            }

            var user = await _userRepository.GetUser(request.UserId);

            if (user == null)
            {
                return NotFound(new { message = $"Пользователь с ID {request.UserId} не найден в базе данных." });
            }

            toDoItem.UserId = user.Id; 
            await _toDoItemRepository.EditToDoItem(toDoItem); 

            return Ok(new { message = $"Задача с ID {id} успешно назначена пользователю с ID {request.UserId}." });
        }
    }

    public class UserAssignmentRequest
    {
        public int UserId { get; set; }
    }
}