using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuccessRecruitment.Models;
using Microsoft.EntityFrameworkCore;

namespace SuccessRecruitment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private SuccessRecruitmentDB _db = null;
        public JobController()
        {
            _db = new SuccessRecruitmentDB();
        }

        [HttpGet]
        [Route("AllJobs")]
        public ActionResult Get()
        {

            return Ok();
        }
    }
}
