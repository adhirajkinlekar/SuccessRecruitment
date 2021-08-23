using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SuccessRecruitment.Services;
using SuccessRecruitment.DataTransferObjects.JobDataTransferObjects;
namespace SuccessRecruitment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        JobService _repo = null;
        public JobController()
        {
            _repo = new JobService();
        }

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
