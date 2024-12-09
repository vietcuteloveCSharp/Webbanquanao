using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebView.NghiaDTO;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private readonly WebBanQuanAoDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SanPhamController(WebBanQuanAoDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _context = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var chiTietSanPhams = await _context.ChiTietSanPhams
                .Include(ct => ct.SanPham)  // Bao gồm bảng SanPham để lấy tên sản phẩm, HinhAnh, DanhMuc, và ThuongHieu
                .ThenInclude(sp => sp.DanhMuc) // Bao gồm DanhMuc qua SanPham
                .Include(ct => ct.SanPham)
                .ThenInclude(sp => sp.ThuongHieu) // Bao gồm ThuongHieu qua SanPham
                .Include(ct => ct.MauSac)  // Bao gồm màu sắc
                .Include(ct => ct.KichThuoc) // Bao gồm kích thước
                .ToListAsync();

            // Nhóm theo Id_SanPham để gộp các chi tiết của cùng một sản phẩm
            var chiTietSanPhamDTOs = chiTietSanPhams
                .GroupBy(ct => ct.Id_SanPham)  // Nhóm theo Id_SanPham để gộp các chi tiết cùng sản phẩm
                .Select(group => new ChiTietSanPhamDTO
                {
                    Id_SanPham = group.Key,  // Id sản phẩm
                    TenSanPham = group.First().SanPham.Ten,  // Lấy tên sản phẩm từ nhóm đầu tiên
                    HinhAnh = group.First().SanPham.HinhAnh, // Lấy hình ảnh từ sản phẩm đầu tiên
                    Gia = group.First().SanPham.Gia,  // Lấy giá từ sản phẩm đầu tiên
                    TenDanhMuc = group.First().SanPham.DanhMuc?.TenDanhMuc,  // Lấy tên danh mục
                    TenThuongHieu = group.First().SanPham.ThuongHieu?.Ten,  // Lấy tên thương hiệu
                    SoLuong = group.Sum(ct => ct.SoLuong),  // Tổng số lượng của sản phẩm
                                                            // Gộp các màu sắc và kích thước
                    TenMauSac = string.Join(", ", group.Select(ct => ct.MauSac.Ten).Distinct()), // Gộp tên màu sắc
                    TenKichThuoc = string.Join(", ", group.Select(ct => ct.KichThuoc.Ten).Distinct()) // Gộp tên kích thước
                }).ToList();
            ViewBag.IsValidColor = new Func<string, bool>(IsValidColor);

            return View(chiTietSanPhamDTOs);
        }
        public static bool IsValidColor(string color)
        {
            // Kiểm tra mã màu HEX hợp lệ
            if (string.IsNullOrEmpty(color))
                return false;

            // Kiểm tra màu có định dạng HEX hợp lệ (#RRGGBB hoặc #RGB)
            return color.StartsWith("#") && (color.Length == 7 || color.Length == 4);
        }
        public IActionResult Create()
        {

            // Khởi tạo SanPhamDTO và đảm bảo các dropdown được khởi tạo đúng
            var sanPhamDTO = new SanPhamDTO
            {
                ChiTietSanPhams = new List<ChiTietSanPhamDTO>() // Khởi tạo danh sách ChiTietSanPham rỗng
            };

            // Gọi PopulateDropDownLists để khởi tạo các dropdown từ cơ sở dữ liệu
            PopulateDropDownLists(sanPhamDTO);

            return View(sanPhamDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPhamDTO sanPhamDTO, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                // Lưu sản phẩm chính vào bảng SanPham
                var sanPham = new SanPham
                {
                    Ten = sanPhamDTO.Ten,
                    MoTa = sanPhamDTO.MoTa,
                    Gia = sanPhamDTO.Gia,
                    SoLuong = sanPhamDTO.SoLuong,
                    NgayTao = DateTime.Now,
                    Id_ThuongHieu = sanPhamDTO.Id_ThuongHieu,
                    Id_DanhMuc = sanPhamDTO.Id_DanhMuc
                };

                // Xử lý file hình ảnh
                if (Image != null && Image.Length > 0 && Image.ContentType.StartsWith("image/"))
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", Image.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    sanPham.HinhAnh = "/images/" + Image.FileName;
                }
                else if (Image != null)
                {
                    ModelState.AddModelError("Image", "Tệp không phải là hình ảnh hợp lệ.");
                    return View(sanPhamDTO); // Trả về View nếu xảy ra lỗi
                }

                _context.SanPhams.Add(sanPham);
                await _context.SaveChangesAsync(); // Lưu sản phẩm chính vào cơ sở dữ liệu

                // Lưu danh sách ChiTietSanPham
                foreach (var chiTiet in sanPhamDTO.ChiTietSanPhams)
                {
                    if (chiTiet.Id_MauSac > 0 && chiTiet.Id_KichThuoc > 0 && chiTiet.SoLuong > 0)
                    {
                        var chiTietEntity = new ChiTietSanPham
                        {
                            Id_SanPham = sanPham.Id,
                            Id_MauSac = chiTiet.Id_MauSac,
                            Id_KichThuoc = chiTiet.Id_KichThuoc,
                            SoLuong = chiTiet.SoLuong,
                            TrangThai = chiTiet.TrangThai ?? true,
                            NgayTao = DateTime.Now
                        };
                        _context.ChiTietSanPhams.Add(chiTietEntity);
                    }
                }

                await _context.SaveChangesAsync(); // Lưu tất cả các chi tiết sản phẩm vào cơ sở dữ liệu

                TempData["success"] = "Thêm sản phẩm và thuộc tính thành công!";
                return RedirectToAction("Index");
            }

            // Xử lý lỗi nếu model không hợp lệ
            TempData["error"] = "Có lỗi xảy ra khi thêm sản phẩm.";
            PopulateDropDownLists(sanPhamDTO); // Đảm bảo dropdown được khởi tạo lại
            return View(sanPhamDTO);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sanPham = await _context.SanPhams
                .Include(sp => sp.ChiTietSanPhams) // Bao gồm chi tiết sản phẩm như màu sắc, kích thước
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            var sanPhamDTO = new SanPhamDTO
            {
                Id = sanPham.Id,
                Ten = sanPham.Ten,
                MoTa = sanPham.MoTa,
                Gia = sanPham.Gia,
                SoLuong = sanPham.SoLuong,
                NgayTao = sanPham.NgayTao,
                Id_ThuongHieu = sanPham.Id_ThuongHieu,
                Id_DanhMuc = sanPham.Id_DanhMuc,
                HinhAnh = sanPham.HinhAnh,
                ChiTietSanPhams = sanPham.ChiTietSanPhams.Select(ct => new ChiTietSanPhamDTO
                {
                    Id_MauSac = ct.Id_MauSac,
                    Id_KichThuoc = ct.Id_KichThuoc,
                    SoLuong = ct.SoLuong
                }).ToList()
            };

            // Truyền dữ liệu vào ViewBag dưới dạng SelectList
            var mauSacs = await _context.MauSacs
                .Select(m => new MauSacDTO
                {
                    Id = m.Id,
                    Ten = m.Ten
                }).ToListAsync();

            var kichThuocs = await _context.KichThuocs
                .Select(k => new KichThuocDTO
                {
                    Id = k.Id,
                    Ten = k.Ten
                }).ToListAsync();

            // Truyền vào ViewBag cho SelectList
            ViewBag.MauSacs = new SelectList(mauSacs, "Id", "Ten");
            ViewBag.KichThuocs = new SelectList(kichThuocs, "Id", "Ten");

            // Khởi tạo các dropdown (màu sắc, kích thước)
            PopulateDropDownLists(sanPhamDTO);

            return View(sanPhamDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SanPhamDTO model, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var sanPham = await _context.SanPhams
                    .Include(sp => sp.ChiTietSanPhams) // Bao gồm chi tiết sản phẩm
                    .FirstOrDefaultAsync(sp => sp.Id == model.Id);

                if (sanPham == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin sản phẩm chính
                sanPham.Ten = model.Ten;
                sanPham.MoTa = model.MoTa;
                sanPham.Gia = model.Gia;
                sanPham.SoLuong = model.SoLuong;
                sanPham.NgayTao = model.NgayTao;
                sanPham.Id_ThuongHieu = model.Id_ThuongHieu;
                sanPham.Id_DanhMuc = model.Id_DanhMuc;

                // Xử lý ảnh nếu có
                if (image != null)
                {
                    // Xóa ảnh cũ nếu cần
                    if (!string.IsNullOrEmpty(sanPham.HinhAnh))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", sanPham.HinhAnh);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Lưu ảnh mới
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    sanPham.HinhAnh = "/images/" + fileName; // Lưu tên file vào database
                }

                // Kiểm tra và xử lý chi tiết sản phẩm (Màu sắc, Kích thước, Số lượng)
                if (model.ChiTietSanPhams != null && model.ChiTietSanPhams.Count > 0)
                {
                    // Xóa các chi tiết sản phẩm cũ
                    _context.ChiTietSanPhams.RemoveRange(sanPham.ChiTietSanPhams);

                    // Thêm các chi tiết sản phẩm mới
                    foreach (var chiTiet in model.ChiTietSanPhams)
                    {
                        // Kiểm tra Id_MauSac và Id_KichThuoc có hợp lệ không
                        var validMauSac = await _context.MauSacs.AnyAsync(m => m.Id == chiTiet.Id_MauSac);
                        var validKichThuoc = await _context.KichThuocs.AnyAsync(k => k.Id == chiTiet.Id_KichThuoc);

                        if (!validMauSac || !validKichThuoc)
                        {
                            ModelState.AddModelError("", "Màu sắc hoặc kích thước không hợp lệ.");
                            PopulateDropDownLists(model); // Khởi tạo lại các dropdown nếu có lỗi
                            return View(model);
                        }

                        var chiTietEntity = new ChiTietSanPham
                        {
                            Id_SanPham = sanPham.Id,
                            Id_MauSac = chiTiet.Id_MauSac,
                            Id_KichThuoc = chiTiet.Id_KichThuoc,
                            SoLuong = chiTiet.SoLuong,
                            TrangThai = chiTiet.TrangThai ?? true,
                            NgayTao = DateTime.Now
                        };

                        _context.ChiTietSanPhams.Add(chiTietEntity);
                    }
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                TempData["success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            // Nếu ModelState không hợp lệ, trả về lại view với các lỗi
            TempData["error"] = "Có lỗi xảy ra khi cập nhật sản phẩm.";
            PopulateDropDownLists(model); // Khởi tạo lại các dropdown nếu có lỗi
            return View(model);
        }



        // Phương thức DELETE để thực hiện xóa ngay mà không cần xác nhận
        public async Task<IActionResult> Delete(int id)
        {
            // Kiểm tra nếu id là null
            if (id == 0)
            {
                return NotFound();
            }

            // Lấy sản phẩm và các chi tiết liên quan
            var sanPham = await _context.SanPhams
                .Include(sp => sp.ChiTietSanPhams) // Bao gồm ChiTietSanPhams liên quan đến sản phẩm
                .FirstOrDefaultAsync(sp => sp.Id == id);

            // Kiểm tra nếu sản phẩm không tồn tại
            if (sanPham == null)
            {
                return NotFound();
            }

            // Xóa tất cả ChiTietSanPhams liên quan đến sản phẩm
            _context.ChiTietSanPhams.RemoveRange(sanPham.ChiTietSanPhams);

            // Xóa sản phẩm chính
            _context.SanPhams.Remove(sanPham);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Thêm thông báo thành công
            TempData["success"] = "Xóa sản phẩm và thuộc tính thành công!";

            // Chuyển hướng về trang danh sách sản phẩm
            return RedirectToAction("Index");
        }

        private void PopulateDropDownLists(SanPhamDTO sanPhamDTO = null)
        {
            ViewBag.ThuongHieus = new SelectList(_context.ThuongHieus, "Id", "Ten", sanPhamDTO?.Id_ThuongHieu);
            ViewBag.DanhMucs = new SelectList(_context.DanhMucs, "Id", "TenDanhMuc", sanPhamDTO?.Id_DanhMuc);
            ViewBag.MauSacs = new SelectList(_context.MauSacs, "Id", "Ten");
            ViewBag.KichThuocs = new SelectList(_context.KichThuocs, "Id", "Ten");
        }

        // Lấy danh sách các lỗi từ ModelState
        private List<string> GetModelErrors()
        {
            return ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
        }
        // Kiểm tra xem sản phẩm có tồn tại trong cơ sở dữ liệu không
        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(sp => sp.Id == id);
        }
        //Add so luong
        [HttpGet]
        public async Task<IActionResult> AddQuantity(int id)
        {
            var product = await _context.SanPhams
                .Include(sp => sp.ChiTietSanPhams)
                    .ThenInclude(ct => ct.MauSac) // Bao gồm thông tin Màu sắc
                .Include(sp => sp.ChiTietSanPhams)
                    .ThenInclude(ct => ct.KichThuoc) // Bao gồm thông tin Kích thước
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Truyền thông tin chi tiết sản phẩm vào ViewBag
            ViewBag.ChiTietSanPhams = product.ChiTietSanPhams;

            // Truyền số lượng hiện tại của sản phẩm vào ViewBag
            var productByQuantity = await _context.ProductQuantities.Where(pq => pq.SanPhamId == id).ToListAsync();
            ViewBag.ProductByQuantity = productByQuantity;

            ViewBag.Id = id;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreProductQuantity(SoLuongSanPhamDTO productQuantityDTO)
        {
            var product = await _context.SanPhams.FindAsync(productQuantityDTO.SanPhamId);
            if (product == null)
            {
                return NotFound();
            }

            // Cập nhật số lượng sản phẩm chính
            product.SoLuong += productQuantityDTO.SoLuong;

            // Cập nhật số lượng cho các chi tiết sản phẩm (CTSP) theo màu sắc và kích thước
            var chiTietSanPham = await _context.ChiTietSanPhams
                .FirstOrDefaultAsync(ct => ct.Id_SanPham == productQuantityDTO.SanPhamId &&
                                            ct.Id_MauSac == productQuantityDTO.Id_MauSac &&
                                            ct.Id_KichThuoc == productQuantityDTO.Id_KichThuoc);

            if (chiTietSanPham != null)
            {
                chiTietSanPham.SoLuong += productQuantityDTO.SoLuong;
            }

            // Tạo một entity mới từ DTO và lưu vào bảng ProductQuantities
            var productQuantityEntity = new SoLuongSanPham
            {
                SanPhamId = productQuantityDTO.SanPhamId,
                SoLuong = productQuantityDTO.SoLuong,
                Id_MauSac = productQuantityDTO.Id_MauSac,
                Id_KichThuoc = productQuantityDTO.Id_KichThuoc,
                NgayTao = DateTime.Now
            };

            _context.ProductQuantities.Add(productQuantityEntity);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Hiển thị thông báo thành công
            TempData["success"] = "Thêm số lượng sản phẩm thành công";
            return RedirectToAction("AddQuantity", "SanPham", new { Id = productQuantityDTO.SanPhamId });
        }

    }
}
