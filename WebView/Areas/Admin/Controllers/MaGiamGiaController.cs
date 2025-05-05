using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class MaGiamGiaController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public MaGiamGiaController(WebBanQuanAoDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _context = dbcontext;
        }
        // GET: Admin/MaGiamGia
        // GET: Admin/MaGiamGia
        public async Task<IActionResult> Index()
        {
            var maGiamGiaList = await _context.MaGiamGias
                .Select(m => new MaGiamGiaDTO
                {
                    Id = m.Id,
                    Ten = m.Ten,
                    LoaiGiamGia = m.LoaiGiamGia,
                    DieuKienGiamGia =  m.DieuKienGiamGia,
                    GiaTriGiam = m.LoaiGiamGia == 0 ? m.GiaTriGiam : null,
                    MenhGia = m.LoaiGiamGia == 1 ? m.MenhGia : null, // Chỉ áp dụng cho Voucher
                    NoiDung = m.NoiDung,
                    GiaTriToiDa = m.GiaTriToiDa,
                    TrangThai = m.TrangThai,
                    ThoiGianTao = m.ThoiGianTao,
                    ThoiGianKetThuc = m.ThoiGianKetThuc,
                    SoLuong = m.SoLuong,
                    SoLuongDaSuDung = m.SoLuongDaSuDung,
                     // Kiểm tra xem mã giảm giá đã được áp dụng chưa
            IsApplied = _context.ChiTietMaGiamGias.Any(ct => ct.Id_MaGiamGia == m.Id)
                })
                .ToListAsync();

            return View(maGiamGiaList);
        }

        [HttpGet]

        // GET: Admin/MaGiamGia/Create
        public IActionResult Create()
        {
            var magiamgia = _context.MaGiamGias.ToList();
            // Tạo một đối tượng MaGiamGiaDTO
            var model = new MaGiamGiaDTO
            {
                Ten = $"MG{new Random().Next(10000, 99999)}" // Tạo mã giảm giá ngẫu nhiên
            };

            return View(model);
        }
        // POST: Admin/MaGiamGia/Create
        // POST: Admin/MaGiamGia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaGiamGiaDTO maGiamGiaDto)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra thời gian kết thúc không được trong quá khứ
                if (maGiamGiaDto.ThoiGianKetThuc < DateTime.Now)
                {
                    ModelState.AddModelError("ThoiGianKetThuc", "Thời gian kết thúc không được trong quá khứ.");
                    return View(maGiamGiaDto);
                }

                // Kiểm tra điều kiện đối với coupon hoặc voucher
                if (!maGiamGiaDto.ValidateCouponOrVoucher())
                {
                    ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin cho loại giảm giá đã chọn.");
                    return View(maGiamGiaDto);
                }

                // Kiểm tra trùng tên mã giảm giá
                var existingCoupon = await _context.MaGiamGias
                    .FirstOrDefaultAsync(m => m.Ten == maGiamGiaDto.Ten);

                if (existingCoupon != null)
                {
                    ModelState.AddModelError("Ten", "Tên mã giảm giá đã tồn tại.");
                    return View(maGiamGiaDto);
                }

                // Lưu thông tin mã giảm giá vào cơ sở dữ liệu
                var entity = new MaGiamGia
                {
                    Id = maGiamGiaDto.Id,
                    Ten = maGiamGiaDto.Ten,
                    LoaiGiamGia = maGiamGiaDto.LoaiGiamGia,
                    NoiDung = maGiamGiaDto.NoiDung,
                    GiaTriToiDa = maGiamGiaDto.GiaTriToiDa ?? 0,
                    TrangThai = maGiamGiaDto.TrangThai,
                    ThoiGianTao = DateTime.Now,
                    ThoiGianKetThuc = maGiamGiaDto.ThoiGianKetThuc,
                    DieuKienGiamGia = maGiamGiaDto.DieuKienGiamGia ?? 0,
                    SoLuong = maGiamGiaDto.SoLuong,
                    SoLuongDaSuDung = 0
                };

                // Chỉ lưu trường phù hợp với loại giảm giá
                if (maGiamGiaDto.LoaiGiamGia == 0) // Coupon
                {
                    entity.GiaTriGiam = maGiamGiaDto.GiaTriGiam ?? 0;
                }
                else if (maGiamGiaDto.LoaiGiamGia == 1) // Voucher
                {
                    entity.MenhGia = maGiamGiaDto.MenhGia ?? 0;
                }

                // Thêm mới vào cơ sở dữ liệu
                _context.MaGiamGias.Add(entity);
                await _context.SaveChangesAsync();

                // Thêm thông báo thành công
                TempData["success"] = "Thêm mới mã giảm giá thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, trả về View cùng với thông báo lỗi
            TempData["error"] = "Có lỗi xảy ra, vui lòng kiểm tra lại!";
            return View(maGiamGiaDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var maGiamGia = await _context.MaGiamGias
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maGiamGia == null)
            {
                return NotFound(); // Nếu không tìm thấy mã giảm giá
            }
            // Kiểm tra xem mã giảm giá có được áp dụng trong ChiTietMaGiamGias không
            var hasApplied = await _context.ChiTietMaGiamGias
                                           .AnyAsync(ct => ct.Id_MaGiamGia == id);

            if (hasApplied)
            {
                // Nếu mã giảm giá đã được áp dụng, không cho phép sửa và thông báo lỗi
                TempData["error"] = "Không thể sửa mã giảm giá này vì nó đã được áp dụng.";
                return RedirectToAction(nameof(Index)); // Trở lại trang danh sách
            }
            var maGiamGiaDto = new MaGiamGiaDTO
            {
                Id = maGiamGia.Id,
                Ten = maGiamGia.Ten,
                LoaiGiamGia = maGiamGia.LoaiGiamGia,
                NoiDung = maGiamGia.NoiDung,
                GiaTriToiDa = maGiamGia.GiaTriToiDa,
                TrangThai = maGiamGia.TrangThai,
                ThoiGianTao = maGiamGia.ThoiGianTao,
                ThoiGianKetThuc = maGiamGia.ThoiGianKetThuc,
                DieuKienGiamGia = maGiamGia.DieuKienGiamGia,
                GiaTriGiam = maGiamGia.GiaTriGiam,
                MenhGia = maGiamGia.MenhGia,
                SoLuong = maGiamGia.SoLuong, // Thêm Số lượng vào DTO
                SoLuongDaSuDung = maGiamGia.SoLuongDaSuDung // Thêm số lượng đã sử dụng
            };

            return View(maGiamGiaDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MaGiamGiaDTO maGiamGiaDto)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra thời gian kết thúc không được trong quá khứ
                if (maGiamGiaDto.ThoiGianKetThuc < DateTime.Now)
                {
                    ModelState.AddModelError("ThoiGianKetThuc", "Thời gian kết thúc không được trong quá khứ.");
                    return View(maGiamGiaDto);
                }

                // Kiểm tra xem mã giảm giá có đang được áp dụng trong ChiTietMaGiamGias không
                var hasApplied = await _context.ChiTietMaGiamGias
                                               .AnyAsync(ct => ct.Id_MaGiamGia == maGiamGiaDto.Id);

                if (hasApplied)
                {
                    TempData["error"] = "Không thể sửa mã giảm giá này vì nó đã được áp dụng.";
                    return RedirectToAction(nameof(Index));
                }

                // Kiểm tra xem tên mã giảm giá đã tồn tại chưa
                var existingCoupon = await _context.MaGiamGias
                    .FirstOrDefaultAsync(m => m.Ten == maGiamGiaDto.Ten && m.Id != maGiamGiaDto.Id);

                if (existingCoupon != null)
                {
                    ModelState.AddModelError("Ten", "Tên mã giảm giá đã tồn tại.");
                    return View(maGiamGiaDto);
                }

                var entity = await _context.MaGiamGias
                    .FirstOrDefaultAsync(m => m.Id == maGiamGiaDto.Id);

                if (entity == null)
                {
                    return NotFound();
                }

                // Cập nhật các thông tin chung
                entity.Ten = maGiamGiaDto.Ten;
                entity.LoaiGiamGia = maGiamGiaDto.LoaiGiamGia;
                entity.NoiDung = maGiamGiaDto.NoiDung;
                entity.GiaTriToiDa = maGiamGiaDto.GiaTriToiDa ?? 0;
                entity.TrangThai = maGiamGiaDto.TrangThai;
                entity.ThoiGianKetThuc = maGiamGiaDto.ThoiGianKetThuc;
                entity.DieuKienGiamGia = maGiamGiaDto.DieuKienGiamGia ?? 0;
                entity.SoLuong = maGiamGiaDto.SoLuong;
                entity.SoLuongDaSuDung = maGiamGiaDto.SoLuongDaSuDung;

                // Cập nhật các trường tùy theo loại giảm giá
                if (maGiamGiaDto.LoaiGiamGia == 0) // Coupon
                {
                    entity.GiaTriGiam = maGiamGiaDto.GiaTriGiam ?? 0;
                    entity.MenhGia = 0; // Đặt lại MenhGia nếu chuyển sang Coupon
                }
                else if (maGiamGiaDto.LoaiGiamGia == 1) // Voucher
                {
                    entity.MenhGia = maGiamGiaDto.MenhGia ?? 0;
                    entity.GiaTriGiam = 0; // Đặt lại GiaTriGiam nếu chuyển sang Voucher
                }

                _context.MaGiamGias.Update(entity);
                await _context.SaveChangesAsync();

                TempData["success"] = "Cập nhật mã giảm giá thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Có lỗi xảy ra, vui lòng kiểm tra lại!";
            return View(maGiamGiaDto);
        }

        // GET: Admin/MaGiamGia/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Tìm mã giảm giá theo id
            var maGiamGia = await _context.MaGiamGias.FindAsync(id);

            // Kiểm tra xem mã giảm giá có tồn tại không
            if (maGiamGia == null)
            {
                return NotFound(); // Nếu không tìm thấy mã giảm giá
            }

            // Kiểm tra xem mã giảm giá này có được áp dụng trong ChiTietMaGiamGias hay không
            var hasApplied = await _context.ChiTietMaGiamGias
                                           .AnyAsync(ct => ct.Id_MaGiamGia == id);

            if (hasApplied)
            {
                // Nếu mã giảm giá đã được áp dụng, không cho phép xóa và thông báo lỗi
                TempData["error"] = "Không thể xóa mã giảm giá này vì nó đã được áp dụng.";
                return RedirectToAction(nameof(Index)); // Trở lại trang danh sách
            }

            // Nếu mã giảm giá chưa được áp dụng, thực hiện xóa
            _context.MaGiamGias.Remove(maGiamGia);
            await _context.SaveChangesAsync();

            // Thông báo thành công
            TempData["success"] = "Mã giảm giá đã được xóa thành công!";
            return RedirectToAction(nameof(Index)); // Trở lại trang danh sách
        }


    }
}
