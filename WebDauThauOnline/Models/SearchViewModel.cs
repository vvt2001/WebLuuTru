using System.Collections.Generic;
using PagedList;
namespace WebDauThauOnline.Models
{
    public class SearchViewModel
    {
        public SearchModel searchModel { get; set; }
        public IPagedList<ThongBaoMoiThau_ThongTinChiTiet> thongBaoMoiThauModel { get; set; }
        public IPagedList<KetQuaLuaChonNhaThau_ThongTinChiTiet> ketQuaLuaChonNhaThauModel { get; set; }
 
    }
}