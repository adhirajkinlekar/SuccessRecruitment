using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SuccessRecruitment.DataTransferObjects.Auth;
using SuccessRecruitment.Models;

namespace SuccessRecruitment.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> Register(UserRegisterDTO newUser);
        Task<validuserdto> Login(UserLoginDTO user);
    }

    public class AuthService : IAuthService
    {
        private readonly RecruitmentDB _db;
        private readonly SymmetricSecurityKey _key;

        public AuthService(RecruitmentDB database, IConfiguration config)
        { 
            _db = database;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public async Task<validuserdto> Login(UserLoginDTO user)
        {
            try
            {
                Tbluser validUser = new Tbluser();
                validUser = await _db.Tblusers.Include(x => x.TblLogin).Include(x=> x.TblUserRoles).Include(x=> x.TblUserPages).Where(u => u.Email == user.Email).FirstOrDefaultAsync();
                
                if (validUser == null)
                {
                    throw new Exception("Incorrect Email or Password");
                }


                HMACSHA512 hmac = new HMACSHA512(validUser.TblLogin.PasswordSalt);

                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != validUser.TblLogin.PasswordHash[i])
                    {
                        throw new Exception("Incorrect Username or Password");
                    }
                }

                List<int> roleIds = validUser.TblUserRoles.Select(x => x.RoleId).ToList();
                List<int> pageIds = validUser.TblUserPages.Select(x => x.PageId).ToList();
                List<string> userRoles = await _db.TblUserRoles.Include(x => x.TblRole).Where(x => roleIds.Contains(x.RoleId) && !x.IsArchived).Select(x => x.TblRole.RoleName).ToListAsync();
                List<string> userPages = await _db.TblUserPages.Include(x => x.TblPage).Where(x => pageIds.Contains(x.PageId) && !x.IsArchived).Select(x => x.TblPage.PageName).ToListAsync();
                //try to get all these results in 1 query
                validuserdto validuserdto = new validuserdto();
                validuserdto.Token = CreateToken(validUser, userRoles, userPages);
                validuserdto.UserName = validUser.UserName;
                validuserdto.UserRoles = string.Join("/ ", userRoles);
                if (validuserdto.Token == null)
                {
                    throw new Exception("Unable to create Token. Please contact system administrator");
                }
               
                return validuserdto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
        }

        public async Task<bool> Register(UserRegisterDTO newUser)
        {
            try
            {
                Tbluser validUser = new Tbluser();

                newUser.Password = "1234";

                if (newUser.UserName == null || newUser.Email == null || newUser.RoleIds.Count == 0)
                {
                    throw new Exception("User details and role are required");
                }

                List<TblRole> roles = await _db.TblRoles.Where(x => newUser.RoleIds.Contains(x.RoleId)).ToListAsync();
                if (roles.Any(x=> x.IsArchived))
                {
                    throw new Exception("One or more selected roles have been archived. Please contact system administrator");
                }

                if(roles.Any(x => x.RoleName == "Recruiter" || x.RoleName == "Candidate") && roles.Count > 1)
                {
                    throw new Exception("Cant select any other role when selected role is either Recruiter or Candidate");
                }

                bool userExists = await _db.Tblusers.AnyAsync(x => x.UserName == newUser.UserName.Trim() && x.Email == newUser.Email.Trim() && !x.IsArchived);

                if (userExists)
                {
                    throw new Exception("User Already exists. Please Log in");
                }
                else
                {
                    validUser = (await _db.Tblusers.AddAsync(new Tbluser
                    {
                        UserId = Guid.NewGuid(),
                        UserName = newUser.UserName,
                        Email = newUser.Email,
                        Phone = newUser.Phone,
                        CreatedBy = Guid.Parse("3AF1BA96-4A39-467C-8AD9-3F418F199CD0"),
                        CreatedDate = DateTime.Now
                    })).Entity;

                    HMACSHA512 hmac = new HMACSHA512();
                    //If a column has been added an identity, the application has to be scafolled otherwise the application will pass a default value from the dataype
                    _db.TblLogins.Add(new TblLogin
                    {
                        UserId = validUser.UserId,
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password)),
                        PasswordSalt = hmac.Key,
                        CreatedBy = Guid.Parse("3AF1BA96-4A39-467C-8AD9-3F418F199CD0"),
                        CreatedDate = DateTime.Now
                    });

                    List<int> roleIds = roles.Select(x => x.RoleId).ToList();
                    
                    foreach (var roleId in roleIds)
                    {
                        validUser.TblUserRoles.Add((await _db.TblUserRoles.AddAsync(new TblUserRole
                        {
                            UserId = validUser.UserId,
                            RoleId = roleId,
                            CreatedBy = Guid.Parse("3AF1BA96-4A39-467C-8AD9-3F418F199CD0"),
                            CreatedDate = DateTime.Now
                        })).Entity);
                    }
                    List<int> tabs = await _db.TblRolePages.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.PageId).ToListAsync();
                    bool IsExternal = roles.Any(x => x.RoleName == "Recruiter" || x.RoleName == "Candidate");
                    bool hasAddEditprivileges = roles.Any(x => x.RoleName == "Admin" || x.RoleName == "General Manager" || x.RoleName == "Recruiter" || x.RoleName == "Candidate");
                    //By default a new user is not given access to editable pages unless their role is Admin or General Manager
                    List<int> subPages = await _db.TblPages.Where(x => tabs.Contains(x.ParentPageId.Value) && x.IsExternal == IsExternal && !x.IsArchived && !x.IsArchived && ( !x.IsAddEditPage || hasAddEditprivileges)).Select(x=> x.PageId).ToListAsync();

                    List<int> childPages = await _db.TblPages.Where(x => subPages.Contains(x.ParentPageId.Value) && x.IsExternal == IsExternal && !x.IsArchived && !x.IsArchived && (!x.IsAddEditPage || hasAddEditprivileges)).Select(x => x.PageId).ToListAsync();

                    List<int> pageIds = new List<int>();

                    pageIds.AddRange(tabs);
                    pageIds.AddRange(subPages);
                    pageIds.AddRange(childPages);

                    foreach (var pageId in pageIds)
                    {
                        validUser.TblUserPages.Add((await _db.TblUserPages.AddAsync(new TblUserPage
                        {
                            UserId = validUser.UserId,
                            PageId = pageId,
                            CreatedBy = Guid.Parse("3AF1BA96-4A39-467C-8AD9-3F418F199CD0"),
                            CreatedDate = DateTime.Now
                        })).Entity);
                    }
                }

                //string token = CreateToken(validUser, roles.Select(x => x.RoleName).ToList(), pages.Select(x=> x.PageName).ToList());

                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
         
        }

        private string CreateToken(Tbluser user, List<string> roleNames, List<string> pageNames)
        {
            try
            {
                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };
                foreach (var roleName in roleNames)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleName));
                }
                foreach (var pageName in pageNames)
                {
                    claims.Add(new Claim("Pages", pageName));
                }
                
                SigningCredentials creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
