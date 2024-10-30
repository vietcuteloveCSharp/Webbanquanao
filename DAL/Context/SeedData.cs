using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class SeedData
    {
        public static void SeedingData(WebBanQuanAoDbContext _context)
        {
            // Thực hiện migration nếu cần thiết
            _context.Database.Migrate();

            // Kiểm tra nếu chưa có dữ liệu trong bảng
            if (!_context.SanPhams.Any())
            {
                // Tạo dữ liệu mẫu cho bảng DanhMuc
                var danhMucAo = new DanhMuc { TenDanhMuc = "Áo", NgayTao = new DateTime(2004, 12, 10), TrangThai = true };
                var danhMucQuan = new DanhMuc { TenDanhMuc = "Quần", NgayTao = new DateTime(2005, 5, 15), TrangThai = true };

                // Thêm dữ liệu mẫu cho bảng ThuongHieu
                var thuongHieuNike = new ThuongHieu { Ten = "Nike", MoTa = "Thương hiệu Nike nổi tiếng", TrangThai = true };
                var thuongHieuAdidas = new ThuongHieu { Ten = "Adidas", MoTa = "Thương hiệu Adidas nổi tiếng", TrangThai = true };

                // Thêm danh mục và thương hiệu vào DbContext và lưu trước
                _context.DanhMucs.AddRange(danhMucAo, danhMucQuan);
                _context.ThuongHieus.AddRange(thuongHieuNike, thuongHieuAdidas);
                _context.SaveChanges(); // Lưu để đảm bảo các danh mục và thương hiệu có Id hợp lệ

                // Lấy Id của các danh mục và thương hiệu vừa lưu
                var aoId = danhMucAo.Id;
                var quanId = danhMucQuan.Id;
                var nikeId = thuongHieuNike.Id;
                var adidasId = thuongHieuAdidas.Id;

                // Thêm dữ liệu mẫu cho bảng SanPham
                var sanPham1 = new SanPham
                {
                    Ten = "Áo Nike thể thao",
                    MoTa = "Áo thể thao cho nam",
                    Gia = 50000,
                    SoLuong = 50,
                    NgayTao = DateTime.Now,
                    NgayCapNhat = DateTime.Now,
                    TrangThai = true,
                    HinhAnh = "nike-shirt.jpg",
                    Id_ThuongHieu = nikeId, // Gán Id đã lưu của thương hiệu Nike
                    Id_DanhMuc = aoId // Gán Id đã lưu của danh mục Áo
                };

                var sanPham2 = new SanPham
                {
                    Ten = "Quần Adidas Jogger",
                    MoTa = "Quần jogger thể thao",
                    Gia = 50000,
                    SoLuong = 30,
                    NgayTao = DateTime.Now,
                    NgayCapNhat = DateTime.Now,
                    TrangThai = true,
                    HinhAnh = "adidas-jogger.jpg",
                    Id_ThuongHieu = adidasId, // Gán Id đã lưu của thương hiệu Adidas
                    Id_DanhMuc = quanId // Gán Id đã lưu của danh mục Quần
                };

                // Thêm sản phẩm vào DbContext và lưu thay đổi
                _context.SanPhams.AddRange(sanPham1, sanPham2);
                _context.SaveChanges(); // Lưu tất cả thay đổi vào cơ sở dữ liệu
            }
        }
    }
}

