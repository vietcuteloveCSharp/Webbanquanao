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
        .Include(ct => ct.MauSac)
        .Include(ct => ct.KichThuoc)
        .ToListAsync();
            // Chuyển đổi thành ChiTietSanPhamDTO
            var chiTietSanPhamDTOs = chiTietSanPhams.Select(ct => new ChiTietSanPhamDTO
            {
                Id = ct.Id,
                SoLuong = ct.SoLuong,
                NgayTao = ct.NgayTao,
                TrangThai = ct.TrangThai,
                Id_SanPham = ct.Id_SanPham,
                Id_MauSac = ct.Id_MauSac,
                Id_KichThuoc = ct.Id_KichThuoc,
                TenMauSac = ct.MauSac.Ten, // Gán tên màu sắc
                TenKichThuoc = ct.KichThuoc.Ten, // Gán tên kích thước
                TenSanPham = ct.SanPham.Ten, // Lấy tên sản phẩm từ bảng SanPham
                HinhAnh = ct.SanPham.HinhAnh, // Lấy hình ảnh từ bảng SanPham
                Gia = ct.SanPham.Gia, // Lấy giá từ bảng SanPham
                TenDanhMuc = ct.SanPham.DanhMuc?.TenDanhMuc, // Lấy tên danh mục từ bảng DanhMuc
                TenThuongHieu = ct.SanPham.ThuongHieu?.Ten // Lấy tên thương hiệu từ bảng ThuongHieu
            }).ToList();

            return View(chiTietSanPhamDTOs);
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

                // Nếu có file ảnh, lưu vào thư mục và gán đường dẫn vào sản phẩm
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
                }


                _context.SanPhams.Add(sanPham);
                await _context.SaveChangesAsync();  // Lưu sản phẩm chính vào cơ sở dữ liệu

                // Lưu danh sách ChiTietSanPham (bao gồm màu sắc, kích thước, trạng thái)
                foreach (var chiTiet in sanPhamDTO.ChiTietSanPhams)
                {
                    var chiTietEntity = new ChiTietSanPham
                    {
                        Id_SanPham = sanPham.Id,
                        Id_MauSac = chiTiet.Id_MauSac,
                        Id_KichThuoc = chiTiet.Id_KichThuoc,
                        SoLuong = chiTiet.SoLuong,
                        TrangThai = chiTiet.TrangThai ?? false,
                        NgayTao = DateTime.Now
                    };

                    _context.ChiTietSanPhams.Add(chiTietEntity);
                }

                await _context.SaveChangesAsync();

                TempData["success"] = "Thêm sản phẩm và thuộc tính thành công!";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Có lỗi xảy ra khi thêm sản phẩm.";
            ViewBag.Errors = GetModelErrors();
            return View(sanPhamDTO);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            var sanPhamDTO = new SanPhamDTO
            {
                Id = sanPham.Id,
                Ten = sanPham.Ten,
                Gia = sanPham.Gia,
                SoLuong = sanPham.SoLuong,
                NgayTao = sanPham.NgayTao,
                Id_ThuongHieu = sanPham.Id_ThuongHieu,
                Id_DanhMuc = sanPham.Id_DanhMuc,
                HinhAnh = sanPham.HinhAnh // Đảm bảo rằng bạn truyền URL ảnh (hoặc tên file)
            };

            PopulateDropDownLists(sanPhamDTO); // Khởi tạo các dropdown

            return View(sanPhamDTO);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(SanPhamDTO model, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var sanPham = await _context.SanPhams
                    .FirstOrDefaultAsync(sp => sp.Id == model.Id);

                if (sanPham == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin sản phẩm
                sanPham.Ten = model.Ten;
                sanPham.Gia = model.Gia;
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

                    sanPham.HinhAnh = fileName; // Lưu tên file vào database
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

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

        private void PopulateDropDownLists(SanPhamDTO sanPhamDTO= null)
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
            var productbyquantity = await _context.ProductQuantities.Where(pq => pq.SanPhamId == id).ToListAsync();
            ViewBag.ProductByQuantity = productbyquantity;
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreProductQuantity(SoLuongSanPhamDTO productQuantityDTO)
        {
            // Kiểm tra xem sản phẩm có tồn tại không
            var product = await _context.SanPhams.FindAsync(productQuantityDTO.SanPhamId);
            if (product == null)
            {
                return NotFound();
            }

            // Cập nhật số lượng sản phẩm chính
            product.SoLuong += productQuantityDTO.SoLuong;

            // Cập nhật số lượng chi tiết sản phẩm (CTSP)
            var chiTietSanPhams = await _context.ChiTietSanPhams.Where(ct => ct.Id_SanPham == productQuantityDTO.SanPhamId).ToListAsync();
            foreach (var chiTiet in chiTietSanPhams)
            {
                chiTiet.SoLuong += productQuantityDTO.SoLuong; // Cập nhật số lượng cho từng chi tiết
            }

            // Tạo một entity mới từ DTO và lưu vào bảng ProductQuantities
            var productQuantityEntity = new SoLuongSanPham
            {
                SanPhamId = productQuantityDTO.SanPhamId,
                SoLuong = productQuantityDTO.SoLuong,
                NgayTao = DateTime.Now
            };

            // Thêm vào bảng ProductQuantities
            _context.ProductQuantities.Add(productQuantityEntity);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Hiển thị thông báo thành công
            TempData["success"] = "Thêm số lượng sản phẩm thành công";
            return RedirectToAction("AddQuantity", "SanPham", new { Id = productQuantityDTO.SanPhamId });
        }


    }
}
