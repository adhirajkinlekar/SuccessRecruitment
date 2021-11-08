using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SuccessRecruitment.DataTransferObjects;
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
        Task<Pages> GetAppInformation();
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

        public async Task<Pages> GetAppInformation()
        {
            try
            {
                var currentUser = GetUserId();

                var userPages = await _db.TblUserPages.Include(x => x.TblPage).Where(x => x.UserId == currentUser && !x.IsArchived).ToListAsync();

                Pages pages = new Pages();

                pages.Tabs = userPages.Where(x => x.TblPage.IsTab).Select(x => new PageInfo
                {
                   PageId = x.TblPage.PageId,
                   PageName = x.TblPage.PageName,
                   ParentPageId = x.TblPage.ParentPageId != null ? x.TblPage.ParentPageId.Value : null
                }).ToList();

                var tabIds = pages.Tabs.Select(x => x.PageId).ToList();

                pages.SubPages = userPages.Where(x => x.TblPage.ParentPageId != null && tabIds.Contains(x.TblPage.ParentPageId.Value)).Select(x => new PageInfo {
                    PageId = x.PageId,
                    PageName= x.TblPage.PageName,
                    PageLink = x.TblPage.PageLink,
                    ParentPageId = x.TblPage.ParentPageId != null ? x.TblPage.ParentPageId.Value : null
                }).ToList();

                var subPageIds = pages.SubPages.Select(x => x.PageId).ToList();
                 
                pages.ChildPages = userPages.Where(x => x.TblPage.ParentPageId != null && subPageIds.Contains(x.TblPage.ParentPageId.Value)).Select(x => new PageInfo
                {
                    PageId = x.PageId,
                    PageName = x.TblPage.PageName,
                    PageLink = x.TblPage.PageLink,
                    ParentPageId = x.TblPage.ParentPageId != null ? x.TblPage.ParentPageId.Value : null
                }).ToList();
              

                return pages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private Guid GetUserId() => Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}