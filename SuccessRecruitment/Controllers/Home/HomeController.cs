using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessRecruitment.Services.Home;
using System;
using System.Threading.Tasks;

namespace SuccessRecruitment.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : RecuritmentControllerBase
    {
        private readonly IHomeService _repo;

        public HomeController(IHomeService homeService)
        {
            _repo = homeService;
        }
        [HttpGet]
        [Authorize(Roles = "Administrator,Manager")]
        [Route("")]
        public async Task<IActionResult> GetAppInformation()
        {
            try
            {
             
                return Ok(await _repo.GetAppInformation());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
// check if returning noContent() allows to return an response without having to retrieve any data