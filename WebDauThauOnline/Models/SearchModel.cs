using System;

namespace WebDauThauOnline.Models
{
    public class SearchModel
    {
        public Kiểu_thông_tin Kiểu_thông_tin { get; set; }
        public Kiểu_thông_báo Kiểu_thông_báo { get; set; }
        public string Số_TBMT_Tên_gói_thầu { get; set; }
        public string Bên_mời_thầu { get; set; }
        public Phạm_vi? Phạm_vi { get; set; }
        public Loại_ngày? Loại_ngày { get; set; }
        public Khoảng_thời_gian? Khoảng_thời_gian { get; set; }
        public DateTime? Từ_ngày { get; set; }
        public DateTime? Đến_ngày { get; set; }
        public Hình_thức_dự_thầu? Hình_thức { get; set; }
        public Lĩnh_vực? Lĩnh_vực { get; set; }

    }


}