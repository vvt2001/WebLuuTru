using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauThauOnline.Models
{
    public class BenMoiThauSearchViewModel
    {
        public BenMoiThauSearchModel BenMoiThauSearchModel { get; set; }
        public IPagedList<BenMoiThauDaDuyet> BenMoiThauDaDuyetModel { get; set; }
    }
}