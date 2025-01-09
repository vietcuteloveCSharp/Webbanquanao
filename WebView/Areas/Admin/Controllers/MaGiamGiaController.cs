using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            DieuKienGiamGia = m.LoaiGiamGia == 0 ? m.DieuKienGiamGia : null, // Chỉ áp dụng cho Coupon
            GiaTriGiam = m.LoaiGiamGia == 0 ? m.GiaTriGiam : null,
            MenhGia = m.LoaiGiamGia == 1 ? m.MenhGia : null, // Chỉ áp dụng cho Voucher
            NoiDung = m.NoiDung,
            GiaTriToiDa = m.GiaTriToiDa,
            TrangThai = m.TrangThai,
            ThoiGianTao = m.ThoiGianTao,
            ThoiGianKetThuc = m.ThoiGianKetThuc,
            SoLuong = m.SoLuong,
            SoLuongDaSuDung = m.SoLuongDaSuDung
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaGiamGiaDTO maGiamGiaDto)
        {
            if (ModelState.IsValid)
            {
                var existingCoupon = await _context.MaGiamGias
           .FirstOrDefaultAsync(m => m.Ten == maGiamGiaDto.Ten);

                if (existingCoupon != null)
                {
                    // Nếu tên mã giảm giá đã tồn tại, trả về lỗi
                    ModelState.AddModelError("Ten", "Tên mã giảm giá đã tồn tại.");
                    return View(maGiamGiaDto);
                }

                // Kiểm tra xem mã giảm giá có bị trùng không (nếu có trường mã giảm giá)
                var existingCode = await _context.MaGiamGias
                    .FirstOrDefaultAsync(m => m.Ten == maGiamGiaDto.Ten);

                if (existingCode != null)
                {
                    // Nếu mã giảm giá đã tồn tại, trả về lỗi
                    ModelState.AddModelError("MaGiamGia", "Mã giảm giá đã tồn tại.");
                    return View(maGiamGiaDto);
                }
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
                    DieuKienGiamGia = maGiamGiaDto.DieuKienGiamGia ?? 0,// Lưu điều kiện giảm giá vào cơ sở dữ liệu
                    SoLuong = maGiamGiaDto.SoLuong, // Lưu số lượng
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


                _context.MaGiamGias.Add(entity);
                await _context.SaveChangesAsync();
                TempData["success"] = "Thêm mới mã giảm giá thành công!";
                return RedirectToAction(nameof(Index));
            }
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
                    return NotFound(); // Nếu không tìm thấy mã giảm giá cần sửa
                }

                // Cập nhật các thông tin chung
                entity.Ten = maGiamGiaDto.Ten;
                entity.LoaiGiamGia = maGiamGiaDto.LoaiGiamGia;
                entity.NoiDung = maGiamGiaDto.NoiDung;
                entity.GiaTriToiDa = maGiamGiaDto.GiaTriToiDa ?? 0;
                entity.TrangThai = maGiamGiaDto.TrangThai;
                entity.ThoiGianKetThuc = maGiamGiaDto.ThoiGianKetThuc ?? DateTime.Now; // Nếu ThoiGianKetThuc là null thì mặc định là ngày hiện tại
                entity.DieuKienGiamGia = maGiamGiaDto.DieuKienGiamGia ?? 0;
                entity.SoLuong = maGiamGiaDto.SoLuong; // Cập nhật số lượng
                entity.SoLuongDaSuDung = maGiamGiaDto.SoLuongDaSuDung;

                // Cập nhật các trường tùy theo loại giảm giá
                if (maGiamGiaDto.LoaiGiamGia == 0) // Coupon
                {
                    entity.GiaTriGiam = maGiamGiaDto.GiaTriGiam ?? 0;
                }
                else if (maGiamGiaDto.LoaiGiamGia == 1) // Voucher
                {
                    entity.MenhGia = maGiamGiaDto.MenhGia ?? 0;
                }

                _context.MaGiamGias.Update(entity);
                await _context.SaveChangesAsync();

                TempData["success"] = "Cập nhật mã giảm giá thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(maGiamGiaDto);
        }

        // GET: Admin/MaGiamGia/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var maGiamGia = await _context.MaGiamGias.FindAsync(id);

            if (maGiamGia == null)
            {
                return NotFound(); // Nếu không tìm thấy mã giảm giá
            }

            _context.MaGiamGias.Remove(maGiamGia);
            await _context.SaveChangesAsync();

            TempData["success"] = "Mã giảm giá đã được xóa thành công!";
            return RedirectToAction(nameof(Index)); // Trở lại trang danh sách
        }
      
    }
}
