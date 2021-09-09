using System;

namespace SuccessRecruitment.DataTransferObjects.JobDataTransferObjects
{
    public class PublishJob
    {
            public string JobTitle { get; set; }
            public string Field { get; set; }
            public string JobLocation { get; set; }
            public string JobDescription { get; set; }
            public Guid EmployerId { get; set; }
            public Guid? CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
    }

    public class UpdateJob
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string Field { get; set; }
        public string JobLocation { get; set; }
        public string JobDescription { get; set; }
        public Guid EmployerId { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class Job
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string Field { get; set; }
        public string JobLocation { get; set; }
        public string JobDescription { get; set; }
        public string RecruiterName { get; set; }
        public Guid RecruiterId { get; set; }
    }

    public class Recruiter
    {
        public Guid RecruiterId { get; set; }
        public string RecruiterName { get; set; }
    }
}
