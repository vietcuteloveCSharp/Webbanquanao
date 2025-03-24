using Bogus.DataSets;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Net.Http;
using WebView.Areas.Admin.ViewModels;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShiftController : Controller
    {
        private readonly HttpClient _httpClient;
        public ShiftController(HttpClient httpClient)
        {
            _httpClient = httpClient;   
        }
        private async Task<List<Shift>> GetShiftsForDate(string date)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Shift>>($"/api/CalamViec/{date}");
                return response ?? new List<Shift>();
            }
            catch
            {
                return new List<Shift>(); // Trả về danh sách trống nếu có lỗi
            }
        }
        public DateTime GetMonday(DateTime d)
        {
            DateTime currentDay = d;
            DateTime monday;
            if (currentDay.DayOfWeek == DayOfWeek.Sunday)
            {
                 monday = currentDay.AddDays(-6);
            }
            else
            {
                monday = currentDay.AddDays(-(int)currentDay.DayOfWeek + 1);
            }
            return monday;
        }
         
        public async Task<IActionResult> ShiftView()
        {
            var weekData = new List<DayShifts>();
            int weekOffSet = 0; // Tuần hiện tại

            // Tính ngày Thứ Hai và Chủ Nhật của tuần hiện tại
                var monday= GetMonday(DateTime.Now).AddDays(7*weekOffSet);
                var sunday = monday.AddDays(6); // Ngày Chủ Nhật

            // Lặp qua từng ngày trong tuần để gọi API
            for (var date = monday; date <= sunday; date = date.AddDays(1))
            {
                var formattedDate = date.ToString("yyyy-MM-dd");
                var shifts = await GetShiftsForDate(formattedDate);

                weekData.Add(new DayShifts
                {
                    Date = date,
                    Shifts = shifts
                });
            }

            return View(weekData);

        }
    }
}
