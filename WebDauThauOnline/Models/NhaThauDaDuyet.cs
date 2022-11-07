//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebDauThauOnline.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class NhaThauDaDuyet
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Bắt buộc.")]
        public string Tên_nhà_thầu { get; set; }
        public string Số_ĐKKD { get; set; }
        public string Địa_chỉ { get; set; }
        public Nullable<System.DateTime> Ngày_phê_duyệt { get; set; }
        public string Loại_hình_doanh_nghiệp { get; set; }
        public Nullable<System.DateTime> Ngày_thành_lập { get; set; }
        public string Tên_tiếng_Anh { get; set; }
        public string Tỉnh_Thành_phố { get; set; }
        public string Quốc_gia { get; set; }
        public string Địa_chỉ_giao_dịch { get; set; }
        public Nullable<int> Số_nhân_viên { get; set; }
        public string Vị_trí_nhà_thầu { get; set; }
        public string Số_điện_thoại { get; set; }
        public string Số_Fax { get; set; }
        public string Trạng_thái_đóng_phí { get; set; }
        public Nullable<int> AccountID { get; set; }

        public Loại_hình_doanh_nghiệp Loại_hình_doanh_nghiệp_EnumValue { get; set; }
        public Tỉnh_Thành_phố? Tỉnh_Thành_phố_EnumValue { get; set; }
        public Quốc_gia Quốc_gia_EnumValue { get; set; }
        public Trạng_thái_đóng_phí Trạng_thái_đóng_phí_EnumValue { get; set; }
    }
}
