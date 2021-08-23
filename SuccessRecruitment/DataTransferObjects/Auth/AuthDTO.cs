using System;

namespace SuccessRecruitment.DataTransferObjects.Auth
{
    public class UserRegisterDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal? Phone { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class UserLoginDTO
    {
        public string UserName { get; set; }
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
}

