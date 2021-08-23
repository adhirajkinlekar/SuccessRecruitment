using System;
using System.Collections.Generic;

#nullable disable

namespace SuccessRecruitment.Models
{
    public partial class TblJob
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string Field { get; set; }
        public string JobLocation { get; set; }
        public string JobDescription { get; set; }
        public Guid PostedBy { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsArchived { get; set; }

        public virtual Tbluser PostedByNavigation { get; set; }
    }
}
