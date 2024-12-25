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
        // GET: Index (List all MaGiamGia)
        public IActionResult Index()
        {
            // Simulate fetching data from a database
            var mockData = new List<MaGiamGiaDTO>
            {
                new MaGiamGiaDTO
                {
                    Id = 1,
                    Ten = "Mã giảm giá 1",
                    LoaiGiamGia = 0,
                    DieuKienGiamGia = 100000,
                    GiaTriGiam = "10",
                    MenhGia = 0,
                    NoiDung = "Áp dụng cho đơn hàng trên 100k",
                    GiaTriToiDa = 50000,
                    TrangThai = 1,
                    ThoiGianTao = DateTime.Now,
                    ThoiGianKetThuc = DateTime.Now.AddDays(7)
                }
            };

            return View(mockData);
        }
    
    [HttpGet]

        // GET: Create
        public IActionResult Create()
        {
            return View(new MaGiamGiaDTO());
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaGiamGiaDTO model)
        {
            // Validate LoaiGiamGia logic
            if (model.LoaiGiamGia == 0) // Coupon
            {
                if (model.GiaTriGiam == null || decimal.Parse(model.GiaTriGiam) <= 0)
                {
                    ModelState.AddModelError("GiaTriGiam", "Giá trị giảm phải lớn hơn 0 với Coupon.");
                }

                if (model.MenhGia > 0)
                {
                    ModelState.AddModelError("MenhGia", "MenhGia không được sử dụng cho Coupon.");
                }
            }
            else if (model.LoaiGiamGia == 1) // Voucher
            {
                if (model.MenhGia <= 0)
                {
                    ModelState.AddModelError("MenhGia", "MenhGia là bắt buộc với Voucher và phải lớn hơn 0.");
                }

                if (!string.IsNullOrEmpty(model.GiaTriGiam))
                {
                    ModelState.AddModelError("GiaTriGiam", "GiaTriGiam không được sử dụng cho Voucher.");
                }
            }

            // Check DieuKienGiamGia logic
            if (model.LoaiGiamGia == 0 && model.DieuKienGiamGia < 0)
            {
                ModelState.AddModelError("DieuKienGiamGia", "Điều kiện giảm giá không được nhỏ hơn 0.");
            }

            // Check general logic
            if (model.ThoiGianKetThuc.HasValue && model.ThoiGianKetThuc <= model.ThoiGianTao)
            {
                ModelState.AddModelError("ThoiGianKetThuc", "Thời gian kết thúc phải sau thời gian tạo.");
            }

            // Return to View if any validation failed
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Save logic to add data to database
            var maGiamGiaEntity = new MaGiamGia
            {
                Ten = model.Ten,
                LoaiGiamGia = model.LoaiGiamGia,
                DieuKienGiamGia = model.DieuKienGiamGia,
                GiaTriGiam = model.GiaTriGiam,
                MenhGia = model.MenhGia,
                NoiDung = model.NoiDung,
                GiaTriToiDa = model.GiaTriToiDa,
                TrangThai = model.TrangThai,
                ThoiGianTao = model.ThoiGianTao,
                ThoiGianKetThuc = model.ThoiGianKetThuc
            };

            _context.MaGiamGias.Add(maGiamGiaEntity);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Mã giảm giá đã được tạo thành công!";
            return RedirectToAction("Index");
        }


    }
}
