using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Models;
using EAD_APP.Core.Requests;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EAD_APP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var people = await _userService.GetAllUsers();
            return Ok(people);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User request)
        {
            await _userService.CreateUser(request);
            return CreatedAtAction(nameof(GetUserById), new { id = request.Id }, request);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(User request)
        {
            var user = await _userService.GetUserById(request.Id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.UpdateUser(request);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUser(id);
            return NoContent();
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var res = await _userService.LoginUser(model);
            if (!res)
            {
                return Unauthorized();
            }
            return Ok(res);
        }
    }
}
