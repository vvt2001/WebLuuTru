using System;
using System.ComponentModel;

namespace WebDauThauOnline.Models
{
    public static class EnumExtension
    {
        public static string ToDescriptionString(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            return default(T);
        }
    }
    public enum Kiểu_thông_tin
    {
        [Description("Thông báo mời thầu")]
        Thông_báo_mời_thầu,
        [Description("Kết quả lựa chọn nhà thầu")]
        Kết_quả_lựa_chọn_nhà_thầu
    }
    public enum Kiểu_thông_báo
    {
        [Description("Thông báo mời thầu")]
        Thông_báo_mời_thầu,
    }
    public enum Phạm_vi
    {
        [Description("Trong phạm vi của luật đấu thầu")]
        Trong_phạm_vi_của_luật_đấu_thầu,
        [Description("Ngoài phạm vi của luật đấu thầu")]
        Ngoài_phạm_vi_của_luật_đấu_thầu
    }
    public enum Loại_ngày
    {
        [Description("Ngày đăng tải")]
        Ngày_đăng_tải,
        [Description("Ngày đóng thầu")]
        Ngày_đóng_thầu,
        [Description("Ngày phát hành HSMT")]
        Ngày_phát_hành_HSMT
    }
    public enum Khoảng_thời_gian
    {
        [Description("-Chọn khoảng thời gian-")]
        mặc_định,
        [Description("1 tuần gần nhất")]
        một_tuần_gần_nhất,
        [Description("6 tuần gần nhất")]
        sáu_tuần_gần_nhất,
        [Description("1 tháng gần nhất")]
        một_tháng_gần_nhất,
        [Description("1 năm gần nhất")]
        một_năm_gần_nhất,
    }
    public enum Hình_thức_dự_thầu
    {
        [Description("Qua mạng")]
        Qua_mạng,
        [Description("Không qua mạng")]
        Không_qua_mạng
    }
    public enum Lĩnh_vực
    {
        [Description("Hàng hóa")]
        Hàng_hóa,
        [Description("Xây lắp")]
        Xây_lắp,
        [Description("Tư vấn")]
        Tư_vấn,
        [Description("Hỗn hợp")]
        Hỗn_hợp,
        [Description("Phi tư vấn")]
        Phi_tư_vấn
    }
    public enum Phân_loại
    {
        [Description("Dự án đầu tư phát triển")]
        Dự_án_đầu_tư_phát_triển,
        [Description("Hoạt động chi thường xuyên")]
        Hoạt_động_chi_thường_xuyên
    }
    public enum Loại_hợp_đồng
    {
        [Description("Trọn gói")]
        Trọn_gói,
    }
    public enum Hình_thức_lựa_chọn_nhà_thầu
    {
        [Description("Chào hàng cạnh tranh trong nước")]
        Chào_hàng_cạnh_tranh_trong_nước,
        [Description("Đấu thầu rộng rãi trong nước")]
        Đấu_thầu_rộng_rãi_trong_nước
    }
    public enum Phương_thức_LCNT
    {
        [Description("Một giai đoạn một túi hồ sơ")]
        Một_giai_đoạn_một_túi_hồ_sơ,
        [Description("Một giai đoạn hai túi hồ sơ")]
        Một_giai_đoạn_hai_túi_hồ_sơ,    
        [Description("Hai giai đoạn một túi hồ sơ")]
        Hai_giai_đoạn_một_túi_hồ_sơ,        
        [Description("Hai giai đoạn hai túi hồ sơ")]
        Hai_giai_đoạn_hai_túi_hồ_sơ
    }
    public enum Hình_thức_bảo_đảm_dự_thầu
    {
        [Description("Thư bảo lãnh")]
        Thư_bảo_lãnh,
    }
    public enum Hình_thức_lựa_chọn_NT
    {
        [Description("Chỉ định thầu")]
        Chỉ_định_thầu,
        [Description("Chỉ định thầu rút gọn")]
        Chỉ_định_thầu_rút_gọn

    }
    public enum Hình_thức_hợp_đồng
    {
        [Description("Trọn gói")]
        Trọn_gói
    }

