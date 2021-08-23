using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuccessRecruitment.DataTransferObjects.Auth;
using SuccessRecruitment.Models;

namespace SuccessRecruitment.Services.Auth
{
    interface IAuthService
    {
        Task<Tbluser> Register(UserRegisterDTO newUser);
        Task<Tbluser> Login(UserLoginDTO user);
    }

    public class AuthService : IAuthService
    {
        RecruitmentDB _db = null;

        public AuthService()
        {
            _db = new RecruitmentDB();
        }

        public async Task<Tbluser> Login(UserLoginDTO user)
        {
            Tbluser userDetails = await _db.Tblusers.Include(x => x.TblLogin).Where(u => u.UserName == user.UserName.Trim()).FirstOrDefaultAsync();

            if (userDetails == null)
            {
                throw new Exception("Incorrect Username or Password");
            }

            HMACSHA512 hmac = new HMACSHA512(userDetails.TblLogin.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != userDetails.TblLogin.PasswordHash[i])
                {
                    throw new Exception("Incorrect Username or Password");
                }
            }

            return userDetails;
        }

        public async Task<Tbluser> Register(UserRegisterDTO newUser)
        {
            try
            {
                Tbluser User = new Tbluser();
                
                if(newUser.UserName == null || newUser.Email == null)
                {
                    throw new Exception("Username and Email are required");
                }

                bool userExists = await _db.Tblusers.AnyAsync(x => x.UserName == newUser.UserName && x.Email == newUser.Email && !x.IsArchived);

                if (userExists)
                {
                    throw new Exception("User Already exists. Please Log in");
                }
                else
                {
                    User = (await _db.Tblusers.AddAsync(new Tbluser
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
                        UserId = User.UserId,
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newUser.Password)),
                        PasswordSalt = hmac.Key,
                        CreatedBy = Guid.Parse("6B951FDB-31BE-4AE1-8B96-28A3075D7060"),
                        CreatedDate = DateTime.Now
                    });

                    await _db.SaveChangesAsync();
                }

                return User;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
         
        }

    }
}
