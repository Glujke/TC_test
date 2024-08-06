using Microsoft.AspNetCore.Mvc;
using Serilog;
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

        public ApiToDoItemController(IToDoItemRepository toDoItemRepository, IUserRepository userRepository,
            IPriorityRepository priorityRepository)
        {
            this.toDoItemRepository = toDoItemRepository;
            this.userRepository = userRepository;
            this.priorityRepository = priorityRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetAll()
        {
            try
            {
                var toDoItems = await _toDoItemRepository.GetAll;
                if (toDoItems == null) return NotFound();
                return Ok(toDoItems);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.InnerException?.Message);
                Log.Error(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new
                {
                    message = errorMessage
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> Get(int id)
        {
            try
            {
                var toDoItem = await _toDoItemRepository.GetToDoItem(id);

                if (toDoItem == null)
                {
                    return NotFound(new
                    {
                        message = $"Задача с ID {id} не найдена в базе данных."
                    });
                }

                return Ok(toDoItem);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.InnerException?.Message);
                Log.Error(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new
                {
                    message = errorMessage
                });
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
                return BadRequest(new
                {
                    message = $"Дата выполнения не может быть раньше текущей."
                });
            }
            try
            {
                await _toDoItemRepository.AddToDoItem(toDoItem);
                return CreatedAtAction(nameof(Get),
                    new
                    {
                        id = toDoItem.Id
                    },
                    toDoItem);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.InnerException?.Message);
                Log.Error(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new
                {
                    message = errorMessage
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] ToDoItem toDoItem)
        {
            try
            {
                var finishToDoItem = await _toDoItemRepository.GetToDoItem(id);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (toDoItem.DueDate < DateTime.Now)
                {
                    return BadRequest(new
                    {
                        message = $"Дата выполнения не может быть раньше текущей."
                    });
                }
                finishToDoItem.CopyFrom(toDoItem);

                await _toDoItemRepository.EditToDoItem(finishToDoItem);
                return Ok(finishToDoItem);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.InnerException?.Message);
                Log.Error(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new
                {
                    message = errorMessage
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var toDoItem = await _toDoItemRepository.GetToDoItem(id);

                if (toDoItem == null)
                {
                    return NotFound(new
                    {
                        message = $"Задача с ID {id} не найдена в базе данных."
                    });
                }

                await _toDoItemRepository.RemoveToDoItem(toDoItem.Id);
                return Ok(new
                {
                    message = $"Задача с ID {id} успешно удалёна."
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.InnerException?.Message);
                Log.Error(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new
                {
                    message = errorMessage
                });
            }
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                var toDoItem = await _toDoItemRepository.GetToDoItem(id);

                if (toDoItem == null)
                {
                    return NotFound(new
                    {
                        message = $"Задача с ID {id} не найдена в базе данных."
                    });
                }
                toDoItem.IsCompleted = true;
                await _toDoItemRepository.EditToDoItem(toDoItem);
                return Ok(toDoItem);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.InnerException?.Message);
                Log.Error(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new
                {
                    message = errorMessage
                });
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
                var toDoItem = await _toDoItemRepository.GetToDoItem(id);

                if (toDoItem == null)
                {
                    return NotFound(new
                    {
                        message = $"Задача с ID {id} не найдена в базе данных."
                    });
                }

                if (request.UserId <= 0)
                {
                    return BadRequest(new
                    {
                        message = "ID пользователя должен быть положительным числом."
                    });
                }

                var user = await _userRepository.GetUser(request.UserId);

                if (user == null)
                {
                    return NotFound(new
                    {
                        message = $"Пользователь с ID {request.UserId} не найден в базе данных."
                    });
                }

                toDoItem.UserId = user.Id;
                await _toDoItemRepository.EditToDoItem(toDoItem);

                return Ok(toDoItem);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.InnerException?.Message);
                Log.Error(ex.StackTrace);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new
                {
                    message = errorMessage
                });
            }
        }
    }
}
