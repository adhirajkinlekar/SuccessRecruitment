using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SuccessRecruitment.DataTransferObjects.JobDataTransferObjects;
using SuccessRecruitment.Models;
namespace SuccessRecruitment.Services
{

    public interface IJobService
    {
        Task<List<Job>> GetAllJobs();
        Task<Job> GetJobById(int jobId);
        Task<bool> PublishJob(PublishJob newJob);
        Task<bool> UpdateJob(UpdateJob updatedJob);
        Task<List<TblJob>> GetJobsByUser();
        Task<List<Recruiter>> GetRecuiters();
    }

    public class JobService : IJobService
    {
        private readonly RecruitmentDB _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JobService(RecruitmentDB database , IHttpContextAccessor IHttpContextAccessor)
        {
            _db = database;
            _httpContextAccessor = IHttpContextAccessor;
        }

        private Guid GetUserId() => Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<List<Job>> GetAllJobs()
        {
            try
            {
                var jobs = await _db.TblJobs.Include(x=> x.Employer).Where(x => !x.IsArchived).Select(x=> new
                Job
                {
                    JobId = x.JobId,
                    JobTitle = x.JobTitle,
                    Field = x.Field,
                    RecruiterName = x.Employer.UserName,
                    JobDescription = x.JobDescription,
                    JobLocation = x.JobLocation,
                    RecruiterId = x.Employer.UserId
                }
                ).ToListAsync();

                return jobs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Job> GetJobById(int jobId)
        {
            try
            {
                var job = await _db.TblJobs.Include(x => x.Employer).Where(x => x.JobId == jobId).Select(x => new
                 Job
                {
                    JobId = x.JobId,
                    JobTitle = x.JobTitle,
                    Field = x.Field,
                    RecruiterName = x.Employer.UserName,
                    RecruiterId = x.Employer.UserId,
                    JobDescription = x.JobDescription,
                    JobLocation = x.JobLocation
                }
                ).FirstOrDefaultAsync();

                if (job == null)
                {
                    throw new Exception("Job not found");
                }

                return job;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        

        public async Task<List<TblJob>> GetJobsByUser()
        {
            try
            {
                var jobsPostedByUser = await _db.TblJobs.Where(x => x.EmployerId == GetUserId() && !x.IsArchived).ToListAsync();
                return jobsPostedByUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

      

        public async Task<bool> PublishJob(PublishJob newJob)
        {
            try
            {
                bool isCreated;
                var jobExists = await _db.TblJobs.AnyAsync(x => x.JobLocation == newJob.JobLocation && x.JobTitle == newJob.JobTitle && x.EmployerId == newJob.EmployerId && !x.IsArchived);

                if (jobExists)
                {
                    throw new Exception("This job has already been posted");
                }
                else
                {
                    _db.TblJobs.Add(new TblJob
                    {
                        JobTitle = newJob.JobTitle,
                        JobDescription = newJob.JobDescription,
                        Field = newJob.Field,
                        JobLocation = newJob.JobLocation,
                        CreatedBy = GetUserId(),
                        EmployerId = newJob.EmployerId,
                        CreatedDate = DateTime.Now
                    });

                    await _db.SaveChangesAsync();
                    isCreated = true;
                }
                return isCreated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> UpdateJob(UpdateJob updatedJob)
        {
            try
            {
                bool isUpdated;
                var job = await _db.TblJobs.Where(x => x.JobId == updatedJob.JobId && !x.IsArchived).FirstOrDefaultAsync();

                if (job == null)
                {
                    throw new Exception("Error occured");
                }
                else
                { 
                    job.JobTitle = updatedJob.JobTitle;
                    job.JobDescription = updatedJob.JobDescription;
                    job.Field = updatedJob.Field;
                    job.JobLocation = updatedJob.JobLocation;
                    job.EmployerId = updatedJob.EmployerId;
                    job.ModifiedBy = GetUserId();
                    job.ModifiedDate = DateTime.Now;

                    await _db.SaveChangesAsync();
                    isUpdated = true;
                }
                return isUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Recruiter>> GetRecuiters()
        {
            try
            {
                List<Recruiter> recruiters = await _db.TblUserRoles.Where(x => !x.IsArchived && !x.TblRole.IsArchived && x.TblRole.RoleName == "Recruiter" && !x.User.IsArchived).Select(x => new Recruiter
                {
                    RecruiterId = x.User.UserId,
                    RecruiterName = x.User.UserName
                }).ToListAsync();

                return recruiters;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
        }
    }
}