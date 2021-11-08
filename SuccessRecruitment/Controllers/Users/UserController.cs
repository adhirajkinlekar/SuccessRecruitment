using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessRecruitment.Services.User;
using System;
using SuccessRecruitment.Shared.Pages;
using System.Threading.Tasks;
using SuccessRecruitment.DataTransferObjects.User;
using System.Collections.Generic;

namespace SuccessRecruitment.Controllers.Users
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : RecuritmentControllerBase
    {

        private readonly IUser _repo;

        public UserController(IUser userService)
        {
            _repo = userService;
        }

        [Route("{id}")]
        [Authorize(Roles = "Administrator,Manager")]
        [HttpGet]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                if (!HasPageAcces(Pages.ViewUsersPage))
                {
                    return Unauthorized();
                }

                return Ok(await _repo.GetUser(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("AllUsers")]
        [Authorize(Roles = "Administrator,Manager")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                if (!HasPageAcces(Pages.ViewUsersPage))
                {
                    return Unauthorized();
                }

                return Ok(await _repo.GetUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("Roles")]
        [Authorize(Roles = "Administrator,Manager")]
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                if (!HasPageAcces(Pages.ViewUsersPage))
                {
                    return Unauthorized();
                }

                return Ok(await _repo.GetRoles());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("UpdateUser")]
        [Authorize(Roles = "Administrator,Manager")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO updateUserDTO)
        {
            try
            {
                if (!HasPageAcces(Pages.ViewUsersPage))
                {
                    return Unauthorized();
                }

                return Ok(await _repo.UpdateUser(updateUserDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}/pages")]
        [Authorize(Roles = "Administrator,Manager")]
        [HttpGet]
        public async Task<IActionResult> GetUserPages(Guid id)
        {
            try
            {
                if (!HasPageAcces(Pages.ViewUsersPage))
                {
                    return Unauthorized();
                }

                return Ok(await _repo.GetUserPages(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("{id}/UpdatePages")]
        [Authorize(Roles = "Administrator,Manager")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserPages(Guid id,List<UpdatePagesDTO> updatePages)
        {
            try
            {
                if (!HasPageAcces(Pages.ViewUsersPage))
                {
                    return Unauthorized();
                }

                return Ok(await _repo.UpdateUserPages(id, updatePages));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
