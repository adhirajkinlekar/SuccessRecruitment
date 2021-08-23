using Microsoft.AspNetCore.Mvc;
using SuccessRecruitment.DataTransferObjects.Auth;
using SuccessRecruitment.Services.Auth;
using System;
using System.Threading.Tasks;

namespace SuccessRecruitment.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        AuthService _repo = null;

       public AuthController()
        {
            _repo = new AuthService();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserRegisterDTO newUser)
        {
            try
            {
               return Ok(await _repo.Register(newUser));
            }
            catch(Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDTO user)
        {
            try
            {
                return Ok(await _repo.Login(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
