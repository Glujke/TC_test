using Microsoft.AspNetCore.Mvc;
using TC.Controllers;
using TC.Repository.Abstract;
using TC.Repository.Entity;
using TestTC.Api.Requests;

namespace TestTC.Api.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiToDoItemController : ControllerBase
    {
        private readonly IToDoItemRepository toDoItemRepository;
        private readonly IUserRepository userRepository;
        private readonly IPriorityRepository priorityRepository;
        private readonly ILogger<ApiToDoItemController> logger;

        public ApiToDoItemController(IToDoItemRepository toDoItemRepository, IUserRepository userRepository,
            IPriorityRepository priorityRepository, ILogger<ApiToDoItemController> logger)
        {
            this.toDoItemRepository = toDoItemRepository;
            this.userRepository = userRepository;
            this.priorityRepository = priorityRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetAll()
        {
            try
            {
                var toDoItems = await toDoItemRepository.GetAll;
                if (toDoItems == null) return NotFound();
                return Ok(toDoItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.InnerException?.Message);
                logger.LogError(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> Get(int id)
        {
            try
            {
                var toDoItem = await toDoItemRepository.GetToDoItem(id);

                if (toDoItem == null)
                {
                    return NotFound(new { message = $"Задача с ID {id} не найдена в базе данных." });
                }

                return Ok(toDoItem);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.InnerException?.Message);
                logger.LogError(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
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
            try
            {
                await toDoItemRepository.AddToDoItem(toDoItem);
                return CreatedAtAction(nameof(Get), new { id = toDoItem.Id }, toDoItem);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.InnerException?.Message);
                logger.LogError(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] ToDoItem toDoItem)
        {
            try
            {
                var finishToDoItem = await toDoItemRepository.GetToDoItem(id);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (toDoItem.DueDate < DateTime.Now)
                {
                    return BadRequest(new { message = $"Дата выполнения не может быть раньше текущей." });
                }
                finishToDoItem.CopyFrom(toDoItem);

                await toDoItemRepository.EditToDoItem(finishToDoItem);
                return Ok(finishToDoItem);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.InnerException?.Message);
                logger.LogError(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var toDoItem = await toDoItemRepository.GetToDoItem(id);

                if (toDoItem == null)
                {
                    return NotFound(new { message = $"Задача с ID {id} не найдена в базе данных." });
                }

                await toDoItemRepository.RemoveToDoItem(toDoItem.Id);
                return Ok(new { message = $"Задача с ID {id} успешно удалёна." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.InnerException?.Message);
                logger.LogError(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                var toDoItem = await toDoItemRepository.GetToDoItem(id);

                if (toDoItem == null)
                {
                    return NotFound(new { message = $"Задача с ID {id} не найдена в базе данных." });
                }
                toDoItem.IsCompleted = true;
                await toDoItemRepository.EditToDoItem(toDoItem);
                return Ok(toDoItem);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.InnerException?.Message);
                logger.LogError(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignUser(int id, [FromBody] UserAssignmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var toDoItem = await toDoItemRepository.GetToDoItem(id);

                if (toDoItem == null)
                {
                    return NotFound(new { message = $"Задача с ID {id} не найдена в базе данных." });
                }
                if (request.UserId <= 0)
                {
                    return BadRequest(new { message = "ID пользователя должен быть положительным числом." });
                }

                var user = await userRepository.GetUser(request.UserId);

                if (user == null)
                {
                    return NotFound(new { message = $"Пользователь с ID {request.UserId} не найден в базе данных." });
                }

                toDoItem.UserId = user.Id;
                await toDoItemRepository.EditToDoItem(toDoItem);

                return Ok(toDoItem);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.InnerException?.Message);
                logger.LogError(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }
    }
}