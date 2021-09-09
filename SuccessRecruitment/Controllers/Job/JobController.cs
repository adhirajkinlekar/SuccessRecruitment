using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SuccessRecruitment.Services;
using SuccessRecruitment.DataTransferObjects.JobDataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using SuccessRecruitment.Shared.Pages;

namespace SuccessRecruitment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : RecuritmentControllerBase
    {
        private readonly IJobService _repo;

        public JobController(IJobService jobService)
        {
            _repo = jobService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Recuiters")]
        public async Task<IActionResult> GetRecruiters()
        {
            try
            {
                if (!HasPageAcces(Pages.ViewJobsPage))
                {
                    return Unauthorized();
                }

                return Ok(await _repo.GetRecuiters());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("AllJobs")]
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
        [HttpGet]
        [Authorize(Roles = "Admin,Recruiter")]
        [Route("Job/{jobId}")]
        public async Task<IActionResult> GetJobById(int jobId)
        {
            try
            {
                return Ok(await _repo.GetJobById(jobId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles  = "Admin,Recruiter")]
        [Route("GetJobsByUser")]
        public async Task<IActionResult> GetJobsByUser()
        {
            try
            {
                if (!HasPageAcces(Pages.ViewJobsPage))
                {
                    return Unauthorized();
                }

                return Ok(await _repo.GetJobsByUser());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,Recruiter")]
        [HttpPost]
        [Route("PublishJob")]
        public async Task<IActionResult> PublishJob(PublishJob newJob)
        {
            try
            {
                var a = ModelState;
                if (!HasPageAcces(Pages.AddJobPage))
                {
                    return Unauthorized();
                }

                return Ok(await _repo.PublishJob(newJob));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,Recruiter")]
        [HttpPut]
        [Route("UpdateJob")]
        public async Task<IActionResult> UpdateJob(UpdateJob updatedJob)
        {
            try
            {
                if (!HasPageAcces(Pages.AddJobPage))
                {
                    return Unauthorized();
                }

                return Ok(await _repo.UpdateJob(updatedJob));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
