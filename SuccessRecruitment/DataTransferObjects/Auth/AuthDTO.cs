using SuccessRecruitment.Models;
using System;
using System.Collections.Generic;

namespace SuccessRecruitment.DataTransferObjects.Auth
{
    public class UserRegisterDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<int> RoleIds { get; set; }
        public decimal? Phone { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class CreateLoginDTO
    {
       public Guid UserId { get; set; }
       public byte[] PasswordHash { get; set; }
       public byte[] PasswordSalt { get; set; }
       public Guid CreatedBy { get; set; }
       public DateTime CreatedDate { get; set; }
    }
    public class validuserdto
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string UserRoles { get; set; }
    }
}

