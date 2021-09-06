using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuccessRecruitment.DataTransferObjects
{
    public class Pages
    {
        public List<PageInfo> Tabs { get; set; }
        public List<PageInfo> SubPages { get; set; }
        public List<PageInfo> ChildPages { get; set; }
    }

    public class PageInfo
    {
        public string PageName { get; set; }
        public int PageId { get; set; }
        public int? ParentPageId { get; set; }
    }


  
}
