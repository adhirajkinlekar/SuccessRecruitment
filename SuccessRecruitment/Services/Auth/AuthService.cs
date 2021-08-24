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
        Task<ValidUserDTO> Register(UserRegisterDTO newUser);
        Task<ValidUserDTO> Login(UserLoginDTO user);
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

        public async Task<ValidUserDTO> Login(UserLoginDTO user)
        {
            try
            {
               
                ValidUserDTO validUser = new ValidUserDTO();
                validUser.UserDetails = await _db.Tblusers.Include(x => x.TblLogin).Where(u => u.UserName == user.UserName).FirstOrDefaultAsync();

                if (validUser.UserDetails == null)
                {
                    throw new Exception("Incorrect Username or Password");
                }

                HMACSHA512 hmac = new HMACSHA512(validUser.UserDetails.TblLogin.PasswordSalt);

                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != validUser.UserDetails.TblLogin.PasswordHash[i])
                    {
                        throw new Exception("Incorrect Username or Password");
                    }
                }

                validUser.Token = CreateToken(validUser.UserDetails);

                if (validUser.Token == null)
                {
                    throw new Exception("Unable to create Token. Please contact system administrator");
                }
               
                return validUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          
        }

        public async Task<ValidUserDTO> Register(UserRegisterDTO newUser)
        {
            try
            {
                ValidUserDTO validUser = new ValidUserDTO();
                
                if(newUser.UserName == null || newUser.Email == null)
                {
                    throw new Exception("Username and Email are required");
                }

                bool userExists = await _db.Tblusers.AnyAsync(x => x.UserName == newUser.UserName.Trim() && x.Email == newUser.Email.Trim() && !x.IsArchived);

                if (userExists)
                {
                    throw new Exception("User Already exists. Please Log in");
                }
                else
                {
                    validUser.UserDetails = (await _db.Tblusers.AddAsync(new Tbluser
                    {
                        UserId = Guid.NewGuid(),
                        UserName = newUser.UserName,
                        Email = newUser.Email,
                        Phone = newUser.Phone,
                        CreatedBy = Guid.Parse("6B951FDB-31BE-4AE1-8B96-28A3075D7060"),
                        CreatedDate = DateTime.Now
                    })).Entity;

                    HMACSHA512 hmac = new HMACSHA512();
                    //If a column has been added an identity, the application has to be scafolled otherwise the application will pass a default value from the dataype
                    _db.TblLogins.Add(new TblLogin
                    {
                        UserId = validUser.UserDetails.UserId,
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password)),
                        PasswordSalt = hmac.Key,
                        CreatedBy = Guid.Parse("6B951FDB-31BE-4AE1-8B96-28A3075D7060"),
                        CreatedDate = DateTime.Now
                    });
                }

                validUser.Token = CreateToken(validUser.UserDetails);

                if (validUser.Token == null)
                {
                    throw new Exception("Unable to create Token. Please contact system administrator");
                }

                await _db.SaveChangesAsync();

                return validUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
         
        }

        private string CreateToken(Tbluser user)
        {
             List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                //new Claim(ClaimTypes.Role, user.Role)
            };


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
    }
}
