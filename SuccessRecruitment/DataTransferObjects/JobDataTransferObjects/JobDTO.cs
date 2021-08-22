using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuccessRecruitment.DataTransferObjects.JobDataTransferObjects
{
    public class PublishJob
    {
            public string JobTitle { get; set; }
            public string Field { get; set; }
            public string JobLocation { get; set; }
            public string JobDescription { get; set; }
            public Guid PostedBy { get; set; }
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
        public Guid PostedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
