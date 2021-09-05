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

        public bool HasPageAcces(string pageName)
        {

            if(User.FindAll(x => x.Type == "Pages").Select(x => x.Value).ToList().Any(name => name == pageName))
            {
                return true;
            }

            return false;
            }
        }
}
