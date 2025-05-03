    using DAL.Context;
    using DAL.Entities;
    using DTO.VuvietanhDTO.Calamviecs;
    using DTO.VuvietanhDTO.Canhanviens;
    using DTO.VuvietanhDTO.Chucvus;
    using DTO.VuvietanhDTO.Ngaylamviecs;
    using DTO.VuvietanhDTO.NhanViens;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using WebView.Areas.Admin.ViewModels;


    namespace WebView.Areas.Admin.Controllers
    {
        [Area("Admin")]
        [Authorize(Roles = "admin")]

        public class ShiftController : Controller
        { 
            private readonly WebBanQuanAoDbContext _context;

            public ShiftController(WebBanQuanAoDbContext webBanQuanAoDbContext)
            {
                _context = webBanQuanAoDbContext;
            }
            public async Task<List<NgayLamViecDTO>> GetAllNgayLamViec()
            {
                List<NgayLamViecDTO> result = new List<NgayLamViecDTO>();

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://localhost:7169/api/NgaylamViec/Get-All-NgayLamViec");
                    if (response.IsSuccessStatusCode)
                    {
                        var responcontent = await response.Content.ReadAsStringAsync();
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NgayLamViecDTO>>(responcontent);
                        return result;
                    }
                }
                return result;
            }
            public async Task<List<CaLamViecDTO>> GetCaLamViecByIdNgayLamViec(int id)
            {
                List<CaLamViecDTO> result = new List<CaLamViecDTO>();
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7169/api/Calamviec/Get_By_IdNgayLamViec/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responcontent = await response.Content.ReadAsStringAsync();
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CaLamViecDTO>>(responcontent);
                        return result;
                    }
                }
                return result;
            }
            public async Task<List<NhanvienDTO>> GetNhanVienByCa(int id)
            {
                List<NhanvienDTO> result = new List<NhanvienDTO>();
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7169/api/CaNhanVien/get-nhanviens-by-ca/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responcontent = await response.Content.ReadAsStringAsync();
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NhanvienDTO>>(responcontent);
                        return result;
                    }
                }
                return result;
            }
            public async Task<ChucvuDTO> GetChucVuById(int id)
            {
                ChucvuDTO result = new ChucvuDTO();
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7169/api/Chucvu/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responcontent = await response.Content.ReadAsStringAsync();
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<ChucvuDTO>(responcontent);
                        return result;
                    }
                }
                return result;
            }
            public async Task<List<Schedule>> GetSchedules()
            {   
                List<Schedule> schedules = new List<Schedule>();
                var ngayLamViecs = await GetAllNgayLamViec();
                foreach (var item in ngayLamViecs)
                {
                    List<Shift> Listshifts = new List<Shift>();
                    List<CaLamViecDTO> shifts = await GetCaLamViecByIdNgayLamViec(item.Id);
                    foreach (var shift in shifts)
                    {
                        List<Employee> ListEmployees = new List<Employee>();
                        List<NhanvienDTO> nhanViens = await GetNhanVienByCa(shift.Id);

                        foreach (var nhanVien in nhanViens)
                        {
                            Employee employee = new Employee
                            {
                                Id = nhanVien.Id,
                                Name = nhanVien.TenNhanVien,
                                Role = GetChucVuById(nhanVien.Id_ChucVu).Result.Ten
                            };
                            ListEmployees.Add(employee);
                        }
                        Shift shiftItem = new Shift
                        {
                            Id = shift.Id,
                            ShiftName = shift.TenCa.ToString(),
                            StartTime = shift.GioBatDau,
                            EndTime = shift.GioKetThuc,
                            Employees = ListEmployees
                        };
                        Listshifts.Add(shiftItem);
                    }
             
                    schedules.Add(new Schedule
                    {
                        Id_NgayLamViec = item.Id,
                        status = item.IsNgayNghi,
                        Date = item.Ngay,
                        Shifts = Listshifts
                    });

                }
                return schedules;
            }
            public async Task<IActionResult> Index()
            {
                List<Schedule> Model = await GetSchedules();
                Model= Model.Where(x => x.status == false).ToList();
                if (!Model.Any())
                {
                    ModelState.AddModelError(string.Empty, "Không có bất kỳ lịch làm việc nào");
                    return View();
                }
                return View(Model);
            }
        private async Task LoadViewBagData()
        {
            try
            {
                ViewBag.Cashiers = await _context.NhanViens
                    .Include(nv => nv.ChucVu)
                    .Where(e => e.ChucVu != null && e.ChucVu.Ten == "Thu ngân")
                    .Select(e => new { Id = e.Id, Name = e.TenNhanVien })
                    .ToListAsync();

                ViewBag.SalesStaff = await _context.NhanViens
                    .Include(nv => nv.ChucVu)
                    .Where(e => e.ChucVu != null && e.ChucVu.Ten == "Nhân viên bán hàng")
                    .Select(e => new { Id = e.Id, Name = e.TenNhanVien })
                    .ToListAsync();
            }
            catch
            {
                ViewBag.Cashiers = new List<dynamic>();
                ViewBag.SalesStaff = new List<dynamic>();
            }
        }
            [HttpGet]
            public async Task<IActionResult> CreateSchedule()
            {
                await LoadViewBagData();

                return View();
            }

            [HttpPost]
            public async Task<IActionResult> CreateSchedule(
                DateTime workDate,
                bool status,
                List<int> morningShift,
                List<int> afternoonShift,
                List<int> nightShift)
            {
            var ngaylamviecExits = await _context.NgayLamviecs.FirstOrDefaultAsync(x => x.Ngay == workDate);
            if (ngaylamviecExits!=null)
            {
                ModelState.AddModelError(string.Empty, $"Ngày {workDate} đã được lên lịch ");
                 await LoadViewBagData();
                return View();
            }
            _context.NgayLamviecs.Add(new NgayLamViec
                {
                    Ngay = workDate,
                    IsNgayNghi = true,
                    GhiChu = "Lịch làm việc"
                });
                _context.SaveChanges();
            _context.CaLamViecs.Add(new CaLamViec
                {
                    IdNgaylamviec = _context.NgayLamviecs.FirstOrDefault(x => x.Ngay == workDate).Id,
                    TenCa = Enum.EnumVVA.EnumTenCa.CaSang,
                    TrangThai = true,
                    GioBatDau = new TimeSpan(08, 00, 00),
                    GioKetThuc = new TimeSpan(12, 00, 00),
                });
                _context.CaLamViecs.Add(new CaLamViec
                {
                    IdNgaylamviec = _context.NgayLamviecs.FirstOrDefault(x => x.Ngay == workDate).Id,
                    TenCa = Enum.EnumVVA.EnumTenCa.CaChieu,
                    TrangThai = true,
                    GioBatDau = new TimeSpan(13, 00, 00),
                    GioKetThuc = new TimeSpan(17, 00, 00),
                });
                _context.CaLamViecs.Add(new CaLamViec
                {
                    IdNgaylamviec = _context.NgayLamviecs.FirstOrDefault(x => x.Ngay == workDate).Id,
                    TenCa = Enum.EnumVVA.EnumTenCa.CaToi,
                    TrangThai= true,
                    GioBatDau = new TimeSpan(18, 00, 00),
                    GioKetThuc = new TimeSpan(22, 00, 00),
                });
            _context.SaveChanges();
            // Lưu các ca làm việc vào cơ sở dữ liệu
            foreach (var id in morningShift)
                {
                    var caLamViec = _context.CaLamViecs.FirstOrDefault(x => x.TenCa == Enum.EnumVVA.EnumTenCa.CaSang && x.IdNgaylamviec == _context.NgayLamviecs.FirstOrDefault(x => x.Ngay == workDate).Id);
                    if (caLamViec != null)
                    {
                        _context.Canhanviens.Add(new CaNhanVien
                        {
                            IdCaLamViec = caLamViec.Id,
                            IdNhanVien = id
                        });
                    }
                }
                foreach (var id in afternoonShift)
                {
                    var caLamViec = _context.CaLamViecs.FirstOrDefault(x => x.TenCa == Enum.EnumVVA.EnumTenCa.CaChieu && x.IdNgaylamviec == _context.NgayLamviecs.FirstOrDefault(x => x.Ngay == workDate).Id);
                    if (caLamViec != null)
                    {
                        _context.Canhanviens.Add(new CaNhanVien
                        {
                            IdCaLamViec = caLamViec.Id,
                            IdNhanVien = id
                        });
                    }
                }
                foreach (var id in nightShift)
                {
                    var caLamViec = _context.CaLamViecs.FirstOrDefault(x => x.TenCa == Enum.EnumVVA.EnumTenCa.CaToi && x.IdNgaylamviec == _context.NgayLamviecs.FirstOrDefault(x => x.Ngay == workDate).Id);
                    if (caLamViec != null)
                    {
                        _context.Canhanviens.Add(new CaNhanVien
                        {
                            IdCaLamViec = caLamViec.Id,
                            IdNhanVien = id
                        });
                    }
                }
                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        public async Task<IActionResult> Details(int id)
        {
            var schedule = await _context.NgayLamviecs
                .Include(n => n.CaLamViecs)
                .ThenInclude(c => c.Canhanviens)
                .ThenInclude(cn => cn.NhanVien)
                .ThenInclude(n => n.ChucVu)
                .Where(n => n.Id == id)
                .Select(n => new Schedule
                {
                    Id_NgayLamViec = n.Id,
                    Date = n.Ngay,
                    status = n.IsNgayNghi,
                    Shifts = n.CaLamViecs.Select(c => new Shift
                    {
                        Id = c.Id,
                        ShiftName = c.TenCa.ToString(),
                        StartTime = c.GioBatDau,
                        EndTime = c.GioKetThuc,
                        Employees = c.Canhanviens.Select(cn => new Employee
                        {
                            Id = cn.NhanVien.Id,
                            Name = cn.NhanVien.TenNhanVien,
                            Role = cn.NhanVien.ChucVu.Ten
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }


    }

}
