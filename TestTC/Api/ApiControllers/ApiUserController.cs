using Microsoft.AspNetCore.Mvc;
using TC.Repository.Abstract;
using TC.Repository.Entity;

namespace TestTC.Api.ApiControllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUserController : ControllerBase {
        private readonly IUserRepository userRepository;
        private readonly ILogger<ApiUserController> logger;

        public ApiUserController(IUserRepository userRepository, ILogger<ApiUserController> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            try
            {
                var users = await userRepository.GetAll;
                if (users == null)
                    return NotFound(new { message = $"Ничего не найдено." });

                return Ok(users);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var user = await userRepository.GetUser(id);

                if (user == null)
                    return NotFound(new { message = $"Пользователь с ID {id} не найден в базе данных." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Add(User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                await userRepository.AddUser(user);
                return CreatedAtAction(nameof(GetUser),
                new { id = user.Id },
                user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var existingUser = await userRepository.GetUser(id);
                if (existingUser == null)
                    return NotFound(new { message = $"Пользователь с ID {id} не найден в базе данных." });

                existingUser.Name = user.Name;
                await userRepository.EditUser(existingUser);

                return CreatedAtAction(nameof(GetUser),
                new { id = user.Id },
                user);
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
            try
            {
                var user = await userRepository.GetUser(id);
                if (user == null)
                    return NotFound(new { message = $"Пользователь с ID {id} не найден в базе данных." });

                await userRepository.RemoveUser(id);

                return Ok(new { message = $"Пользователь с ID {id} успешно удалён." });
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