    public enum Tỉnh_Thành_phố
    {
        [Description("An Giang")]
        An_Giang,
        [Description("Bà Rịa – Vũng Tàu")]
        Bà_Rịa_Vũng_Tàu,
        [Description("Bắc Giang")]
        Bắc_Giang,
        [Description("Bắc Kạn")]
        Bắc_Kạn,
        [Description("Bạc Liêu")]
        Bạc_Liêu,
        [Description("Bắc Ninh")]
        Bắc_Ninh,
        [Description("Bến Tre")]
        Bến_Tre,
        [Description("Bình Định")]
        Bình_Định,
        [Description("Bình Dương")]
        Bình_Dương,
        [Description("Bình Phước")]
        Bình_Phước,
        [Description("Bình Thuận")]
        Bình_Thuận,
        [Description("Cà Mau")]
        Cà_Mau,
        [Description("Cần Thơ")]
        Cần_Thơ,
        [Description("Cao Bằng")]
        Cao_Bằng,
        [Description("Đà Nẵng")]
        Đà_Nẵng,
        [Description("Đắk Lắk")]
        Đắk_Lắk,
        [Description("Đắk Nông")]
        Đắk_Nông,
        [Description("Điện Biên")]
        Điện_Biên,
        [Description("Đồng Nai")]
        Đồng_Nai,
        [Description("Đồng Tháp")]
        Đồng_Tháp,
        [Description("Gia Lai")]
        Gia_Lai,
        [Description("Hà Giang")]
        Hà_Giang,
        [Description("Hà Nam")]
        Hà_Nam,
        [Description("Hà Nội")]
        Hà_Nội,
        [Description("Hà Tĩnh")]
        Hà_Tĩnh,
        [Description("Hải Dương")]
        Hải_Dương,
        [Description("Hải Phòng")]
        Hải_Phòng,
        [Description("Hậu Giang")]
        Hậu_Giang,
        [Description("Hòa Bình")]
        Hòa_Bình,
        [Description("Hưng Yên")]
        Hưng_Yên,
        [Description("Khánh Hòa")]
        Khánh_Hòa,
        [Description("Kiên Giang")]
        Kiên_Giang,
        [Description("Kon Tum")]
        Kon_Tum,
        [Description("Lai Châu")]
        Lai_Châu,
        [Description("Lâm Đồng")]
        Lâm_Đồng,
        [Description("Lạng Sơn")]
        Lạng_Sơn,
        [Description("Lào Cai")]
        Lào_Cai,
        [Description("Long An")]
        Long_An,
        [Description("Nam Định")]
        Nam_Định,
        [Description("Nghệ An")]
        Nghệ_An,
        [Description("Ninh Bình")]
        Ninh_Bình,
        [Description("Ninh Thuận")]
        Ninh_Thuận,
        [Description("Phú Thọ")]
        Phú_Thọ,
        [Description("Phú Yên")]
        Phú_Yên,
        [Description("Quảng Bình")]
        Quảng_Bình,
        [Description("Quảng Nam")]
        Quảng_Nam,
        [Description("Quảng Ngãi")]
        Quảng_Ngãi,
        [Description("Quảng Ninh")]
        Quảng_Ninh,
        [Description("Quảng Trị")]
        Quảng_Trị,
        [Description("Sóc Trăng")]
        Sóc_Trăng,
        [Description("Sơn La")]
        Sơn_La,
        [Description("Tây Ninh")]
        Tây_Ninh,
        [Description("Thái Bình")]
        Thái_Bình,
        [Description("Thái Nguyên")]
        Thái_Nguyên,
        [Description("Thanh Hóa")]
        Thanh_Hóa,
        [Description("Thừa Thiên Huế")]
        Thừa_Thiên_Huế,
        [Description("Tiền Giang")]
        Tiền_Giang,
        [Description("TP Hồ Chí Minh")]
        TP_Hồ_Chí_Minh,
        [Description("Trà Vinh")]
        Trà_Vinh,
        [Description("Tuyên Quang")]
        Tuyên_Quang,
        [Description("Vĩnh Long")]
        Vĩnh_Long,
        [Description("Vĩnh Phúc")]
        Vĩnh_Phúc,
        [Description("Yên Bái")]
        Yên_Bái,
    }
    public enum Nhà_thầu
    {
        [Description("Trong nước")]
        Trong_nước,
        [Description("Nước ngoài")]
        Nước_ngoài
    }

