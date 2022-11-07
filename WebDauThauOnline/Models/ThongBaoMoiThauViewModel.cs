using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauThauOnline.Models
{
    public class ThongBaoMoiThauViewModel
    {
        public EmpFileModel EmpFileModel { get; set; }
        public ThongBaoMoiThau_ThongTinChiTiet ThongBaoMoiThauModel { get; set; }
        public IEnumerable<File> Files { get; set; }
    }
}