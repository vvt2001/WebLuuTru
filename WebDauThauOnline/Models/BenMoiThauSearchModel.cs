using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauThauOnline.Models
{
    public class BenMoiThauSearchModel
    {
        public string Mã_cơ_quan { get; set; }
        public string Tên_bên_mời_thầu { get; set; }
        public Bộ_ban_ngành? Bộ_ban_ngành { get; set; }
        public Tập_đoàn_TCT? Tập_đoàn_TCT { get; set; }
        public Tỉnh_Thành_phố? Tỉnh_Thành_phố { get; set; }
        public DateTime? Từ_ngày { get; set; }
        public DateTime? Đến_ngày { get; set; }
    }
}