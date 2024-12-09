using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebView.NghiaDTO
{
    public class GioHangDTO
    {
        public int Id { get; set; }
        public int SoLuong { get; set; }
        public bool TrangThai { get; set; }
        public string KhachHangTen { get; set; }
        public string SanPhamTen { get; set; }
        public string MauSacTen { get; set; }
        public string KichThuocTen { get; set; }
        public decimal Gia { get; set; }
        public decimal TongTien => SoLuong * Gia; // Tính tổng tiền của sản phẩm trong giỏ
        public string HinhAnh { get; set; } = string.Empty;
        public ChiTietSanPhamDTO ChiTietSanPhams { get; set; }
    }

}
