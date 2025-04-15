    using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Sanphams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhuyenMaiController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        public KhuyenMaiController(WebBanQuanAoDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _context = dbcontext;
        }
        // GET: Danh sách khuyến mại
        public async Task<IActionResult> Index()
        {
            var khuyenMaiDtos = await _context.KhuyenMais
                .Include(km => km.chiTietKhuyenMais)
                .ThenInclude(ct => ct.DanhMuc) // Lấy thêm thông tin danh mục liên kết
                .Select(km => new KhuyenMaiDTO
                {
                    Id = km.Id,
                    Ten = km.Ten,
                    MoTa = km.MoTa,
                    GiaTriGiam = km.GiaTriGiam,
                    DieuKienGiamGia = km.DieuKienGiamGia,
                    NgayTao = km.NgayTao,
                    NgayBatDau = km.NgayBatDau,
                    NgayKetThuc = km.NgayKetThuc,
                    TrangThai = km.TrangThai,
                    chiTietKhuyenMaiDTOs = km.chiTietKhuyenMais.Select(ct => new ChiTietKhuyenMaiDTO
                    {
                        Id_KhuyenMai = ct.Id_KhuyenMai,
                        Id_DanhMuc = ct.Id_DanhMuc,
                        DanhMucDTO = new DanhMucDTO // Chuyển danh mục thành DTO nếu cần
                        {
                            Id = ct.DanhMuc.Id,
                            TenDanhMuc = ct.DanhMuc.TenDanhMuc
                        }
                    }).ToList()
                }).ToListAsync();

            return View(khuyenMaiDtos);
        }

        public async Task PopulateDropDownLists(KhuyenMaiDTO khuyenMaiDTO = null)
        {
            var danhMucs = await _context.DanhMucs.Select(dm => new SelectListItem
            {
                Value = dm.Id.ToString(),
                Text = dm.TenDanhMuc
            }).ToListAsync();

            ViewBag.DanhMucs = danhMucs;
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Kiểm tra và cập nhật trạng thái khuyến mãi đã hết hạn
            var khuyenMais = _context.KhuyenMais
                .Where(km => km.TrangThai == 1 && km.NgayKetThuc <= DateTime.Now) // Lọc khuyến mãi đang hoạt động và đã hết hạn
                .ToList();

            foreach (var km in khuyenMais)
            {
                km.TrangThai = 2; // Chuyển sang trạng thái "Kết thúc"
            }
            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            // Lấy danh mục đã được chọn trong các khuyến mãi có trạng thái "Đang khuyến mãi" hoặc "Kết thúc"
            var danhMucIdsDaChon = _context.KhuyenMais
                .Where(km => km.TrangThai == 1 || km.TrangThai == 2)  // Lọc các khuyến mãi có trạng thái 'Đang khuyến mãi' (1) hoặc 'Kết thúc' (2)
                .SelectMany(km => km.chiTietKhuyenMais)  // Lấy chi tiết khuyến mãi
                .Select(ct => ct.Id_DanhMuc)  // Lấy ID danh mục đã chọn
                .Distinct()
                .ToList();

            // Lọc các danh mục chưa được chọn (chưa có trong danh sách đã chọn)
            var danhMucsChuaChon = _context.DanhMucs
                .Where(dm => !danhMucIdsDaChon.Contains(dm.Id))  // Chỉ lấy những danh mục chưa được chọn
                .Select(dm => new SelectListItem
                {
                    Value = dm.Id.ToString(),
                    Text = dm.TenDanhMuc
                })
                .ToList();

            // Khởi tạo giá trị mặc định cho NgayBatDau và NgayKetThuc
            var khuyenmaiDTO = new KhuyenMaiDTO()
            {
                NgayBatDau = DateTime.Now,
                NgayKetThuc = DateTime.Now.AddHours(1),
            };

            // Gán danh sách danh mục chưa chọn vào ViewBag để hiển thị trong form
            ViewBag.DanhMucs = danhMucsChuaChon;

            return View(khuyenmaiDTO);
        }
        // POST: Thêm Khuyến Mại
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhuyenMaiDTO khuyenMaiDTO)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên khuyến mãi đã tồn tại trong cơ sở dữ liệu chưa
                var existingKhuyenMai = await _context.KhuyenMais
                    .FirstOrDefaultAsync(km => km.Ten == khuyenMaiDTO.Ten);

                if (existingKhuyenMai != null)
                {
                    // Nếu đã tồn tại, hiển thị thông báo lỗi
                    ModelState.AddModelError("Ten", "Tên khuyến mãi đã tồn tại. Vui lòng chọn tên khác.");
                }

                // Kiểm tra Ngày kết thúc phải lớn hơn Ngày bắt đầu
                if (khuyenMaiDTO.NgayKetThuc <= khuyenMaiDTO.NgayBatDau)
                {
                    ModelState.AddModelError("NgayKetThuc", "Ngày kết thúc phải lớn hơn ngày bắt đầu.");
                }

                // Nếu có lỗi thì trả lại view cùng với thông báo lỗi
                if (!ModelState.IsValid)
                {
                    // Gán lại danh sách danh mục cho ViewBag
                    ViewBag.DanhMucs = await _context.DanhMucs
                        .Select(dm => new SelectListItem
                        {
                            Value = dm.Id.ToString(),
                            Text = dm.TenDanhMuc
                        })
                        .ToListAsync();
                    return View(khuyenMaiDTO);
                }

                // Tạo khuyến mãi mới
                var khuyenMai = new KhuyenMai
                {
                    Ten = khuyenMaiDTO.Ten,
                    MoTa = khuyenMaiDTO.MoTa,
                    GiaTriGiam = khuyenMaiDTO.GiaTriGiam,
                    DieuKienGiamGia = khuyenMaiDTO.DieuKienGiamGia,
                    NgayTao = DateTime.Now,  // Sử dụng thời gian hiện tại cho ngày tạo
                    NgayBatDau = khuyenMaiDTO.NgayBatDau,
                    NgayKetThuc = khuyenMaiDTO.NgayKetThuc,
                    TrangThai = khuyenMaiDTO.NgayKetThuc <= DateTime.Now ? 2 : khuyenMaiDTO.TrangThai // Tự động đặt trạng thái "Kết thúc" nếu ngày kết thúc đã qua
                };

                // Thêm khuyến mãi vào cơ sở dữ liệu
                _context.KhuyenMais.Add(khuyenMai);
                await _context.SaveChangesAsync();

                // Thêm chi tiết khuyến mãi (Danh mục áp dụng)
                if (Request.Form["Id_DanhMuc"] != StringValues.Empty)
                {
                    var danhMucIds = Request.Form["Id_DanhMuc"].ToArray();
                    foreach (var danhMucId in danhMucIds)
                    {
                        int idDanhMuc = int.Parse(danhMucId);
                        var chiTietKhuyenMai = new ChiTietKhuyenMai
                        {
                            Id_KhuyenMai = khuyenMai.Id,
                            Id_DanhMuc = idDanhMuc
                        };

                        _context.ChiTietKhuyenMais.Add(chiTietKhuyenMai);
                    }
                    await _context.SaveChangesAsync();
                }

                // Thêm thông báo thành công
                TempData["SuccessMessage"] = "Khuyến mãi đã được thêm thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, trả lại View cùng với thông báo lỗi
            TempData["ErrorMessage"] = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại!";

            // Gán lại danh sách danh mục vào ViewBag khi có lỗi
            ViewBag.DanhMucs = await _context.DanhMucs
                .Select(dm => new SelectListItem
                {
                    Value = dm.Id.ToString(),
                    Text = dm.TenDanhMuc
                })
                .ToListAsync();

            return View(khuyenMaiDTO);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Lấy thông tin Khuyến Mại từ cơ sở dữ liệu
            var khuyenMai = await _context.KhuyenMais
                .Include(km => km.chiTietKhuyenMais) // Bao gồm danh mục của Khuyến Mại
                .FirstOrDefaultAsync(km => km.Id == id);

            if (khuyenMai == null)
            {
                return NotFound();
            }

            // Lấy các danh mục đã được gán cho các khuyến mãi khác mà chưa ngưng
            var danhMucIdsDuocGan = await _context.ChiTietKhuyenMais
                .Where(ct => ct.KhuyenMai.TrangThai != 0 && ct.KhuyenMai.Id != id) // Loại bỏ khuyến mãi đang chỉnh sửa và các khuyến mãi có trạng thái ngưng
                .Select(ct => ct.Id_DanhMuc)
                .Distinct()
                .ToListAsync();

            var khuyenMaiDTO = new KhuyenMaiDTO
            {
                Id = khuyenMai.Id,
                Ten = khuyenMai.Ten,
                MoTa = khuyenMai.MoTa,
                GiaTriGiam = khuyenMai.GiaTriGiam,
                DieuKienGiamGia = khuyenMai.DieuKienGiamGia,
                NgayTao = khuyenMai.NgayTao,
                NgayBatDau = khuyenMai.NgayBatDau, // Lấy thời gian thực tế từ dữ liệu
                NgayKetThuc = khuyenMai.NgayKetThuc, // Lấy thời gian thực tế từ dữ liệu
                TrangThai = khuyenMai.TrangThai,
                // Chuyển danh sách ChiTietKhuyenMaiDTO thành một danh sách các Id danh mục đã chọn
                chiTietKhuyenMaiDTOs = khuyenMai.chiTietKhuyenMais.Select(ct => new ChiTietKhuyenMaiDTO
                {
                    Id_DanhMuc = ct.Id_DanhMuc
                }).ToList()
            };

            // Lấy danh mục chưa bị trùng
            ViewBag.DanhMucs = _context.DanhMucs
                .Where(dm => !danhMucIdsDuocGan.Contains(dm.Id)) // Lọc ra các danh mục chưa được sử dụng
                .Select(dm => new SelectListItem
                {
                    Value = dm.Id.ToString(),
                    Text = dm.TenDanhMuc
                })
                .ToList();

            return View(khuyenMaiDTO);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(KhuyenMaiDTO khuyenMaiDTO)
        {
            if (ModelState.IsValid)
            {
                var khuyenMai = await _context.KhuyenMais
                    .Include(km => km.chiTietKhuyenMais)
                    .FirstOrDefaultAsync(km => km.Id == khuyenMaiDTO.Id);

                if (khuyenMai == null)
                {
                    return NotFound();
                }

                // Kiểm tra tên khuyến mãi có trùng hay không
                var existingKhuyenMai = await _context.KhuyenMais
                    .FirstOrDefaultAsync(km => km.Ten == khuyenMaiDTO.Ten && km.Id != khuyenMaiDTO.Id);

                if (existingKhuyenMai != null)
                {
                    ModelState.AddModelError("Ten", "Tên khuyến mãi đã tồn tại. Vui lòng chọn tên khác.");
                }

                // Kiểm tra ngày kết thúc phải lớn hơn ngày bắt đầu
                if (khuyenMaiDTO.NgayKetThuc <= khuyenMaiDTO.NgayBatDau)
                {
                    ModelState.AddModelError("NgayKetThuc", "Ngày kết thúc phải lớn hơn ngày bắt đầu.");
                }

                // Nếu có lỗi validation, trả lại view với dữ liệu
                if (!ModelState.IsValid)
                {
                    ViewBag.DanhMucs = await _context.DanhMucs
                        .Select(dm => new SelectListItem
                        {
                            Value = dm.Id.ToString(),
                            Text = dm.TenDanhMuc
                        })
                        .ToListAsync();
                    return View(khuyenMaiDTO);
                }

                // Cập nhật thông tin khuyến mãi
                khuyenMai.Ten = khuyenMaiDTO.Ten;
                khuyenMai.MoTa = khuyenMaiDTO.MoTa;
                khuyenMai.GiaTriGiam = khuyenMaiDTO.GiaTriGiam;
                khuyenMai.DieuKienGiamGia = khuyenMaiDTO.DieuKienGiamGia;
                khuyenMai.NgayBatDau = khuyenMaiDTO.NgayBatDau;
                khuyenMai.NgayKetThuc = khuyenMaiDTO.NgayKetThuc;
                khuyenMai.TrangThai = khuyenMaiDTO.TrangThai;

                // Xóa các chi tiết khuyến mãi cũ và thêm các chi tiết mới
                _context.ChiTietKhuyenMais.RemoveRange(khuyenMai.chiTietKhuyenMais);

                if (Request.Form["Id_DanhMuc"] != StringValues.Empty)
                {
                    var danhMucIds = Request.Form["Id_DanhMuc"].ToArray();
                    foreach (var danhMucId in danhMucIds)
                    {
                        int idDanhMuc = int.Parse(danhMucId);
                        var chiTietKhuyenMai = new ChiTietKhuyenMai
                        {
                            Id_KhuyenMai = khuyenMai.Id,
                            Id_DanhMuc = idDanhMuc
                        };
                        _context.ChiTietKhuyenMais.Add(chiTietKhuyenMai);
                    }
                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Khuyến mãi đã được cập nhật thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Đảm bảo rằng danh mục được truyền lại khi có lỗi validation
            ViewBag.DanhMucs = await _context.DanhMucs
                .Select(dm => new SelectListItem
                {
                    Value = dm.Id.ToString(),
                    Text = dm.TenDanhMuc
                })
                .ToListAsync();

            TempData["ErrorMessage"] = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại!";
            return View(khuyenMaiDTO);
        }

        // POST: Xóa Khuyến Mại
        public async Task<IActionResult> Delete(int id)
        {
            var khuyenMai = await _context.KhuyenMais.FindAsync(id);

            if (khuyenMai != null)
            {
                var chiTietKhuyenMais = _context.ChiTietKhuyenMais.Where(ct => ct.Id_KhuyenMai == id);
                _context.ChiTietKhuyenMais.RemoveRange(chiTietKhuyenMais);
                _context.KhuyenMais.Remove(khuyenMai);

                await _context.SaveChangesAsync();

                // Thêm thông báo thành công
                TempData["SuccessMessage"] = "Khuyến mãi đã được xóa thành công!";
            }
            else
            {
                // Thêm thông báo lỗi nếu không tìm thấy khuyến mãi
                TempData["ErrorMessage"] = "Không tìm thấy khuyến mãi để xóa!";
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
