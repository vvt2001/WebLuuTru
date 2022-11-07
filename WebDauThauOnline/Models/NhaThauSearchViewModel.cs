using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace WebDauThauOnline.Models
{
    public class NhaThauSearchViewModel
    {
        public NhaThauSearchModel NhaThauSearchModel { get; set; }
        public IPagedList<NhaThauDaDuyet> NhaThauDaDuyetModel { get; set; }
    }
}