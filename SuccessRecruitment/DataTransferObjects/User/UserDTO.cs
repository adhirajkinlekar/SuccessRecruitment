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
    }
}
