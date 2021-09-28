using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SuccessRecruitment.DataTransferObjects.User;
using SuccessRecruitment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuccessRecruitment.Services.User
{
    public interface IUser
    {
        Task<List<UserDTO>> GetUsers();
        Task<List<RoleDTO>> GetRoles();
    }
    public class UserService:IUser
    {
        private readonly RecruitmentDB _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(RecruitmentDB database, IHttpContextAccessor IHttpContextAccessor)
        {
            _db = database;
            _httpContextAccessor = IHttpContextAccessor;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            try
            {
                var users = await _db.Tblusers.Where(x => !x.IsArchived).Select(x => new UserDTO
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Email = x.Email,
                    Phone = x.Phone == null ? "not provided" : x.Phone.ToString(),
                    RoleName = string.Join(",", x.TblUserRoles.ToList().Select(x => x.TblRole.RoleName).ToList())
                }).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
        }

        public async Task<List<RoleDTO>> GetRoles()
        {
            try
            {
                List<RoleDTO> roles = await _db.TblRoles.Where(x => !x.IsArchived).Select(x => new RoleDTO
                {
                    RoleId = x.RoleId,
                    RoleName = x.RoleName
                }).ToListAsync();

                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
