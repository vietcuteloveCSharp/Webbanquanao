using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebView.NghiaDTO;

[Area("Admin")]
[Authorize(Roles ="admin")]
public class NhanVienController : Controller
{
    private readonly WebBanQuanAoDbContext _context;

    public NhanVienController(WebBanQuanAoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var nhanViens = await _context.NhanViens
            .Include(nv => nv.ChucVu)
            .Where(nv => nv.Id_ChucVu != 11)
            .Select(nv => new NhanVienDTO
            {
                Id = nv.Id,
                TaiKhoan = nv.TaiKhoan,
                MatKhau = nv.MatKhau,
                TenNhanVien = nv.TenNhanVien,
                Sdt = nv.Sdt,
                Email = nv.Email,
                NgaySinh = nv.NgaySinh,
                DiaChi = nv.DiaChi,
                GhiChu = nv.GhiChu,
                TrangThai = nv.TrangThai,
                NgayTao = nv.NgayTao,
                NgayCapNhat = nv.NgayCapNhat,
                Id_ChucVu = nv.Id_ChucVu,
                ChucVuDTO = new ChucVuDTO { Id = nv.ChucVu.Id, Ten = nv.ChucVu.Ten }
            })
            .ToListAsync();
        return View(nhanViens);
    }

    public IActionResult Create()
    {
        ViewBag.ChucVus = _context.ChucVus
            .Where(cv => cv.Id != 11) // Lọc bỏ chức vụ Admin
            .Select(cv => new { cv.Id, cv.Ten })
            .ToList();

        // Lấy ID của chức vụ "Nhân viên"
        ViewBag.NhanVienId = _context.ChucVus
            .Where(cv => cv.Ten == "Nhân viên bán hàng")
            .Select(cv => cv.Id)
            .FirstOrDefault();

        return View();
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NhanVienDTO nhanVienDTO)
    {
        var nhanVienId = _context.ChucVus
            .Where(cv => cv.Ten == "Nhân viên bán hàng")
            .Select(cv => cv.Id)
            .FirstOrDefault();

        // Nếu là Nhân viên, bỏ qua validation TaiKhoan và MatKhau
        if (nhanVienDTO.Id_ChucVu == nhanVienId)
        {
            ModelState.Remove("TaiKhoan"); // Loại bỏ lỗi validation cho TaiKhoan
            ModelState.Remove("MatKhau");  // Loại bỏ lỗi validation cho MatKhau
            nhanVienDTO.TaiKhoan = null;   // Đặt null để không lưu
            nhanVienDTO.MatKhau = null;    // Đặt null để không lưu
        }

        if (ModelState.IsValid)
        {
            // Kiểm tra trùng lặp (chỉ khi không phải Nhân viên)
            if (nhanVienDTO.Id_ChucVu != nhanVienId)
            {
                if (_context.NhanViens.Any(nv => nv.TaiKhoan == nhanVienDTO.TaiKhoan))
                {
                    ModelState.AddModelError("TaiKhoan", "Tài khoản đã tồn tại.");
                }
            }

            if (_context.NhanViens.Any(nv => nv.Email == nhanVienDTO.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
            }
            if (_context.NhanViens.Any(nv => nv.Sdt == nhanVienDTO.Sdt))
            {
                ModelState.AddModelError("Sdt", "Số điện thoại đã tồn tại.");
            }
            if (_context.NhanViens.Any(nv => nv.TenNhanVien == nhanVienDTO.TenNhanVien))
            {
                ModelState.AddModelError("TenNhanVien", "Tên nhân viên đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                var nhanVien = new NhanVien
                {
                    TaiKhoan = nhanVienDTO.TaiKhoan,
                    MatKhau = nhanVienDTO.MatKhau,
                    TenNhanVien = nhanVienDTO.TenNhanVien,
                    Sdt = nhanVienDTO.Sdt,
                    Email = nhanVienDTO.Email,
                    NgaySinh = nhanVienDTO.NgaySinh,
                    DiaChi = nhanVienDTO.DiaChi,
                    GhiChu = nhanVienDTO.GhiChu,
                    NgayTao = DateTime.Now,
                    NgayCapNhat = DateTime.Now,
                    TrangThai = nhanVienDTO.TrangThai,
                    Id_ChucVu = nhanVienDTO.Id_ChucVu
                };

                _context.NhanViens.Add(nhanVien);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm nhân viên thành công.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Load lại ViewBag khi có lỗi
        ViewBag.ChucVus = _context.ChucVus
            .Where(cv => cv.Id != 11)
            .Select(cv => new { cv.Id, cv.Ten })
            .ToList();
        ViewBag.NhanVienId = nhanVienId;

        TempData["ErrorMessage"] = "Thêm nhân viên thất bại. Vui lòng kiểm tra lại thông tin.";
        return View(nhanVienDTO);
    }
    public async Task<IActionResult> Edit(int id)
    {
        var nhanVien = await _context.NhanViens.FindAsync(id);
        if (nhanVien == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy nhân viên.";
            return NotFound();
        }

        var nhanVienDTO = new NhanVienDTO
        {
            Id = nhanVien.Id,
            TaiKhoan = nhanVien.TaiKhoan,
            MatKhau = nhanVien.MatKhau,
            TenNhanVien = nhanVien.TenNhanVien,
            Sdt = nhanVien.Sdt,
            Email = nhanVien.Email,
            NgaySinh = nhanVien.NgaySinh,
            DiaChi = nhanVien.DiaChi,
            GhiChu = nhanVien.GhiChu,
            NgayTao = nhanVien.NgayTao,
            NgayCapNhat = nhanVien.NgayCapNhat,
            Id_ChucVu = nhanVien.Id_ChucVu,
            TrangThai = nhanVien.TrangThai
        };

        ViewBag.ChucVus = _context.ChucVus
            .Where(cv => cv.Id != 11) // Loại bỏ Admin
            .Select(cv => new { cv.Id, cv.Ten })
            .ToList();

        ViewBag.NhanVienId = _context.ChucVus
            .Where(cv => cv.Ten == "Nhân viên bán hàng")
            .Select(cv => cv.Id)
            .FirstOrDefault();

        return View(nhanVienDTO);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NhanVienDTO nhanVienDTO)
    {
        if (id != nhanVienDTO.Id)
        {
            TempData["ErrorMessage"] = "ID nhân viên không hợp lệ.";
            return BadRequest();
        }

        var nhanVienId = _context.ChucVus
            .Where(cv => cv.Ten == "Nhân Viên")
            .Select(cv => cv.Id)
            .FirstOrDefault();

        // Nếu là Nhân viên, bỏ qua validation TaiKhoan và MatKhau
        if (nhanVienDTO.Id_ChucVu == nhanVienId)
        {
            ModelState.Remove("TaiKhoan");
            ModelState.Remove("MatKhau");
            nhanVienDTO.TaiKhoan = null;
            nhanVienDTO.MatKhau = null;
        }

        if (ModelState.IsValid)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy nhân viên.";
                return NotFound();
            }

            // Kiểm tra trùng lặp (bỏ qua bản ghi hiện tại)
            if (nhanVienDTO.Id_ChucVu != nhanVienId && _context.NhanViens.Any(nv => nv.TaiKhoan == nhanVienDTO.TaiKhoan && nv.Id != id))
            {
                ModelState.AddModelError("TaiKhoan", "Tài khoản đã tồn tại.");
            }
            if (_context.NhanViens.Any(nv => nv.Email == nhanVienDTO.Email && nv.Id != id))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
            }
            if (_context.NhanViens.Any(nv => nv.Sdt == nhanVienDTO.Sdt && nv.Id != id))
            {
                ModelState.AddModelError("Sdt", "Số điện thoại đã tồn tại.");
            }
            if (_context.NhanViens.Any(nv => nv.TenNhanVien == nhanVienDTO.TenNhanVien && nv.Id != id))
            {
                ModelState.AddModelError("TenNhanVien", "Tên nhân viên đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                nhanVien.TenNhanVien = nhanVienDTO.TenNhanVien;
                nhanVien.Sdt = nhanVienDTO.Sdt;
                nhanVien.Email = nhanVienDTO.Email;
                nhanVien.NgaySinh = nhanVienDTO.NgaySinh;
                nhanVien.DiaChi = nhanVienDTO.DiaChi;
                nhanVien.GhiChu = nhanVienDTO.GhiChu;
                nhanVien.NgayCapNhat = DateTime.Now;
                nhanVien.Id_ChucVu = nhanVienDTO.Id_ChucVu;
                nhanVien.TrangThai = nhanVienDTO.TrangThai;

                if (nhanVienDTO.Id_ChucVu == nhanVienId)
                {
                    nhanVien.TaiKhoan = null;
                    nhanVien.MatKhau = null;
                }
                else
                {
                    nhanVien.TaiKhoan = nhanVienDTO.TaiKhoan;
                    if (!string.IsNullOrEmpty(nhanVienDTO.MatKhau))
                    {
                        nhanVien.MatKhau = nhanVienDTO.MatKhau; // Chỉ cập nhật mật khẩu nếu có nhập
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật nhân viên thành công.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Load lại ViewBag khi có lỗi
        ViewBag.ChucVus = _context.ChucVus
            .Where(cv => cv.Id != 11)
            .Select(cv => new { cv.Id, cv.Ten })
            .ToList();
        ViewBag.NhanVienId = nhanVienId;

        TempData["ErrorMessage"] = "Cập nhật nhân viên thất bại. Vui lòng kiểm tra lại thông tin.";
        return View(nhanVienDTO);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var nhanvien = await _context.NhanViens.FindAsync(id);

        if (nhanvien == null)
        {
            return NotFound();
        }

        // Chuyển đổi trạng thái
        nhanvien.TrangThai = !nhanvien.TrangThai;

        try
        {
            _context.NhanViens.Update(nhanvien);
            await _context.SaveChangesAsync();

            TempData["success"] = "Thay đổi trạng thái thành công!";
        }
        catch (Exception ex)
        {
            TempData["error"] = $"Lỗi: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        var nhanVien = await _context.NhanViens.FindAsync(id);
        if (nhanVien == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy nhân viên.";
            return NotFound();
        }

        _context.NhanViens.Remove(nhanVien);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Xóa nhân viên thành công.";
        return RedirectToAction(nameof(Index));
    }
}
