﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessRecruitment.Services.User;
using System;
using SuccessRecruitment.Shared.Pages;
using System.Threading.Tasks;

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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
    }
}
