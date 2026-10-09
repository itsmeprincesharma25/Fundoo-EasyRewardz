using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.Model;
using System.Security.Claims;

namespace Fundoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FundooController : ControllerBase
    {
        private readonly IUserBL _userBL;

        public FundooController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        [HttpPost("register")]
        public IActionResult RegisterUserModel(
            RegistrationModel registrationModel)
        {
            ResponseModel<RegistrationModel> response =
                _userBL.RegisterUserBL(registrationModel);

            return Ok(response);
        }

        [HttpPost("login")]
        public IActionResult LoginUserModel(
            LoginModel loginModel)
        {
            ResponseModel<string> response =
                _userBL.LoginUserBL(loginModel);

            if (!response.Success)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordModel model)
        {
            ResponseModel<string> response =
                await _userBL.ForgotPasswordBL(model);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(
            ResetPasswordModel model)
        {
            ResponseModel<string> response =
                _userBL.ResetPasswordBL(model);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var email =
                User.FindFirstValue(ClaimTypes.Email);

            var name =
                User.FindFirstValue(ClaimTypes.Name);

            return Ok(new
            {
                UserId = userId,
                Email = email,
                Name = name
            });
        }
    }
}