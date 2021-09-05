using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SuccessRecruitment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SuccessRecruitment.Services.Home
{
    public interface IHomeService
    {
        Task GetAppInformation();
    }

    public class HomeService : IHomeService
    {
        private readonly RecruitmentDB _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeService(RecruitmentDB database, IHttpContextAccessor IHttpContextAccessor)
        {
            _db = database;
            _httpContextAccessor = IHttpContextAccessor;
        }

        public async Task GetAppInformation()
        {
            var currentUser = GetUserId();

            //List<int> userRoles = await _db.TblUserRoles.Where(x => x.UserId == currentUser && !x.IsArchived).Select(x => x.RoleId).ToListAsync();

            //List<TblPage> userPages = await _db.TblUserPages.Where(x => x.UserId == currentUser && !x.TblPage.IsExternal).Select(x => x.TblPage).ToListAsync();

            return;
        }

        private Guid GetUserId() => Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}