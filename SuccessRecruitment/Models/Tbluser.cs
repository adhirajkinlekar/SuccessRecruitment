using System;
using System.Collections.Generic;

#nullable disable

namespace SuccessRecruitment.Models
{
    public partial class Tbluser
    {
        public Tbluser()
        {
            TblJobs = new HashSet<TblJob>();
            TblUserPages = new HashSet<TblUserPage>();
            TblUserRoles = new HashSet<TblUserRole>();
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public decimal? Phone { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsArchived { get; set; }

        public virtual TblLogin TblLogin { get; set; }
        public virtual ICollection<TblJob> TblJobs { get; set; }
        public virtual ICollection<TblUserPage> TblUserPages { get; set; }
        public virtual ICollection<TblUserRole> TblUserRoles { get; set; }
    }
}
