using System;
using System.Collections.Generic;

#nullable disable

namespace SuccessRecruitment.Models
{
    public partial class TblLogin
    {
        public int LoginId { get; set; }
        public Guid UserId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsArchived { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Tbluser User { get; set; }
    }
}
