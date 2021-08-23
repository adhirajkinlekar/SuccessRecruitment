using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SuccessRecruitment.Services;
using SuccessRecruitment.DataTransferObjects.JobDataTransferObjects;
using Microsoft.AspNetCore.Authorization;

namespace SuccessRecruitment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _repo;

        public JobController(IJobService jobService)
        {
            _repo = jobService;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllJobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            try
            {
                return Ok(await _repo.GetAllJobs());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("PublishJob")]
        public async Task<IActionResult> PublishJob(PublishJob newJob)
        {
            try
            {
                return Ok(await _repo.PublishJob(newJob));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateJob")]
        public async Task<IActionResult> UpdateJob(UpdateJob updatedJob)
        {
            try
            {
                return Ok(await _repo.UpdateJob(updatedJob));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
