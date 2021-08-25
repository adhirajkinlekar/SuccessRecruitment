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
        Task<List<TblJob>> GetAllJobs();
        Task<bool> PublishJob(PublishJob newJob);
        Task<bool> UpdateJob(UpdateJob updatedJob);
        Task<List<TblJob>> GetJobsByUser();
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

        public async Task<List<TblJob>> GetAllJobs()
        {
            try
            {
                var activeJobs = await _db.TblJobs.Where(x => !x.IsArchived).ToListAsync();
                return activeJobs;
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
                var jobExists = await _db.TblJobs.AnyAsync(x => x.JobLocation == newJob.JobLocation && x.JobTitle == newJob.JobTitle && x.EmployerId == newJob.PostedBy && !x.IsArchived);

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
                        CreatedBy = Guid.Parse("3AF1BA96-4A39-467C-8AD9-3F418F199CD0"),
                        EmployerId = Guid.Parse("3AF1BA96-4A39-467C-8AD9-3F418F199CD0"),
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
                    throw new Exception("Selected job doesn't exist. Please contact support team");
                }
                else
                { 
                    job.JobTitle = updatedJob.JobTitle;
                    job.JobDescription = updatedJob.JobDescription;
                    job.Field = updatedJob.Field;
                    job.JobLocation = updatedJob.JobLocation;
                    job.EmployerId = Guid.Parse("6B951FDB-31BE-4AE1-8B96-28A3075D7060");
                    job.ModifiedBy = Guid.Parse("6B951FDB-31BE-4AE1-8B96-28A3075D7060");
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

        private Guid GetUserId() => Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}