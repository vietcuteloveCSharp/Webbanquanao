using DAL.Entities;
using DTO.VuvietanhDTO.NhanViens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebView.Areas.Admin.ViewModels;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize()]
    public class ShiftController : Controller
    {
       
        public ShiftController()
        {
        }
        public async Task<List<NgayLamViec>> GetDayWork()
        {
            List<NgayLamViec> list = new List<NgayLamViec>() 
            {
                new NgayLamViec
                {
                    Id = 1,
                    Ngay = DateTime.Now,
                    IsNgayNghi = false,
                    GhiChu = "Ngày làm việc bình thường"
                },
                new NgayLamViec
                {
                    Id = 2,
                    Ngay = DateTime.Now.AddDays(1),
                    IsNgayNghi = false,
                    GhiChu = "Ngày làm việc bình thường"
                },

            };

            return list;
        }
        public async Task<List<CaLamViec>> GetShift()
        {
            List<CaLamViec> list = new List<CaLamViec>()
            {
                new CaLamViec
                {
                    Id = 1,
                    TenCa = Enum.EnumVVA.EnumTenCa.CaSang,
                    GioBatDau = new TimeSpan(8,0,0),
                    GioKetThuc = new TimeSpan(12,0,0)
                },
                new CaLamViec
                {
                    Id = 2,
                    TenCa = Enum.EnumVVA.EnumTenCa.CaChieu,
                    GioBatDau = new TimeSpan(13,0,0),
                    GioKetThuc = new TimeSpan(18,0,0)
                },
            };
            return list;
        }
        public async Task<List<CaLamViec_NgayLamViec_NhanVien>> GetShiftEmployeeByDayWorkId(int Id_NgayLamViec)
        {
            List<CaLamViec_NgayLamViec_NhanVien> list = new List<CaLamViec_NgayLamViec_NhanVien>()
            {
                new CaLamViec_NgayLamViec_NhanVien
                {
                    Id = 1,
                    Id_CaLamViec = 1,
                    Id_NgayLamViec = 1,
                    Id_NhanVien = 1
                },
                new CaLamViec_NgayLamViec_NhanVien
                {
                    Id = 1,
                    Id_CaLamViec = 2,
                    Id_NgayLamViec = 1,
                    Id_NhanVien = 2
                },
            };
            return list;
        }
        
        public async Task<NhanVienProfileDTO> GetNhanVienById(int id)
        {
            NhanVienProfileDTO result = new NhanVienProfileDTO();
            using (HttpClient client = new HttpClient())
            {
                
                client.BaseAddress = new Uri("AppSettings:BaseApiAddress/NhanVien/Get_NhanVien_ByID");
                var response = await client.GetAsync($"AppSettings:BaseApiAddress/NhanVien/Get_NhanVien_ByID ={id}");
                if (response.IsSuccessStatusCode)
                {
                    var nhanVien = await response.Content.ReadAsStringAsync();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<NhanVienProfileDTO>(nhanVien);
                }
                else
                {
                    throw new Exception("Nhân viên không tồn tại");
                }
            }
            return result;
        }
        public async Task<List<DayWorkViewModel>> LoadData()
        {
            List<DayWorkViewModel> list = new List<DayWorkViewModel>();
            var dayWorks = await GetDayWork();
            var shifts = await GetShift();
            foreach (var dayWork in dayWorks)
            {
                var shiftEmployees = await GetShiftEmployeeByDayWorkId(dayWork.Id);
                var shiftViews = new List<ShiftView>();
                foreach (var shift in shifts)
                {
                    var shiftView = new ShiftView
                    {
                        Id = shift.Id,
                        TenCa = shift.TenCa,
                        GioBatDau = shift.GioBatDau,
                        GioKetThuc = shift.GioKetThuc,
                        NhanViens = new List<NhanVienProfileDTO>()
                    };
                    foreach (var shiftEmployee in shiftEmployees)
                    {
                        if (shiftEmployee.Id_CaLamViec == shift.Id && shiftEmployee.Id_NgayLamViec == dayWork.Id)
                        {
                            var nhanVien = await GetNhanVienById(shiftEmployee.Id_NhanVien);
                            shiftView.NhanViens.Add(nhanVien);
                        }
                    }
                    shiftViews.Add(shiftView);
                }
                list.Add(new DayWorkViewModel
                {
                    Id = dayWork.Id,
                    Ngay = dayWork.Ngay,
                    IsNgayNghi = dayWork.IsNgayNghi,
                    GhiChu = dayWork.GhiChu,
                    ShiftViews = shiftViews
                });
            }
            return list;
        }
        public IActionResult ShiftView()
        {
            return View();
        }
    }
}
