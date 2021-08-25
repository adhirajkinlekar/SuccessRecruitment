using System;
using System.Collections.Generic;

#nullable disable

namespace SuccessRecruitment.Models
{
    public partial class TblRolePage
    {
        public int RolePageId { get; set; }
        public int RoleId { get; set; }
        public int PageId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsArchived { get; set; }

        public virtual TblPage TblPage { get; set; }
        public virtual TblRole TblRole { get; set; }
    }
}
