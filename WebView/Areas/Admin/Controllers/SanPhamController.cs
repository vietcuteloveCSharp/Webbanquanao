using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
                    Gia = group.First().SanPham.Gia,  // Lấy giá từ sản phẩm đầu tiên
                    TenDanhMuc = group.First().SanPham.DanhMuc?.TenDanhMuc,  // Lấy tên danh mục
                    TenThuongHieu = group.First().SanPham.ThuongHieu?.Ten,  // Lấy tên thương hiệu
                    SoLuong = group.Sum(ct => ct.SoLuong),  // Tổng số lượng của sản phẩm
                                                            // Gộp các màu sắc và kích thước
                    MaHex = string.Join(", ", group.Select(ct => ct.MauSac.MaHex).Distinct()), // Gộp tên màu sắc
                    TenKichThuoc = string.Join(", ", group.Select(ct => ct.KichThuoc.Ten).Distinct()), // Gộp tên kích thước
                                                                                                       // Lấy danh sách hình ảnh từ bảng HinhAnhs
                    HinhAnhList = _context.HinhAnhs
                .Where(ha => ha.Id_SanPham == group.Key) // Lọc hình ảnh theo Id_SanPham
                .Select(ha => ha.Url) // Chỉ lấy cột đường dẫn hình ảnh
                .ToList()
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
        public async Task<IActionResult> Create(SanPhamDTO sanPhamDTO, List<string>? ImageUrls)
        {

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Kiểm tra tên sản phẩm có bị trùng hay không
                    var existingProduct = await _context.SanPhams
                        .FirstOrDefaultAsync(sp => sp.Ten.ToLower() == sanPhamDTO.Ten.ToLower());

                    if (existingProduct != null)
                    {
                        ModelState.AddModelError("Ten", "Tên sản phẩm đã tồn tại.");
                        PopulateDropDownLists(sanPhamDTO);
                        ViewBag.ImageUrls = ImageUrls;
                        return View(sanPhamDTO);
                    }
                    ModelState.Clear();

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
                    _context.SanPhams.Add(sanPham);
                    await _context.SaveChangesAsync();

                    // Xử lý nhiều ảnh từ URL
                    if (ImageUrls != null && ImageUrls.Any())
                    {
                        foreach (var imageUrl in ImageUrls)
                        {
                            if (!string.IsNullOrEmpty(imageUrl) &&
                                Uri.TryCreate(imageUrl, UriKind.Absolute, out Uri? uriResult) &&
                                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                            {
                                _context.HinhAnhs.Add(new HinhAnh
                                {
                                    Id_SanPham = sanPham.Id,
                                    Url = imageUrl,
                                    ImageSourceType = 1 // Ảnh từ URL
                                });
                            }
                            else
                            {
                                ModelState.AddModelError("ImageUrls", $"URL '{imageUrl}' không hợp lệ.");
                            }
                        }
                    }

                    // Lưu danh sách ChiTietSanPham
                    if (sanPhamDTO.ChiTietSanPhams != null && sanPhamDTO.ChiTietSanPhams.Any())
                    {
                        foreach (var chiTiet in sanPhamDTO.ChiTietSanPhams)
                        {
                            if (chiTiet.Id_MauSac > 0 && chiTiet.Id_KichThuoc > 0 && chiTiet.SoLuong > 0)
                            {
                                _context.ChiTietSanPhams.Add(new ChiTietSanPham
                                {
                                    Id_SanPham = sanPham.Id,
                                    Id_MauSac = chiTiet.Id_MauSac,
                                    Id_KichThuoc = chiTiet.Id_KichThuoc,
                                    SoLuong = chiTiet.SoLuong,
                                    TrangThai = chiTiet.TrangThai ?? true,
                                    NgayTao = DateTime.Now
                                });
                            }
                            else
                            {
                                ModelState.AddModelError("ChiTietSanPhams", "Thông tin chi tiết sản phẩm không hợp lệ.");
                            }
                        }
                    }

                    if (!ModelState.IsValid)
                    {
                        // Nếu có lỗi trong ModelState, rollback và trả lại view
                        await transaction.RollbackAsync();
                        PopulateDropDownLists(sanPhamDTO);
                        return View(sanPhamDTO);
                    }
                    ModelState.Clear();
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["success"] = "Thêm sản phẩm và thuộc tính thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    TempData["error"] = $"Có lỗi xảy ra: {ex.Message}";
                }
            }

            // Nếu có lỗi trong ModelState hoặc không hợp lệ, trả về view với dữ liệu hiện tại
            TempData["error"] = "Có lỗi xảy ra khi thêm sản phẩm.";
            PopulateDropDownLists(sanPhamDTO);
            ViewBag.ImageUrls = ImageUrls; // Gửi lại danh sách ImageUrls cho view
            return View(sanPhamDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var sanPham = await _context.SanPhams
                .Include(sp => sp.ChiTietSanPhams)
                .Include(sp => sp.HinhAnhs)
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (sanPham == null)
                return NotFound();

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
                HinhAnhs = sanPham.HinhAnhs.Select(ha => new HinhAnhDTO
                {
                    Id = ha.Id,
                    Id_SanPham = ha.Id_SanPham,
                    Url = ha.Url,
                    ImageSourceType = 0
                }).ToList(),
                ChiTietSanPhams = sanPham.ChiTietSanPhams.Select(ct => new ChiTietSanPhamDTO
                {
                    Id_MauSac = ct.Id_MauSac,
                    Id_KichThuoc = ct.Id_KichThuoc,
                    SoLuong = ct.SoLuong,
                    TrangThai = ct.TrangThai
                }).ToList()
            };


            PopulateDropDownLists(sanPhamDTO);
            return View(sanPhamDTO);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var hinhAnh = await _context.HinhAnhs.FirstOrDefaultAsync(ha => ha.Id_SanPham == id);
            if (hinhAnh == null)
                return NotFound();

            _context.HinhAnhs.Remove(hinhAnh);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SanPhamDTO model, List<string> ImageUrls, List<int> DeletedImageIds)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Có lỗi xảy ra khi cập nhật sản phẩm.";
                PopulateDropDownLists(model);
                return View(model);
            }

            try
            {
                // Lấy sản phẩm hiện tại từ cơ sở dữ liệu
                var sanPham = await _context.SanPhams
                    .Include(sp => sp.ChiTietSanPhams)
                    .Include(sp => sp.HinhAnhs)
                    .FirstOrDefaultAsync(sp => sp.Id == model.Id);

                if (sanPham == null)
                    return NotFound();

                // Cập nhật thông tin sản phẩm chính
                sanPham.Ten = model.Ten;
                sanPham.MoTa = model.MoTa;
                sanPham.Gia = model.Gia;
                sanPham.SoLuong = model.SoLuong;
                sanPham.NgayTao = model.NgayTao;
                sanPham.Id_ThuongHieu = model.Id_ThuongHieu;
                sanPham.Id_DanhMuc = model.Id_DanhMuc;

                // Xóa ảnh theo danh sách DeletedImageIds
                if (DeletedImageIds != null && DeletedImageIds.Any())
                {
                    var imagesToDelete = sanPham.HinhAnhs.Where(ha => DeletedImageIds.Contains(ha.Id)).ToList();
                    _context.HinhAnhs.RemoveRange(imagesToDelete);
                }

                // Thêm các hình ảnh mới từ danh sách URL
                if (ImageUrls != null && ImageUrls.Any())
                {
                    foreach (var imageUrl in ImageUrls)
                    {
                        if (!string.IsNullOrEmpty(imageUrl) && Uri.TryCreate(imageUrl, UriKind.Absolute, out Uri? uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                        {
                            // Kiểm tra xem ảnh đã tồn tại
                            if (sanPham.HinhAnhs.Any(ha => ha.Url == imageUrl))
                            {
                                ModelState.AddModelError("ImageUrls", $"Hình ảnh với URL '{imageUrl}' đã tồn tại.");
                                continue;
                            }

                            // Thêm ảnh mới nếu không trùng
                            sanPham.HinhAnhs.Add(new HinhAnh
                            {
                                Id_SanPham = sanPham.Id,
                                Url = imageUrl,
                                ImageSourceType = 1 // Ảnh từ URL
                            });
                        }
                        else
                        {
                            // URL không hợp lệ
                            ModelState.AddModelError("ImageUrls", $"URL '{imageUrl}' không hợp lệ.");
                        }
                    }
                }

                // Kiểm tra nếu có lỗi trong ModelState
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Có lỗi xảy ra khi cập nhật sản phẩm.";
                    PopulateDropDownLists(model);
                    return View(model);
                }


                // Cập nhật hoặc thêm mới chi tiết sản phẩm
                if (model.ChiTietSanPhams != null)
                {
                    foreach (var chiTiet in model.ChiTietSanPhams)
                    {
                        var existingDetail = sanPham.ChiTietSanPhams
                            .FirstOrDefault(ct => ct.Id_MauSac == chiTiet.Id_MauSac && ct.Id_KichThuoc == chiTiet.Id_KichThuoc);

                        if (existingDetail != null)
                        {
                            // Cập nhật số lượng và trạng thái
                            existingDetail.SoLuong = chiTiet.SoLuong;
                            existingDetail.TrangThai = chiTiet.TrangThai ?? true;
                        }
                        else
                        {
                            // Thêm mới chi tiết sản phẩm
                            sanPham.ChiTietSanPhams.Add(new ChiTietSanPham
                            {
                                Id_SanPham = sanPham.Id,
                                Id_MauSac = chiTiet.Id_MauSac,
                                Id_KichThuoc = chiTiet.Id_KichThuoc,
                                SoLuong = chiTiet.SoLuong,
                                TrangThai = chiTiet.TrangThai ?? true,
                                NgayTao = DateTime.Now
                            });
                        }
                    }
                }

                // Lưu thay đổi
                await _context.SaveChangesAsync();
                TempData["success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException dbEx)
            {
                TempData["error"] = "Đã xảy ra lỗi khi cập nhật sản phẩm: " + dbEx.Message;
                PopulateDropDownLists(model);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Đã xảy ra lỗi khi cập nhật sản phẩm: " + ex.Message;
                PopulateDropDownLists(model);
                return View(model);
            }
        }


        // Tách xử lý hình ảnh URL
        private void HandleImageUrl(SanPham sanPham, string imageUrl)
        {
            sanPham.HinhAnhs.Add(new HinhAnh
            {
                Id_SanPham = sanPham.Id,
                Url = imageUrl,
                ImageSourceType = 1 // Ảnh từ URL
            });
        }

        // Tách cập nhật chi tiết sản phẩm
        private void UpdateChiTietSanPhams(SanPham sanPham, List<ChiTietSanPhamDTO> chiTietDtos)
        {
            if (chiTietDtos == null) return;

            // Xóa chi tiết không còn trong danh sách
            var chiTietIds = chiTietDtos
                .Select(dto => (dto.Id_MauSac, dto.Id_KichThuoc))
                .ToHashSet();
            // Xóa chi tiết không còn trong danh sách
            foreach (var chiTiet in sanPham.ChiTietSanPhams
                .Where(ct => !chiTietIds.Contains((ct.Id_MauSac, ct.Id_KichThuoc)))
                .ToList()) // Tạo danh sách tạm để xóa
            {
                sanPham.ChiTietSanPhams.Remove(chiTiet);
            }

            // Cập nhật hoặc thêm mới
            foreach (var dto in chiTietDtos)
            {
                var existingDetail = sanPham.ChiTietSanPhams
                    .FirstOrDefault(ct => ct.Id_MauSac == dto.Id_MauSac && ct.Id_KichThuoc == dto.Id_KichThuoc);

                if (existingDetail != null)
                {
                    // Cập nhật chi tiết sản phẩm
                    existingDetail.SoLuong = dto.SoLuong;
                    existingDetail.TrangThai = dto.TrangThai ?? true;
                }
                else
                {
                    // Thêm mới chi tiết sản phẩm
                    sanPham.ChiTietSanPhams.Add(new ChiTietSanPham
                    {
                        Id_SanPham = sanPham.Id,
                        Id_MauSac = dto.Id_MauSac,
                        Id_KichThuoc = dto.Id_KichThuoc,
                        SoLuong = dto.SoLuong,
                        TrangThai = dto.TrangThai ?? true,
                        NgayTao = DateTime.Now
                    });
                }
            }
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
            var productDetails = await _context.ChiTietSanPhams
                .Include(ct => ct.MauSac)
                .Include(ct => ct.KichThuoc)
                .Where(ct => ct.Id_SanPham == id)
                .ToListAsync();

            if (!productDetails.Any())
            {
                return NotFound();
            }

            ViewBag.ChiTietSanPhams = productDetails;
            ViewBag.ProductByQuantity = productDetails;
            ViewBag.Id = id;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StoreProductQuantity(ChiTietSanPhamDTO productQuantityDTO)
        {
            if (productQuantityDTO.SoLuong <= 0)
            {
                TempData["error"] = "Số lượng phải lớn hơn 0.";
                return RedirectToAction("AddQuantity", new { id = productQuantityDTO.Id_SanPham });
            }

            var product = await _context.SanPhams.FindAsync(productQuantityDTO.Id_SanPham);
            if (product == null)
            {
                return NotFound();
            }

            product.SoLuong += productQuantityDTO.SoLuong;

            var chiTietSanPham = await _context.ChiTietSanPhams
                .FirstOrDefaultAsync(ct => ct.Id_SanPham == productQuantityDTO.Id_SanPham &&
                                           ct.Id_MauSac == productQuantityDTO.Id_MauSac &&
                                           ct.Id_KichThuoc == productQuantityDTO.Id_KichThuoc);

            if (chiTietSanPham != null)
            {
                chiTietSanPham.SoLuong += productQuantityDTO.SoLuong;
            }
            else
            {
                var newProductQuantity = new ChiTietSanPham
                {
                    Id_SanPham = productQuantityDTO.Id_SanPham,
                    SoLuong = productQuantityDTO.SoLuong,
                    Id_MauSac = productQuantityDTO.Id_MauSac,
                    Id_KichThuoc = productQuantityDTO.Id_KichThuoc,
                    NgayTao = DateTime.Now
                };

                _context.Add(newProductQuantity);
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Thêm số lượng sản phẩm thành công";
            return RedirectToAction("AddQuantity", "SanPham", new { Id = productQuantityDTO.Id_SanPham });
        }



    }
}
