using APIIngame.Models;
using APIIngame.Services;
using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIIngame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ForgotPasswordAsync(email);
                if (result.IsSuccess)
                    return Ok(result); //Code 200

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");

        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ChangePasswordAsync(model);
                if (result.IsSuccess)
                    return Ok(result); //Code 200

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");

        }
    }
}
