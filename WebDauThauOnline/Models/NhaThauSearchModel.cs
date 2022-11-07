using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDauThauOnline.Models
{
    public class NhaThauSearchModel
    {
        public Nhà_thầu? Nhà_Thầu { get; set; }
        public string Số_ĐKKD { get; set; }
        public Tỉnh_Thành_phố? Tỉnh_Thành_Phố { get; set; }
        public string Tên_nhà_thầu { get; set; }
        public DateTime? Từ_ngày { get; set; }
        public DateTime? Đến_ngày { get; set; }

    }
}