using EAD_APP.BusinessLogic.Interfaces;
using EAD_APP.Core.Enums;
using EAD_APP.Core.Models;
using EAD_APP.Core.Requests;
using EAD_APP.Core.Response;
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
            try
            {
                await _userService.CreateUser(request);
                return CreatedAtAction(nameof(GetUserById), new { id = request.Id }, request);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
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
            var newUser = await _userService.GetUserById(user.Id);
            return Ok(newUser);
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
            var res = new ApiResponse() { IsSuccess = "true", Msg = "Success" };
            return Ok(res);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var user = await _userService.LoginUser(model);
                if (user == null)
                {
                    return Unauthorized();
                }
                return Ok(user);
                
            }
            catch (Exception e)
            {
                var res = new ApiResponse() { IsSuccess = "false", Msg = e.Message };
                return BadRequest(res);
            }
            
        }
        
        
        [HttpPut]
        [Route("{userId}/{status}")]
        public async Task<IActionResult> UpdateStatus(string userId, ActiveStatus status)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.UpdateStatus(user, status);
            var res = new ApiResponse() { IsSuccess = "true", Msg = "Success"};
            return Ok(res);
        }
    }
}
