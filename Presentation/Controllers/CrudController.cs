using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Logic.Interfaces;

namespace MyApp.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CrudController : ControllerBase
    {
        private readonly IUserLogic _logic;

        public CrudController(IUserLogic logic)
        {
            _logic = logic;
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _logic.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("users")]
        public IActionResult CreateUser([FromQuery] string username, [FromQuery] string password)
        {
            var user = _logic.CreateUser(username, password);
            if (user == null) return BadRequest("Could not create user.");
            return Ok(user);
        }

        [HttpPut("users/{id}")]
        public IActionResult UpdateUser(int id, [FromQuery] string username, [FromQuery] string password)
        {
            var success = _logic.UpdateUser(id, username, password);
            if (!success) return NotFound("Update failed.");
            return Ok("User updated");
        }

        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var success = _logic.DeleteUser(id);
            if (!success) return NotFound("Delete failed.");
            return Ok("User deleted");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromQuery] string username, [FromQuery] string password)
        {
            var token = _logic.Login(username, password);
            if (token == null) return Unauthorized("Invalid credentials");
            return Ok(new { Token = token });
        }
    }
}
