using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebView.NghiaDTO;

[Area("Admin")]
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
            .Where(cv => cv.Ten == "Nhân Viên")
            .Select(cv => cv.Id)
            .FirstOrDefault();

        return View();
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NhanVienDTO nhanVienDTO)
    {
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
            MatKhau = "", // Không hiển thị mật khẩu cũ
            TenNhanVien = nhanVien.TenNhanVien,
            Sdt = nhanVien.Sdt,
            Email = nhanVien.Email,
            NgaySinh = nhanVien.NgaySinh,
            DiaChi = nhanVien.DiaChi,
            GhiChu = nhanVien.GhiChu,
            NgayTao = nhanVien.NgayTao,
            NgayCapNhat = nhanVien.NgayCapNhat,
            Id_ChucVu = nhanVien.Id_ChucVu
        };

        // Chỉ lấy chức vụ Nhân viên và Thu ngân (loại bỏ Admin)
        ViewBag.ChucVus = _context.ChucVus
            .Where(cv => cv.Id != 11) // Giả sử Admin có ID = 11
            .Select(cv => new { cv.Id, cv.Ten })
            .ToList();

        ViewBag.NhanVienId = 14; // Giả sử ID của Nhân viên là 1

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

        if (ModelState.IsValid)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy nhân viên.";
                return NotFound();
            }

            nhanVien.TenNhanVien = nhanVienDTO.TenNhanVien;
            nhanVien.Sdt = nhanVienDTO.Sdt;
            nhanVien.Email = nhanVienDTO.Email;
            nhanVien.NgaySinh = nhanVienDTO.NgaySinh;
            nhanVien.DiaChi = nhanVienDTO.DiaChi;
            nhanVien.GhiChu = nhanVienDTO.GhiChu;
            nhanVien.NgayCapNhat = DateTime.Now;
            nhanVien.Id_ChucVu = nhanVienDTO.Id_ChucVu;

            // Nếu là Nhân viên -> Không lưu tài khoản, mật khẩu
            if (nhanVienDTO.Id_ChucVu == 14) // Giả sử ID của Nhân viên là 1
            {
                nhanVien.TaiKhoan = null;
                nhanVien.MatKhau = null;
            }
            else
            {
                nhanVien.TaiKhoan = nhanVienDTO.TaiKhoan;
                if (!string.IsNullOrEmpty(nhanVienDTO.MatKhau))
                {
                    nhanVien.MatKhau = nhanVienDTO.MatKhau;
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật nhân viên thành công.";
            return RedirectToAction(nameof(Index));
        }

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
