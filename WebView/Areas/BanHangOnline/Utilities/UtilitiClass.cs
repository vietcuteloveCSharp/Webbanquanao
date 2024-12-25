using DAL.Context;
using WebView.Areas.BanHangOnline.HoangDTO.Resp;

namespace WebView.Areas.BanHangOnline.Utilities
{
    public class UtilitiClass
    {
        private static WebBanQuanAoDbContext _context;

        public UtilitiClass()
        {
            //_context = new WebBanQuanAoDbContext();
        }

        public static List<DanhMucResp> GetDanhMucSanPham()
        {
            var list = new List<DanhMucResp>();
            _context = new WebBanQuanAoDbContext();
            list = _context.DanhMucs.OrderByDescending(x => x.Id).Take(4).ToList().Select(x => new DanhMucResp
            {
                Id = x?.Id,
                Id_DanhMucCha = x?.Id_DanhMucCha,
                NgayTao = x?.NgayTao,
                TenDanhMuc = x?.TenDanhMuc,
                TrangThai = x?.TrangThai
            }).ToList();

            return list;
        }
    }
}
