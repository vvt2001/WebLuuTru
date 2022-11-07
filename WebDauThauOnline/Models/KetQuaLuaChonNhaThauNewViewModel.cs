using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauThauOnline.Models
{
    public class KetQuaLuaChonNhaThauNewViewModel
    {
        public EmpFileModel EmpFileModel { get; set; }
        public KetQuaLuaChonNhaThau_ThongTinChiTiet KetQuaLuaChonNhaThau_ThongTinChiTiet { get; set; }
        public IEnumerable<File> Files { get; set; }
    }
}