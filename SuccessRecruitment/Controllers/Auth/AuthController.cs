using Microsoft.AspNetCore.Mvc;
using SuccessRecruitment.DataTransferObjects.Auth;
using SuccessRecruitment.Services.Auth;
using System;
using System.Threading.Tasks;

namespace SuccessRecruitment.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : RecuritmentControllerBase
    {
        private readonly IAuthService _repo;

        public AuthController(IAuthService authService)
        {
            _repo = authService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserRegisterDTO newUser)
        {
            try
            {
               newUser.Email = newUser.Email?.Trim();
               newUser.UserName = newUser.UserName?.Trim();
               newUser.Password = newUser.Password?.Trim();
               return Ok(await _repo.Register(newUser));

            }
            catch(Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDTO user)
        {
            try
            {
                user.Email = user.Email.Trim();
                return Ok(await _repo.Login(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("IsAuthenticated")]
        public IActionResult  CheckUserAuthentication()
        {
            try
            {
                return Ok(User.Identity.IsAuthenticated);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
