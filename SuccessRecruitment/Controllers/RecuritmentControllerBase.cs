using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SuccessRecruitment.Controllers
{
    public class RecuritmentControllerBase: ControllerBase
    {
        //protected static Guid CurrentUser { get; set; }
        //protected Guid GetCurrentUser()
        //{
        //        if(User != null)
        //        {
        //            var currentUser = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        //            return CheckIfUserIsValid(currentUser);
        //        }
        //        return new Guid();
        //}
        //protected Guid CheckIfUserIsValid(Guid currenUser)
        //{
        //    try
        //    {
        //        if(currenUser == Guid.Empty)
        //        {
        //            throw new Exception("Unauthorised user");
        //        }
        //        else
        //        {
        //            return currenUser;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

    }
}
