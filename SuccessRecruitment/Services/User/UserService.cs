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
        Task<UserDTO> GetUser(Guid id);
        Task<List<UserDTO>> GetUsers();
        Task<List<RoleDTO>> GetRoles();
        Task<bool> UpdateUser(UpdateUserDTO updateUserDTO);
        Task<List<UserPagesDTO>> GetUserPages(Guid id);
        Task<bool> UpdateUserPages(Guid id,List<UpdatePagesDTO> updatedPages);
        
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

        public async Task<UserDTO> GetUser(Guid id)
        {
            try
            {
                var user = await _db.Tblusers.Where(x => x.UserId == id && !x.IsArchived).Select(x => new UserDTO
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Email = x.Email,
                    Phone = x.Phone.ToString(),
                    RoleName = string.Join(",", x.TblUserRoles.ToList().Select(x => x.TblRole.RoleName).ToList()),
                    RoleIds = x.TblUserRoles.ToList().Where(y=> !y.IsArchived).Select(x=> x.RoleId).ToList()
                }).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

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
                    RoleName = string.Join(",", x.TblUserRoles.ToList().Where(y=> !y.IsArchived).Select(x => x.TblRole.RoleName).ToList())
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
        public async Task<bool> UpdateUser(UpdateUserDTO updateUserDTO)
        {
            try
            {
                if (updateUserDTO.UserName == null || updateUserDTO.Email == null || updateUserDTO.RoleIds.Count == 0)
                {
                    throw new Exception("User details and role are required");
                }

                List<TblRole> roles = await _db.TblRoles.Where(x => updateUserDTO.RoleIds.Contains(x.RoleId)).ToListAsync();

                if (roles.Any(x => x.IsArchived))
                {
                    throw new Exception("One or more selected roles have been archived. Please contact system administrator");
                }

                if (roles.Any(x => x.RoleName == "Recruiter" || x.RoleName == "Candidate") && roles.Count > 1)
                {
                    throw new Exception("Cant select any other role when selected role is either Recruiter or Candidate");
                }

                var user = await _db.Tblusers.Where(x => x.UserId == updateUserDTO.UserId && !x.IsArchived).FirstOrDefaultAsync();

                user.Email = updateUserDTO.Email;
                user.UserName = updateUserDTO.UserName;
                user.Phone = updateUserDTO.Phone;

                var existingUserRoles = await _db.TblUserRoles.Where(x => x.UserId == updateUserDTO.UserId && !x.IsArchived).ToListAsync();
                var existingRoleIds = existingUserRoles.Select(x => x.RoleId).ToList();
                if (!existingRoleIds.SequenceEqual(updateUserDTO.RoleIds))
                {
                    var newRoleIds = updateUserDTO.RoleIds.Where(id => !existingRoleIds.Contains(id)).ToList();
                    var removedRoleIds = existingRoleIds.Where(id => !updateUserDTO.RoleIds.Contains(id)).ToList();


                    if (removedRoleIds.Count > 0)
                    {
                        foreach (var role in existingUserRoles.Where(x => !updateUserDTO.RoleIds.Contains(x.RoleId)).ToList())
                        {
                            role.IsArchived = true;
                        }


                        var rolePages = await _db.TblRolePages.Where(x => removedRoleIds.Contains(x.RoleId)).Select(x => x.PageId).ToListAsync();

                        var toBeArchivedPages = await _db.TblUserPages.Where(x => rolePages.Contains(x.PageId) && x.UserId == updateUserDTO.UserId).ToListAsync();
                        foreach (var page in toBeArchivedPages)
                        {
                            page.IsArchived = true;
                        }

                    }

                    if (newRoleIds.Count > 0)
                    {
                        var userRoles = await _db.TblUserRoles.Where(x => newRoleIds.Contains(x.RoleId) && x.UserId == updateUserDTO.UserId && x.IsArchived).ToListAsync();

                        foreach (var roleId in newRoleIds)
                        {
                            var archivedRole = userRoles.Where(x => x.RoleId == roleId).FirstOrDefault();
                            if (archivedRole != null)
                            {
                                archivedRole.IsArchived = false;
                            }
                            else
                            {
                                _db.TblUserRoles.Add(new TblUserRole
                                {
                                    UserId = updateUserDTO.UserId,
                                    RoleId = roleId,
                                    CreatedBy = Guid.Parse("3AF1BA96-4A39-467C-8AD9-3F418F199CD0"),
                                    CreatedDate = DateTime.Now
                                });
                            }
                        }

                        var rolePageIds = await _db.TblRolePages.Where(x => newRoleIds.Contains(x.RoleId) && !x.IsArchived).Select(x => x.PageId).ToListAsync();

                        var userPages = await _db.TblUserPages.Where(x => x.UserId == updateUserDTO.UserId && rolePageIds.Contains(x.PageId)).ToListAsync();

                        foreach (var pageId in rolePageIds)
                        {
                            var userPage = userPages.Where(x => x.PageId == pageId).FirstOrDefault();

                            if (userPage != null)
                            {
                                userPage.IsArchived = false;
                            }
                            else
                            {
                                _db.TblUserPages.Add(new TblUserPage
                                {
                                    UserId = updateUserDTO.UserId,
                                    PageId = pageId,
                                    CreatedBy = Guid.Parse("3AF1BA96-4A39-467C-8AD9-3F418F199CD0"),
                                    CreatedDate = DateTime.Now
                                });
                            }
                        }

                    }
                }
               
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UserPagesDTO>> GetUserPages(Guid id)
        {
            try
            {
                var userRoles = await _db.TblUserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).ToListAsync();

                var rolePages = await _db.TblRolePages.Where(x => userRoles.Contains(x.RoleId)).Select(x => new UserPagesDTO
                {
                    PageId = x.PageId,
                    PageName = x.TblPage.PageName,
                    IsTab = x.TblPage.IsTab,
                    TabId = x.TblPage.TabId.HasValue ? x.TblPage.TabId.Value : null,
                    IsAccessible = x.TblPage.TblUserPages.FirstOrDefault(y => y.UserId == id && !y.IsArchived) != null
                }).ToListAsync();

                return rolePages;
            }
         
               catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UpdateUserPages(Guid id, List<UpdatePagesDTO> updatedPages)
        {

            var userPages = await _db.TblUserPages.Where(x => x.UserId == id).ToListAsync();

            foreach(var page in updatedPages)
            {
                var userPage = userPages.Where(x => x.PageId == page.PageId).FirstOrDefault();

                if (page.IsAccessible)
                {
                    if (userPage != null)
                    {
                        userPage.IsArchived = false;
                    }
                    else
                    {
                        _db.TblUserPages.Add(new TblUserPage
                        {
                            UserId = id,
                            PageId = page.PageId,
                            CreatedBy = Guid.Parse("3AF1BA96-4A39-467C-8AD9-3F418F199CD0"),
                            CreatedDate = DateTime.Now
                        });
                    }
                }
                else
                {
                    userPage.IsArchived = true;
                }
                await _db.SaveChangesAsync();

            }
            return true;
        }
    }
}
