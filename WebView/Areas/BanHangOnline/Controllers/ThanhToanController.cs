using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebView.Areas.BanHangOnline.HoangDTO;
using WebView.Areas.BanHangOnline.HoangDTO.Param;
using WebView.Areas.BanHangOnline.HoangDTO.Resp;
using WebView.Models.Vnpay;
using WebView.Repository;
using WebView.Services.Vnpay;

namespace WebView.Areas.BanHangOnline.Controllers
{
    [Area("BanHangOnline")]
    public class ThanhToanController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        private readonly IVnPayService _vnPayService;
        public ThanhToanController(WebBanQuanAoDbContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }

        public async Task<IActionResult> Index()
        {
            var resp = new ThanhToanResp();
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(new { status = 401, success = false, message = "Chưa đăng nhập" });
            }
            // list giỏ hàng của khách hàng
            var lstGioHang = await _context.GioHangs.Where(x => x.Id_KhachHang == tk.Id && x.SoLuong > 0).Include(x => x.ChiTietSanPham).ToListAsync();

            if (lstGioHang == null || lstGioHang.Count <= 0)
            {
                ViewData["SpGioHang"] = new ThanhToanResp();
                return View();
            }
            // list id san pham chi tiet trong giỏ hàng
            var lstIdSpCT = lstGioHang.Select(x => x.Id_ChiTietSanPham).Distinct().ToList();
            // list san pham theo id spct có trong giỏ hàng
            var lstSp = await _context.ChiTietSanPhams.Where(x => lstIdSpCT.Contains(x.Id)).Include(x => x.SanPham).Include(x => x.MauSac).Include(x => x.KichThuoc).Select(x => x.SanPham).Distinct().ToListAsync();
            var lstIdSp = lstSp.Select(x => x.Id).Distinct().ToList();
            // list hinh anh theo sp
            var lstHinhAnh = _context.HinhAnhs.Where(x => lstIdSp.Contains((int)x.Id_SanPham)).ToList();
            // list sp có hình ảnh kèm theo
            var lstSpVoiHinhAnh = lstSp.Select(x => new
            {
                IdSP = x?.Id,
                ListHinHAnh = lstHinhAnh.Where(a => a.Id_SanPham == x.Id).Take(1).Select(a => new HinhAnhResp
                {
                    Id = a.Id,
                    Id_SanPham = x.Id,
                    ImageData = a.ImageData,
                    Url = a.Url
                }).ToList()
            }).ToList();
            // Lấy list Mau sac va kich thuoc dựa trên sp sản phẩm chi tiết
            var lstSpCTTheoIdSp = await _context.ChiTietSanPhams.Where(x => lstIdSp.Contains(x.Id_SanPham)).Include(x => x.MauSac).Include(x => x.KichThuoc).ToListAsync();
            var lstMauSac = lstSpCTTheoIdSp.Select(x => new
            {
                Id = x.MauSac.Id,
                MaHex = x.MauSac.MaHex,
                Ten = x.MauSac.Ten,
                IdSp = x.Id_SanPham
            }).Distinct().ToList(); ;
            var lstKichThuoc = lstSpCTTheoIdSp.Select(x => new
            {
                Id = x.KichThuoc.Id,
                Ten = x.KichThuoc.Ten,
                IdSp = x.Id_SanPham,
                IdMs = x.Id_MauSac
            }).Distinct().ToList();
            // Lọc sản phẩm trong giỏ hàng có khuyến mại
            var timenow = DateTime.Now;
            var lstIdDm = lstSp.Select(x => x.Id_DanhMuc).Distinct().ToList();
            var lstKhuyenMai = await _context.KhuyenMais.Where(x => x.TrangThai == 1)
                .Where(x => x.NgayBatDau <= timenow && timenow <= x.NgayKetThuc)
                .Include(x => x.chiTietKhuyenMais)
                .Where(x => x.chiTietKhuyenMais.Any(a => lstIdDm.Contains((int)a.Id_DanhMuc))).ToListAsync();
            var lstSpNew = new List<SanPhamResp>();
            foreach (var item in lstSp)
            {
                var khuyenMai = lstKhuyenMai.FirstOrDefault(x => x.chiTietKhuyenMais.Any(a => a.Id_DanhMuc == item.Id_DanhMuc));
                var giaBan = khuyenMai != null && item.Gia >= khuyenMai.DieuKienGiamGia ? Math.Round(item.Gia - (item.Gia * khuyenMai.GiaTriGiam / 100)) : Math.Round(item.Gia);
                lstSpNew.Add(new SanPhamResp
                {
                    Id = item.Id,
                    GiaBan = giaBan,
                    GiaBanDau = Math.Round(item.Gia),
                    MoTa = item.MoTa,
                    SoLuong = item.SoLuong,
                    Ten = item.Ten,
                    ListHinHAnh = lstSpVoiHinhAnh.FirstOrDefault(b => b.IdSP == item.Id).ListHinHAnh,
                    Id_DanhMuc = item.Id_DanhMuc
                });
            }
            // Tổng hợp lại toàn bộ dựa trên list giỏ hàng

