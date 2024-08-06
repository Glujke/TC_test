using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TC.Repository.Abstract;
using TC.Repository.Entity;

namespace TC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public ApiUserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        // GET api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await userRepository.GetAll;
            if (users == null) return NotFound(new { message = $"Ничего не найдено." });

            return Ok(users);
        }

        // GET api/User/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await userRepository.GetUser(id);

            if (user == null) return NotFound(new { message = $"Пользователь с ID {id} не найден в базе данных." });

            return Ok(user);
        }

        // POST api/User
        [HttpPost]
        public async Task<ActionResult<User>> Add(User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await userRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingUser = await userRepository.GetUser(id);
            if (existingUser == null) return NotFound(new { message = $"Пользователь с ID {id} не найден в базе данных." });

            existingUser.Name = user.Name;
            await userRepository.EditUser(existingUser);


            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // DELETE api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userRepository.GetUser(id);
            if (user == null) return NotFound(new { message = $"Пользователь с ID {id} не найден в базе данных." });

            await userRepository.RemoveUser(id);

            return Ok(new { message = $"Пользователь с ID {id} успешно удалён." });
        }
    }
}