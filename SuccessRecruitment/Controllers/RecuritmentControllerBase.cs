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

            var pages = User.FindAll(x => x.Type == "Pages");

            List<string> pageNames = new List<string>();

            foreach (var page in pages)
            {
                pageNames.Add(page.Value);
            }

            if(pageNames.Any(name=> name == pageName))
            {
                return true;
            }

            return false;
            }

        }
}
