using DAL.Context;
using DAL.Entities;
using DAL.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebView.NghiaDTO;
using WebView.Repository;

namespace WebView.Controllers
{
    public class GioHangController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;

        public GioHangController(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            var khachHangId = HttpContext.Session.GetInt32("KhachHangId");

            if (khachHangId == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }

            // Lấy giỏ hàng từ session
            var cart = HttpContext.Session.GetObjectFromJson<List<GioHangDTO>>("GioHang") ?? new List<GioHangDTO>();

            if (!cart.Any())
            {
                TempData["WarningMessage"] = "Giỏ hàng của bạn đang trống.";
                return View(new List<GioHangDTO>());
            }

            // Cập nhật thông tin từ database nếu cần (tránh dữ liệu cũ từ session)
            foreach (var item in cart)
            {
                var chiTietSanPham = _context.ChiTietSanPhams
                    .Include(ct => ct.SanPham)
                    .Include(ct => ct.MauSac)
                    .Include(ct => ct.KichThuoc)
                    .FirstOrDefault(ct => ct.Id == item.ChiTietSanPhams.Id);

                if (chiTietSanPham != null)
                {
                    item.SanPhamTen = chiTietSanPham.SanPham.Ten;
                    item.MauSacTen = chiTietSanPham.MauSac.Ten;
                    item.KichThuocTen = chiTietSanPham.KichThuoc.Ten;
                    item.Gia = chiTietSanPham.SanPham.Gia;
                }
            }
            TempData["SuccessMessage"] = "Số lượng sản phẩm đã được cập nhật.";

            ViewData["TongCong"] = cart.Sum(x => x.TongTien);
            return View(cart);
        }
        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public IActionResult AddToCart(int productId, int colorId, int sizeId, int soLuong)
        {
            Console.WriteLine($"Received: Product ID={productId}, Color ID={colorId}, Size ID={sizeId}, Quantity={soLuong}");

            var chiTietSanPham = _context.ChiTietSanPhams
                .Include(ct => ct.MauSac)
                .Include(ct => ct.KichThuoc)
                .FirstOrDefault(ct => ct.Id_SanPham == productId && ct.Id_MauSac == colorId && ct.Id_KichThuoc == sizeId);

            if (chiTietSanPham == null)
            {
                if (!_context.SanPhams.Any(sp => sp.Id == productId))
                    Console.WriteLine("Sản phẩm không tồn tại.");
                if (!_context.MauSacs.Any(ms => ms.Id == colorId))
                    Console.WriteLine("Màu sắc không tồn tại.");
                if (!_context.KichThuocs.Any(kt => kt.Id == sizeId))
                    Console.WriteLine("Kích thước không tồn tại.");

                return Json(new { success = false, message = "Sản phẩm không tồn tại hoặc không đủ thông tin." });
            }

            // Tạo đối tượng ChiTietSanPhamDTO
            var ctspa = new ChiTietSanPhamDTO
            {
                Id = chiTietSanPham.Id,
                Id_KichThuoc = chiTietSanPham.Id_KichThuoc,
                Id_MauSac = chiTietSanPham.Id_MauSac,
                Id_SanPham = chiTietSanPham.Id_SanPham,
                NgayTao = chiTietSanPham.NgayTao,
                SoLuong = chiTietSanPham.SoLuong,
                TrangThai = chiTietSanPham.TrangThai
            };

            // Lấy giỏ hàng từ session (nếu có)
            var cart = HttpContext.Session.GetObjectFromJson<List<GioHangDTO>>("GioHang") ?? new List<GioHangDTO>();

            // Kiểm tra nếu sản phẩm đã có trong giỏ hàng
            var existingItem = cart.FirstOrDefault(x => x.ChiTietSanPhams.Id_SanPham == productId &&
                                                         x.ChiTietSanPhams.Id_MauSac == colorId &&
                                                         x.ChiTietSanPhams.Id_KichThuoc == sizeId);

            if (existingItem != null)
            {
                // Nếu sản phẩm đã có, cộng thêm số lượng
                existingItem.SoLuong += soLuong;
            }
            else
            {
                // Nếu sản phẩm chưa có, thêm sản phẩm mới vào giỏ hàng
                cart.Add(new GioHangDTO { ChiTietSanPhams = ctspa, SoLuong = soLuong });
            }

            // Lưu lại giỏ hàng vào session
            HttpContext.Session.SetObjectAsJson("GioHang", cart);

            return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng!" });
        }




