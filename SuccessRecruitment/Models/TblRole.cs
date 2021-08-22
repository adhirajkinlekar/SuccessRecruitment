using System;
using System.Collections.Generic;

#nullable disable

namespace SuccessRecruitment.Models
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblUserRoles = new HashSet<TblUserRole>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsArchived { get; set; }

        public virtual ICollection<TblUserRole> TblUserRoles { get; set; }
    }
}
