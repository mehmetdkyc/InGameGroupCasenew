using APIIngame.Models;
using APIIngame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIIngame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        //api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(registerModel);
                if (result.IsSuccess)
                    return Ok(result); //Code 200

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");


        }
        //api/auth/register
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(loginModel);
                if (result.IsSuccess)
                    return Ok(result); //Code 200

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");

        }
    }
}