        // Xóa sản phẩm khỏi giỏ hàng
        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<GioHangDTO>>("GioHang") ?? new List<GioHangDTO>();
            var itemToRemove = cart.FirstOrDefault(i => i.Id == id);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson("GioHang", cart);
            }

            return RedirectToAction("Index");
        }


        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int colorId, int sizeId, int soLuong)
        {
            // Lấy giỏ hàng từ session
            var gioHang = HttpContext.Session.GetObjectFromJson<List<GioHangDTO>>("GioHang");

            if (gioHang == null)
            {
                return RedirectToAction("Index");
            }

            // Tìm sản phẩm trong giỏ hàng dựa trên productId, sizeId và colorId
            var item = gioHang.FirstOrDefault(g => g.ChiTietSanPhams.Id_SanPham == productId &&
                                                    g.ChiTietSanPhams.Id_MauSac == colorId &&
                                                    g.ChiTietSanPhams.Id_KichThuoc == sizeId);

            if (item != null)
            {
                // Cập nhật số lượng của sản phẩm tìm thấy
                item.SoLuong = soLuong;
            }
            else
            {
                // Nếu không tìm thấy sản phẩm trong giỏ hàng
                TempData["WarningMessage"] = "Sản phẩm không tồn tại trong giỏ hàng.";
                return RedirectToAction("Index");
            }

            // Lưu lại giỏ hàng sau khi cập nhật
            HttpContext.Session.SetObjectAsJson("GioHang", gioHang);

            // Thông báo thành công
            TempData["SuccessMessage"] = "Số lượng sản phẩm đã được cập nhật.";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult CompleteOrder(string FullName, string Address, string Phone, string PaymentMethod)
        {
            var khachHangId = HttpContext.Session.GetInt32("KhachHangId");

            if (khachHangId == null)
            {
                return RedirectToAction("Login", "TaiKhoan");
            }

            var gioHangItems = _context.GioHangs
                .Include(g => g.ChiTietSanPham)
                .ThenInclude(sp => sp.SanPham)
                .Where(g => g.Id_KhachHang == khachHangId && g.TrangThai)
                .ToList();

            if (!gioHangItems.Any())
            {
                return RedirectToAction("Index", "GioHang");
            }

            // Tính tổng tiền
            decimal tongTien = gioHangItems.Sum(item => item.SoLuong * item.ChiTietSanPham.SanPham.Gia);

            // Tạo đối tượng hóa đơn mới và lưu vào cơ sở dữ liệu
            var hoaDon = new HoaDon
            {
                Id_KhachHang = khachHangId.Value,
                TongTien = tongTien.ToString("#,##0 VNĐ"),
                NgayTao = DateTime.Now,
                TrangThai = Enum.EnumVVA.ETrangThaiHD.ChoXuLy, // Đơn hàng mới đặt, trạng thái là 1 (Chờ xử lý)
                Id_NhanVien = 1,
            };

            _context.HoaDons.Add(hoaDon);
            _context.SaveChanges();

            // Sau khi lưu hóa đơn, lưu chi tiết hóa đơn
            foreach (var item in gioHangItems)
            {
                var chiTietHoaDon = new ChiTietHoaDon
                {
                    Id_HoaDon = hoaDon.Id,
                    Id_ChiTietSanPham = item.Id_ChiTietSanPham,
                    SoLuong = item.SoLuong,
                    Gia = item.ChiTietSanPham.SanPham.Gia.ToString("0.##") // hoặc ToString("#,##0.##") để định dạng số
                };

                _context.ChiTietHoaDons.Add(chiTietHoaDon);
            }

            _context.SaveChanges();

            // Xóa giỏ hàng sau khi đặt hàng thành công
            foreach (var item in gioHangItems)
            {
                _context.GioHangs.Remove(item);
            }

            _context.SaveChanges();

            return RedirectToAction("XacNhanThanhToan");
        }

        public IActionResult XacNhanThanhToan()
        {
            return View();
        }


    }
}
