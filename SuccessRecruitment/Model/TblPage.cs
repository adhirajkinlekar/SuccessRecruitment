using System;
using System.Collections.Generic;

#nullable disable

namespace SuccessRecruitment.Models
{
    public partial class TblPage
    {
        public TblPage()
        {
            InverseParentPage = new HashSet<TblPage>();
            TblRolePages = new HashSet<TblRolePage>();
            TblUserPages = new HashSet<TblUserPage>();
        }

        public int PageId { get; set; }
        public string PageName { get; set; }
        public int? ParentPageId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsTab { get; set; }
        public bool IsAddEditPage { get; set; }
        public bool IsArchived { get; set; }
        public bool IsExternal { get; set; }

        public virtual TblPage ParentPage { get; set; }
        public virtual ICollection<TblPage> InverseParentPage { get; set; }
        public virtual ICollection<TblRolePage> TblRolePages { get; set; }
        public virtual ICollection<TblUserPage> TblUserPages { get; set; }
    }
}
