using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }

    public class JobService : IJobService
    {
        SuccessRecruitmentDB _db = null;

        public JobService()
        {
            _db = new SuccessRecruitmentDB();
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

        public async Task<bool> PublishJob(PublishJob newJob)
        {
            try
            {
                bool isCreated;
                var jobExists = await _db.TblJobs.AnyAsync(x => x.JobLocation == newJob.JobLocation && x.JobTitle == newJob.JobTitle && x.PostedBy == newJob.PostedBy && !x.IsArchived);

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
                        CreatedBy = Guid.Parse("6B951FDB-31BE-4AE1-8B96-28A3075D7060"),
                        PostedBy = Guid.Parse("6B951FDB-31BE-4AE1-8B96-28A3075D7060"),
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
                    job.PostedBy = Guid.Parse("6B951FDB-31BE-4AE1-8B96-28A3075D7060");
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
    }
}