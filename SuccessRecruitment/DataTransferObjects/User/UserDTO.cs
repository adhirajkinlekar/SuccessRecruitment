using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuccessRecruitment.DataTransferObjects.User
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RoleName { get; set; }
        public bool IsArchived { get; set; }
        public List<int> RoleIds { get; set; }
    }
    public class RoleDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class UpdateUserDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public decimal? Phone { get; set; }
        public List<int> RoleIds { get; set; }
    }
    public class UserPagesDTO
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public bool IsTab { get; set; }
        public int? TabId { get; set; }
        public bool IsAccessible { get; set; }
    }

    public class UpdatePagesDTO
    {
        public int PageId { get; set; }
        public bool IsAccessible { get; set; }
    }
}