            resp = new ThanhToanResp
            {
                GioHang = lstGioHang.Select(x => new GioHangResp
                {
                    Id = x?.Id,
                    SoLuong = x?.SoLuong,
                    IdChiTietSanPham = x?.Id_ChiTietSanPham,
                    IdKichThuoc = x?.ChiTietSanPham?.Id_KichThuoc,
                    IdMauSac = x?.ChiTietSanPham?.Id_MauSac,
                    KichThuocResps = lstKichThuoc.Where(a => a.IdSp == x.ChiTietSanPham.Id_SanPham && a.Id == x.ChiTietSanPham?.Id_KichThuoc).Select(a => new KichThuocResp
                    {
                        Id = a?.Id,
                        Ten = a?.Ten,
                        Id_MauSac = x?.ChiTietSanPham?.Id_MauSac
                    }).Distinct().ToList(),
                    MauSacResps = lstMauSac.Where(a => a.IdSp == x.ChiTietSanPham.Id_SanPham && a.Id == x.ChiTietSanPham?.Id_MauSac).Select(x => new MauSacResp
                    {
                        Id = x.Id,
                        MaHex = x.MaHex,
                        Ten = x.Ten
                    }).Distinct().ToList(),
                    SanPham = lstSpNew.FirstOrDefault(a => a.Id == x.ChiTietSanPham.Id_SanPham)
                }).ToList(),
                KhachHangModel = new KhachHangResp
                {
                    Id = tk?.Id,
                    Avatar = tk?.Avatar,
                    Email = tk?.Email,
                    Sdt = tk?.Sdt,
                    Ten = tk?.Ten,
                    TrangThai = tk.TrangThai,
                }
            };
            ViewData["SpThanhToan"] = resp;
            HttpContext.Session.Remove("SpThanhToan");
            HttpContext.Session.SetString("SpThanhToan", JsonConvert.SerializeObject(resp));
            return View();
        }
        // api: nhận thông tin thanh toán
        //  request: 
        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrlVnpay(ThanhToanParam req)
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                ViewData["message"] = "Thanh toán thất bại";
                return View("ThanhToanThatBai");
            }
            if (_context.KhachHangs.Any(x => x.Id == tk.Id && x.TrangThai == false))
            {
                ViewData["message"] = "Tài khoản khách hàng bị khóa. Hiện không thể thực hiện thanh toán";
                return View("ThanhToanThatBai");
            }
            if (string.IsNullOrEmpty(req.DiaChiGiaoHang))
            {
                ViewData["message"] = "Thanh toán thất bại";
                return View("ThanhToanThatBai");
            }
            // Xác định số lượng trong giỏ hàng <= so lượng trong sản phẩm chi tiết
            var lstGioHang = await _context.GioHangs.Include(x => x.ChiTietSanPham).ThenInclude(a => a.SanPham)
                .Where(x => x.ChiTietSanPham != null && x.ChiTietSanPham.SanPham != null)
                .Where(x => x.ChiTietSanPham.SoLuong > 0)
                .Where(x => x.SoLuong > 0 && x.SoLuong <= x.ChiTietSanPham.SoLuong)
                .Where(x => x.Id_KhachHang == tk.Id).ToListAsync();

            if (lstGioHang == null || lstGioHang.Count <= 0)
            {
                ViewData["message"] = "Thanh toán thất bại";
                return View("ThanhToanThatBai");
            }
            // check khuyến mại cho sản phẩm or danh mục sản phẩm
            var lstIdDm = lstGioHang.Select(x => x.ChiTietSanPham.SanPham.Id_DanhMuc).Distinct().ToList();
            var timenow = DateTime.Now;
            var lstKhuyenMai = await _context.ChiTietKhuyenMais.Where(x => lstIdDm.Contains((int)x.Id_DanhMuc))
                .Include(x => x.KhuyenMai)
                .Where(x => x.KhuyenMai.TrangThai == 1)
                .Where(x => x.KhuyenMai.NgayBatDau <= timenow && timenow <= x.KhuyenMai.NgayKetThuc)
                .ToListAsync();

            // lấy tổng tiền hóa đơn
            decimal tongTienHoaDon = 0;
            foreach (var item in lstGioHang)
            {
                var khuyenmai = lstKhuyenMai.FirstOrDefault(x => x.Id_DanhMuc == item.ChiTietSanPham.SanPham.Id_DanhMuc)?.KhuyenMai;
                var giaban = khuyenmai != null && item.ChiTietSanPham.SanPham.Gia >= khuyenmai.DieuKienGiamGia ? Math.Round(item.ChiTietSanPham.SanPham.Gia - (item.ChiTietSanPham.SanPham.Gia * khuyenmai.GiaTriGiam / 100)) : Math.Round(item.ChiTietSanPham.SanPham.Gia);
                tongTienHoaDon += item.SoLuong * giaban;

            }
            // check mã giảm giá mà kh đã thêm

            // kiếm tra khách hàng đã sử dụng mã giảm giá chưa
            if (_context.ChiTietMaGiamGias.Any(x => x.Id_MaGiamGia == req.IdMaGiamGia && x.Id_KhachHang == tk.Id))
            {
                ViewData["message"] = "Mã giảm giá không hợp lệ";
                return View("ThanhToanThatBai");
            }

            decimal tongTienGiam = 0;

            // kiểm tra mã giảm giá có hợp lệ
            DateTime timeNow = DateTime.Now;
            var giamGia = await _context.MaGiamGias.Where(x => x.Id == req.IdMaGiamGia).Where(x => x.TrangThai == 1 && x.SoLuong >= 1).Where(x => x.ThoiGianTao.CompareTo(timeNow) <= 0).Where(x => !(x.ThoiGianKetThuc != null) || timeNow <= x.ThoiGianKetThuc).FirstOrDefaultAsync();
            if (giamGia != null)
            {


                // kiểm tra đến: đk giá trị đơn hàng, giá trị tối đa
                // coupon : %
                if (giamGia.LoaiGiamGia == 0)
                {
                    if (tongTienHoaDon < giamGia.DieuKienGiamGia)
                    {
                        ViewData["message"] = "Chưa đủ điều kiện sử dụng hóa đơn";
                        return View("ThanhToanThatBai");
                    }
                    tongTienGiam = (decimal)giamGia.GiaTriGiam * tongTienHoaDon;
                    if (tongTienGiam > giamGia.GiaTriToiDa)
                    {
                        tongTienGiam = (decimal)giamGia.GiaTriToiDa;
                    }
                }
                else
                // voucher : tien
                if (giamGia.LoaiGiamGia == 1)
                {
                    if (tongTienHoaDon < giamGia.DieuKienGiamGia)
                    {
                        ViewData["message"] = "Chưa đủ điều kiện sử dụng hóa đơn";
                        return View("ThanhToanThatBai");
                    }
                    tongTienGiam = giamGia.MenhGia;

                }
                // tổng tiền hóa đơn - tổng tiền được giảm với mã giảm giá
                tongTienHoaDon = Math.Round(tongTienHoaDon) - Math.Round(tongTienGiam);
            }
            // tổng tiền hóa đơn - tiền phí vận chuyển
            tongTienHoaDon = Math.Round(tongTienHoaDon + req.PhiVanChuyen);
            // tạo mới hóa đơn vào db nhưng chưa thực hiện lưu vào db
            if (tongTienHoaDon <= 0)
            {
                tongTienHoaDon = Math.Round(req.PhiVanChuyen);
            }
            var hoaDonDb = _context.HoaDons.Add(new HoaDon
            {
                Id_KhachHang = tk.Id,
                TongTien = tongTienHoaDon,
                TrangThai = req.PhuongThucThanhToan.Trim().ToLower() == "vnpay" ? Enum.EnumVVA.ETrangThaiHD.ChoThanhToan : Enum.EnumVVA.ETrangThaiHD.ChoXacNhan,
                NgayTao = DateTime.Now,
                PhiVanChuyen = req.PhiVanChuyen,
                DiaChiGiaoHang = req.DiaChiGiaoHang,

            }).Entity;
            _context.SaveChanges();
            // khi phương thức thanh toán là cod
            if (req.PhuongThucThanhToan.Trim().ToLower() == "cod")
            {
                //thành công:
                //trừ số lượng sản phẩm chi tiết theo số lượng giỏ hàng
                foreach (var item in lstGioHang)
                {
                    if (item.ChiTietSanPham != null && item.ChiTietSanPham.SanPham != null)
                    {
                        item.ChiTietSanPham.SanPham.SoLuong = item.ChiTietSanPham.SanPham.SoLuong - item.SoLuong;
                        item.ChiTietSanPham.SoLuong = item.ChiTietSanPham.SoLuong - item.SoLuong;
                    }
                    else
                    {
                        ViewData["message"] = "Lỗi số lượng sản phẩm";
                        return View("ThanhToanThatBai");
                    }

                }
                _context.SaveChanges();

                // thêm chi tiết hóa đơn
                foreach (var spctgh in lstGioHang)
                {
                    var chiTietHD = _context.ChiTietHoaDons.Add(new ChiTietHoaDon
                    {
                        Id_HoaDon = hoaDonDb.Id,
                        SoLuong = spctgh.SoLuong,
                        Id_ChiTietSanPham = spctgh.Id_ChiTietSanPham,
                        Gia = Math.Round(spctgh.ChiTietSanPham.SanPham.Gia), // giá này chưa được giảm khi có đợt khuyến mại
                        TrangThai = true
                    });
                }
                _context.SaveChanges();

                if (giamGia != null)
                {
                    giamGia.SoLuong -= 1;
                    giamGia.SoLuongDaSuDung += 1;
                    if (giamGia.SoLuong == 0)
                    {
                        giamGia.TrangThai = 2;
                    }

                    // thêm chi tiết mã giảm giá
                    _context.ChiTietMaGiamGias.Add(new ChiTietMaGiamGia
                    {
                        Id_HoaDon = hoaDonDb.Id,
                        Id_KhachHang = tk.Id,
                        Id_MaGiamGia = giamGia?.Id ?? 0,
                        TongTien = tongTienGiam,
                        NgaySuDung = DateTime.Now,
                        NoiDung = $"{tk.Ten} đã sử dụng mã giảm giá {giamGia.Id} cho đơn hàng {hoaDonDb.Id} vào {DateTime.Now}, tổng số tiền giảm {tongTienGiam}"
                    });
                }
                // xóa toàn bộ sản phẩm đã mua có trong giỏ hàng khách hàng
                _context.RemoveRange(lstGioHang);
                //lưu thông tin thanh toán
                var ptThanhToan = new PhuongThucThanhToan();
                if (!_context.PhuongThucThanhToans.Any(x => x.Ten.ToLower().Trim() == "cod"))
                {
                    ptThanhToan = _context.PhuongThucThanhToans.Add(new PhuongThucThanhToan
                    {
                        Mota = $"Phương thức thanh toán cod",
                        Ten = "cod",
                        NgayTao = DateTime.Now,
                        TrangThai = true
                    }).Entity;

                    _context.SaveChanges();
                }
                else
                {
                    ptThanhToan = _context.PhuongThucThanhToans.FirstOrDefault(x => x.Ten.ToLower().Trim() == "cod");
                }

                _context.ThanhToanHoaDons.Add(new ThanhToanHoaDon
                {
                    Id_HoaDon = hoaDonDb.Id,
                    Id_PhuongThucThanhToan = ptThanhToan.Id,
                    TongTien = hoaDonDb.TongTien,
                    NgayThanhToan = DateTime.Now,
                    SoTienDaThanhToan = hoaDonDb.TongTien,
                    MaGiaoDich = "",
                });
                _context.SaveChanges();

                return View("ThanhToanThanhCong", null);
            }
            else
            {
                // khi phương thức thanh toán là vnpay
                var requestVnPay = new PaymentInformationModel
                {
                    Name = tk.Ten,
                    Amount = (double)Math.Round(tongTienHoaDon),
                    OrderType = "other",
                    OrderDescription = $"Thanh toán vnpay cho đơn của khách hàng {tk.Ten}, tổng tiền hàng: {(double)Math.Round(tongTienHoaDon)} tại CANMAN shop",
                    IdHoaDon = hoaDonDb.Id,
                    PhuongThucThanhToan = req.PhuongThucThanhToan,
                };

                // Xử lý logic tạo URL thanh toán VNPAY
                var url = _vnPayService.CreatePaymentUrl(requestVnPay, HttpContext);

                if (!string.IsNullOrEmpty(url))
                {
                    // Lưu thông tin: DonHang, ChiTietHoaDon, ChiTietMaGiamGia, VnpayInfo,

                    // thêm  chi tiết hóa đơn
                    foreach (var spctgh in lstGioHang)
                    {
                        var chiTietHD = _context.ChiTietHoaDons.Add(new ChiTietHoaDon
                        {
                            Id_HoaDon = hoaDonDb.Id,
                            SoLuong = spctgh.SoLuong,
                            Id_ChiTietSanPham = spctgh.Id_ChiTietSanPham,
                            Gia = spctgh.ChiTietSanPham.SanPham.Gia, // giá này chưa được giảm khi có đợt khuyến mại
                            TrangThai = true
                        });
                    }
                    if (giamGia != null)
                    {
                        giamGia.SoLuong -= 1;
                        giamGia.SoLuongDaSuDung += 1;
                        if (giamGia.SoLuong == 0)
                        {
                            giamGia.TrangThai = 2;
                        }


                        // thêm chi tiết mã giảm giá
                        _context.ChiTietMaGiamGias.Add(new ChiTietMaGiamGia
                        {
                            Id_HoaDon = hoaDonDb.Id,
                            Id_KhachHang = tk.Id,
                            Id_MaGiamGia = giamGia?.Id ?? 0,
                            TongTien = tongTienGiam,
                            NgaySuDung = DateTime.Now,
                            NoiDung = $"{tk.Ten} đã sử dụng mã giảm giá {giamGia.Id} cho đơn hàng {hoaDonDb.Id} vào {DateTime.Now}, tổng số tiền giảm {tongTienGiam}"
                        });
                    }
                    // xóa toàn bộ sản phẩm đã mua có trong giỏ hàng khách hàng
                    _context.RemoveRange(lstGioHang);

                    _context.SaveChanges();
                    // Chuyển hướng người dùng đến URL thanh toán
                    return Redirect(url);
                }
                else
                {
                    // Trường hợp URL không được tạo thành công
                    ViewData["message"] = "không thể tạo url vnpay";
                    return View("ThanhToanThatBai");
                }
            }
        }



        // phương thực callback, nhận respone của vnpay. khi thực hiện thanh toán
        [HttpGet]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(new { status = 401, success = false, message = "Chưa đăng nhập" });
            }
            var response = _vnPayService.PaymentExecute(Request.Query);
            // lấy được respone
            // lấy session
            var lstSessionThanhToan = HttpContext.Session.GetObjectFromJson<List<SessionThanhToanModel>>("SessionThanhToan");
            var sessionHoaDon = lstSessionThanhToan.FirstOrDefault(x => x.VnpTxnRef.Equals(response.VnpTxnRef));
            if (sessionHoaDon == null)
            {
                ViewData["message"] = "Thanh toán thất bại";
                return View("ThanhToanThatBai");
            }

            // Thay đổi số lượng của sản phẩm chi tiết
            var hoaDon = await _context.HoaDons.Where(x => x.Id == sessionHoaDon.IdHoaDon && x.TrangThai == Enum.EnumVVA.ETrangThaiHD.ChoThanhToan).Include(x => x.ChiTietHoaDons).FirstOrDefaultAsync();
            if (hoaDon == null)
            {
                ViewData["message"] = "Lỗi hệ thống";
                return View("ThanhToanThatBai");
            }
            // xác nhận thành công thanh toán
            if (response.VnPayResponseCode == "00")
            {
                // thay đổi trạng thái của hóa đơn
                hoaDon.TrangThai = Enum.EnumVVA.ETrangThaiHD.ChoThanhToan;
                _context.SaveChanges();
                // thêm ThanhToanHoaDons
                var ptThanhToan = new PhuongThucThanhToan();
                if (!_context.PhuongThucThanhToans.Any(x => x.Ten.ToLower().Trim() == sessionHoaDon.PhuongThucThanhToan.ToLower().Trim()))
                {
                    ptThanhToan = _context.PhuongThucThanhToans.Add(new PhuongThucThanhToan
                    {
                        Mota = $"Phương thức thanh toán {sessionHoaDon.PhuongThucThanhToan.Trim().ToLower()}",
                        Ten = sessionHoaDon.PhuongThucThanhToan.Trim().ToLower(),
                        NgayTao = DateTime.Now,
                        TrangThai = true
                    }).Entity;

                    _context.SaveChanges();
                }
                else
                {
                    ptThanhToan = _context.PhuongThucThanhToans.FirstOrDefault(x => x.Ten.ToLower().Trim() == sessionHoaDon.PhuongThucThanhToan.ToLower().Trim());
                }

                _context.ThanhToanHoaDons.Add(new ThanhToanHoaDon
                {
                    Id_HoaDon = hoaDon.Id,
                    Id_PhuongThucThanhToan = ptThanhToan.Id,
                    TongTien = hoaDon.TongTien,
                    NgayThanhToan = DateTime.Now,
                    SoTienDaThanhToan = hoaDon.TongTien,
                    MaGiaoDich = response.TransactionId,
                });
                // Sửa : số lượng trong ChiTietSanPham,
                var lstIdCTSP = hoaDon.ChiTietHoaDons.Select(x => x.Id_ChiTietSanPham);
                var lstIdspctAndSoLuong = hoaDon.ChiTietHoaDons.Select(x => new
                {
                    IdSpCt = x.Id_ChiTietSanPham,
                    SoLuong = x.SoLuong,
                }).ToList();
                var lstSpChiTiet = await _context.ChiTietSanPhams.Where(x => lstIdCTSP.Contains(x.Id)).Include(x => x.SanPham).ToListAsync();
                foreach (var item in lstSpChiTiet)
                {
                    if (lstIdspctAndSoLuong.Any(x => x.IdSpCt == item.Id))
                    {
                        var soLuongDaBan = lstIdspctAndSoLuong.First(x => x.IdSpCt == item.Id).SoLuong;
                        item.SoLuong = item.SoLuong - soLuongDaBan;
                        if (item.SoLuong < 0)
                        {
                            item.SoLuong = 0;
                            item.TrangThai = false;
                        }
                        item.SanPham.SoLuong = item.SanPham.SoLuong - soLuongDaBan;
                        _context.SaveChanges();
                    }
                }

                lstSessionThanhToan.Remove(sessionHoaDon);
                HttpContext.Session.Remove("SessionThanhToan");
                HttpContext.Session.SetString("SessionThanhToan", JsonConvert.SerializeObject(lstSessionThanhToan));
            }
            else
            {
                hoaDon.TrangThai = Enum.EnumVVA.ETrangThaiHD.HuyDon;
                _context.SaveChanges();
                string mess = "";
                switch (response.VnPayResponseCode)
                {
                    case "07":
                        mess = "Trừ tiền thành công. Giao dịch bị nghi ngờ (liên quan tới lừa đảo, giao dịch bất thường).";
                        break;
                    case "09":
                        mess = "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking tại ngân hàng.";
                        break;
                    case "10":
                        mess = "Giao dịch không thành công do: Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần";
                        break;
                    case "12":
                        mess = "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng bị khóa.";
                        break;
                    case "13":
                        mess = "Giao dịch không thành công do Quý khách nhập sai mật khẩu xác thực giao dịch (OTP). Xin quý khách vui lòng thực hiện lại giao dịch.";
                        break;
                    case "24":
                        mess = "Giao dịch không thành công do: Khách hàng hủy giao dịch";
                        break;
                    case "51":
                        mess = "Giao dịch không thành công do: Tài khoản của quý khách không đủ số dư để thực hiện giao dịch.";
                        break;
                    case "65":
                        mess = "Giao dịch không thành công do: Tài khoản của Quý khách đã vượt quá hạn mức giao dịch trong ngày.";
                        break;
                    case "99":
                        mess = "Lỗi hệ thống thanh toán vnpay";
                        break;
                    case "11":
                        mess = "Giao dịch không thành công do: Đã hết hạn chờ thanh toán.";
                        break;
                    case "75":
                        mess = "Ngân hàng thanh toán đang bảo trì.";
                        break;
                    case "79":
                        mess = "Giao dịch không thành công do: KH nhập sai mật khẩu thanh toán quá số lần quy định.";
                        break;
                }
                ViewData["message"] = mess;
                return View("ThanhToanThatBai");

            }

            return View("ThanhToanThanhCong", response);
        }

        [HttpGet]
        public async Task<IActionResult> ThucHienThanhToanLai(string mess)
        {
            ViewData["message"] = mess;
            return View("ThucHienThanhToanLai");
        }


        // api cho phép lấy danh sách phiếu giảm giá
        // dk: khách hàng đăng nhập, tìm kiếm các phiếu giảm giá đủ điều kiện
        [HttpGet]
        public async Task<IActionResult> GetDanhSachPhieuGiamGia(string tenPhieu = "")
        {
            var tk = HttpContext.Session.GetObjectFromJson<KhachHang>("TaiKhoan");
            if (tk == null)
            {
                return Json(new { status = 401, success = false, message = "Chưa đăng nhập" });
            }
            // lấy danh sách giỏ hàng của khách hàng
            var lstGioHang = await _context.GioHangs.Where(x => x.Id_KhachHang == tk.Id).ToListAsync();
            if (lstGioHang == null || lstGioHang.Count <= 0)
            {
                return Json(new { status = 400, data = "", message = "Thất bại" });
            }
            // lấy danh sách phiếu giảm giá
            DateTime timeNow = DateTime.Now;
            var lstPhieuGiamGia = await _context.MaGiamGias.Where(x => string.IsNullOrEmpty(tenPhieu) || x.Ten.ToLower().Contains(tenPhieu.ToLower()))
                .Where(x => !string.IsNullOrEmpty(x.Ten)).Where(x => x.TrangThai == 1 && x.SoLuong >= 1)
                .Where(x => x.ThoiGianTao.CompareTo(timeNow) <= 0)
                .Where(x => !(x.ThoiGianKetThuc != null) || timeNow <= x.ThoiGianKetThuc).ToListAsync();

            if (lstPhieuGiamGia == null || lstPhieuGiamGia.Count <= 0)
            {
                return Json(new { status = 200, data = "", message = "Thành công" });
            }
            // kiểm tra tổng hóa đơn > đk giảm giá hóa đơn
            //var lstIdSpCTTrongGioHang = lstGioHang.Select(x => x.Id_ChiTietSanPham).ToList();
            //var lstSpCT = await _context.ChiTietSanPhams.Where(x => lstIdSpCTTrongGioHang.Contains(x.Id)).Include(x => x.SanPham).ToListAsync();
            //decimal tongTienHang = 0;
            //foreach (var gh in lstGioHang)
            //{
            //    tongTienHang += gh.SoLuong * (lstSpCT.Where(x => x.Id == gh.Id_ChiTietSanPham).FirstOrDefault().SanPham.Gia);
            //}
            // Lọc xem tổng tiền hàng đủ đk hóa đơn -> tìm các phiếu giảm giá
            //lstPhieuGiamGia = lstPhieuGiamGia.Where(x => x.DieuKienGiamGia <= tongTienHang).ToList();
            // kiểm tra khách hàng đã sử dụng hóa đơn -> đã sử dụng thì bỏ hóa đơn này ra
            var lstIdPhieuGiamGia = lstPhieuGiamGia.Select(x => x.Id).ToList();
            // xem khách hàng đã sử dụng phiếu giảm giá này chưa
            var isPhieuGiamGiaDaSuDung = await _context.ChiTietMaGiamGias.Where(x => lstIdPhieuGiamGia.Contains((int)x.Id_MaGiamGia) && x.Id_KhachHang == tk.Id).Select(x => (int)x.Id_MaGiamGia).ToListAsync();
            if (isPhieuGiamGiaDaSuDung != null && isPhieuGiamGiaDaSuDung.Count > 0)
            {
                lstPhieuGiamGia = lstPhieuGiamGia.Where(x => !isPhieuGiamGiaDaSuDung.Contains(x.Id))
                    .Select(x => new MaGiamGia
                    {
                        Id = x.Id,
                        DieuKienGiamGia = Math.Round(x.DieuKienGiamGia),
                        GiaTriGiam = Math.Round((decimal)x.GiaTriGiam),
                        GiaTriToiDa = Math.Round((decimal)x.GiaTriToiDa),
                        MenhGia = Math.Round(x.MenhGia),
                        LoaiGiamGia = x.LoaiGiamGia,
                        NoiDung = x.NoiDung,
                        SoLuong = x.SoLuong,
                        SoLuongDaSuDung = x.SoLuongDaSuDung,
                        Ten = x.Ten,
                        ThoiGianKetThuc = x.ThoiGianKetThuc,
                        ThoiGianTao = x.ThoiGianTao,
                        TrangThai = x.TrangThai,
                    }).ToList();
            }

            return Json(new { status = 200, data = lstPhieuGiamGia, message = "Thành công" });
        }

    }
}
