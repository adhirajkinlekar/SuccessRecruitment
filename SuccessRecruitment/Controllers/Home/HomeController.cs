using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuccessRecruitment.Services.Home;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [Authorize(Roles = "Admin")]
        [Route("")]
        public async Task<IActionResult> GetAppInformation()
        {
            try
            {
                await _repo.GetAppInformation();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