    public enum Bộ_ban_ngành
    {
        [Description("Bộ Công an")]
        Bộ_Công_an,
        [Description("Bộ Công Thương")]
        Bộ_Công_Thương,
        [Description("Bộ Giáo dục và Đào tạo")]
        Bộ_Giáo_dục_và_Đào_tạo,
        [Description("Bộ Giao thông vận tải")]
        Bộ_Giao_thông_vận_tải,
        [Description("Bộ Kế hoạch và Đầu tư")]
        Bộ_Kế_hoạch_và_Đầu_tư,
        [Description("Bộ Khoa học và Công nghệ")]
        Bộ_Khoa_học_và_Công_nghệ,
        [Description("Bộ Lao động - Thương Binh và Xã hội")]
        Bộ_Lao_động_Thương_Binh_và_Xã_hội,
        [Description("Bộ Ngoại giao")]
        Bộ_Ngoại_giao,
        [Description("Bộ Nội vụ")]
        Bộ_Nội_vụ,
        [Description("Bộ Nông nghiệp và Phát triển nông thôn")]
        Bộ_Nông_nghiệp_và_Phát_triển_nông_thôn,
        [Description("Bộ Quốc phòng")]
        Bộ_Quốc_phòng,
        [Description("Bộ Tài chính")]
        Bộ_Tài_chính,
        [Description("Bộ Tài nguyên và Môi trường")]
        Bộ_Tài_nguyên_và_Môi_trường,
        [Description("Bộ Thông tin và Truyền thông")]
        Bộ_Thông_tin_và_Truyền_thông,
        [Description("Bộ Tư pháp")]
        Bộ_Tư_pháp,
        [Description("Bộ Văn hóa - Thể thao và Du lịch")]
        Bộ_Văn_hóa_Thể_thao_và_Du_lịch,
        [Description("Bộ Xây dựng")]
        Bộ_Xây_dựng,
        [Description("Bộ Y tế")]
        Bộ_Y_tế,
        [Description("Văn phòng Chính phủ")]
        Văn_phòng_Chính_phủ,
        [Description("Thanh tra Chính phủ")]
        Thanh_tra_Chính_phủ,
        [Description("Ngân hàng Nhà nước Việt Nam")]
        Ngân_hàng_Nhà_nước_Việt_Nam,
        [Description("Ủy ban Dân tộc")]
        Ủy_ban_Dân_tộc,
        [Description("Đài Tiếng nói Việt Nam")]
        Đài_Tiếng_nói_Việt_Nam,
        [Description("Bảo hiểm Xã hội Việt Nam")]
        Bảo_hiểm_Xã_hội_Việt_Nam,
        [Description("Đài Truyền hình Việt Nam")]
        Đài_Truyền_hình_Việt_Nam,
        [Description("Viện Hàn lâm Khoa học và Công nghệ Việt Nam")]
        Viện_Hàn_lâm_Khoa_học_và_Công_nghệ_Việt_Nam,
        [Description("Ban Quản lý Lăng Chủ tịch Hồ Chí Minh")]
        Ban_Quản_lý_Lăng_Chủ_tịch_Hồ_Chí_Minh,
        [Description("Thông tấn xã Việt Nam")]
        Thông_tấn_xã_Việt_Nam,
        [Description("Học viện Chính trị Quốc gia Hồ Chí Minh")]
        Học_viện_Chính_trị_Quốc_gia_Hồ_Chí_Minh,
        [Description("Viện Hàn lâm Khoa học Xã hội Việt Nam")]
        Viện_Hàn_lâm_Khoa_học_Xã_hội_Việt_Nam,
        [Description("Kiểm toán nhà nước")]
        Kiểm_toán_nhà_nước,
        [Description("Khác")]
        Khác,
    }
    public enum Tập_đoàn_TCT
    {
        [Description("Tập đoàn điện lực Việt Nam (EVN)")]
        Tập_đoàn_điện_lực_Việt_Nam,
        [Description("Tập đoàn Dầu khí Quốc gia Việt Nam")]
        Tập_đoàn_Dầu_khí_Quốc_gia_Việt_Nam,
        [Description("Tổng công ty Hàng không Việt Nam")]
        Tổng_công_ty_Hàng_không_Việt_Nam,
        [Description("Tổng công ty Cà phê Việt Nam")]
        Tổng_công_ty_Cà_phê_Việt_Nam,
        [Description("Tổng công ty Công nghiệp tàu thủy Việt Nam")]
        Tổng_công_ty_Công_nghiệp_tàu_thủy_Việt_Nam,
        [Description("Tổng công ty truyền thông đa phương tiện VTC")]
        Tổng_công_ty_truyền_thông_đa_phương_tiện_VTC,
        [Description("Tập đoàn Dệt May Việt Nam")]
        Tập_đoàn_Dệt_May_Việt_Nam,
        [Description("Tổng công ty Đường sắt Việt Nam")]
        Tổng_công_ty_Đường_sắt_Việt_Nam,
        [Description("Tập đoàn Bưu chính - Viễn thông (VNPT)")]
        Tập_đoàn_Bưu_chính_Viễn_thông,
        [Description("Tập đoàn Công nghiệp Cao su Việt Nam")]
        Tập_đoàn_Công_nghiệp_Cao_su_Việt_Nam,
        [Description("Tổng công ty Sông Đà")]
        Tổng_công_ty_Sông_Đà,
        [Description("Tổng công ty Lương thực miền Nam")]
        Tổng_công_ty_Lương_thực_miền_Nam,
        [Description("Tổng Công ty Đầu tư và Kinh doanh vốn Nhà nước")]
        Tổng_Công_ty_Đầu_tư_và_Kinh_doanh_vốn_Nhà_nước,
        [Description("Tổng công ty xi măng Việt Nam (Vicem)")]
        Tổng_công_ty_xi_măng_Việt_Nam,
        [Description("Tập đoàn Công nghiệp Than - Khoáng sản Việt Nam")]
        Tập_đoàn_Công_nghiệp_Than_Khoáng_sản_Việt_Nam,
        [Description("Tổng công ty Thuốc lá Việt Nam")]
        Tổng_công_ty_Thuốc_lá_Việt_Nam,
        [Description("Tổng công ty Lương thực miền Bắc")]
        Tổng_công_ty_Lương_thực_miền_Bắc,
        [Description("Tổng công ty Hàng hải Việt Nam")]
        Tổng_công_ty_Hàng_hải_Việt_Nam,
        [Description("Tập đoàn Bảo Việt")]
        Tập_đoàn_Bảo_Việt,
        [Description("Tập đoàn Công nghiệp Hóa chất Việt Nam")]
        Tập_đoàn_Công_nghiệp_Hóa_chất_Việt_Nam,
        [Description("Tổng công ty Giấy Việt Nam")]
        Tổng_công_ty_Giấy_Việt_Nam,
        [Description("Tổng công ty Thép Việt Nam")]
        Tổng_công_ty_Thép_Việt_Nam,
        [Description("Tập đoàn Viễn thông quân đội Viettel")]
        Tập_đoàn_Viễn_thông_quân_đội_Viettel,
        [Description("Ngân hàng phát triển Việt Nam")]
        Ngân_hàng_phát_triển_Việt_Nam,
        [Description("Ngân hàng thương mại")]
        Ngân_hàng_thương_mại,
        [Description("Công ty tư vấn đấu thầu")]
        Công_ty_tư_vấn_đấu_thầu,
        [Description("Tập đoàn Xăng dầu Việt Nam (Petrolimex)")]
        Tập_đoàn_Xăng_dầu_Việt_Nam,
    }

    public enum Phân_loại_trực_thuộc
    {   
        [Description("Bộ ban ngành")]
        Bộ_ban_ngành,
        [Description("Tập đoàn/ TCT")]
        Tập_đoàn_TCT,
        [Description("UBND Tỉnh/ Thành phố")]
        UBND_Tỉnh_Thành_phố,
    }

    public enum Loại_hình_doanh_nghiệp
    {
        [Description("Công ty TNHH")]
        Công_ty_TNHH,
        [Description("Công ty Cổ phần")]
        Công_ty_Cổ_phần,
    }

    public enum Quốc_gia
    {
        [Description("Việt Nam")]
        Việt_Nam,
        [Description("Nước ngoài")]
        Nước_ngoài
    }
    public enum Trạng_thái_đóng_phí
    {
        [Description("Chưa nộp chi phí")]
        Chưa_nộp_chi_phí,
        [Description("Đã nộp chi phí")]
        Đã_nộp_chi_phí,
    }
}