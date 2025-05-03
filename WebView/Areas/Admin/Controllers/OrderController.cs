using System;
using DAL.Context;
using DAL.Entities;
using DTO.NTTuyen.ChiTietHoaDon;
using DTO.NTTuyenDTO.ChiTietSanPhams;
using DTO.VuvietanhDTO.HoadonsDTO;
using DTO.VuvietanhDTO.KhachHangs;
using DTO.VuvietanhDTO.Sanphams;
using Enum.EnumVVA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static WebView.Areas.Admin.ViewModels.ViewHoaDon;
using Microsoft.AspNetCore.Authorization;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="admin")]
    public class OrderController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly WebBanQuanAoDbContext _context;

        private List<HoaDonView> listHoaDonView;
        private const string ApiUri = "https://localhost:7169/api";
        public OrderController(WebBanQuanAoDbContext dbcontext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = dbcontext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            this.listHoaDonView = new List<HoaDonView>();
        }
        [HttpGet("/Hoadon/Get-All-HoaDon")]
        public async Task<List<FullHoaDonDTO>> GetListHoaDon()
        {

            List<FullHoaDonDTO> result = new List<FullHoaDonDTO>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(ApiUri + "/Hoadon/Get-All-HoaDon");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<FullHoaDonDTO>>(responseContent);
                }
            }

            return result;
        }
        [HttpGet("khachhang/{id}")]
        public async Task<KhachhangDTO> GetKhachHangById(int id)
        {
            KhachhangDTO result = new KhachhangDTO();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(ApiUri+"/Khachhang/"+id);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<KhachhangDTO>(responseContent);
                }
            }
            return result;
        }
        [HttpGet("sanpham/{id}")]
        public async Task<SanPhamDTO> GetSanPhamById(int id)
        {
            SanPhamDTO result = new SanPhamDTO();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(ApiUri+"/Sanpham/"+ id);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<SanPhamDTO>(responseContent);
                }
            }
            return result;
        }
        
        [HttpGet("chitietsanpham/{id}")]
        public async Task<ChiTietSanPhamDTO> GetChiTietSanPhamById(int id)
        {
            ChiTietSanPhamDTO result = new ChiTietSanPhamDTO();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(ApiUri+"/Chitietsanpham/"+ id);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChiTietSanPhamDTO>(responseContent);
                }
            }
            return result;
        }
        [HttpGet("chitiethoadon/hoadon/id")]
        public async Task<List<ChiTietHoaDonDTO>> GetChiTietHoaDonByHoaDonId(int id)
        {
            List<ChiTietHoaDonDTO> result = new List<ChiTietHoaDonDTO>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(ApiUri+"/Chitiethoadon/hoadon/"+ id);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<ChiTietHoaDonDTO>>(responseContent);
                }
            }
            return result;
        }
        // ...

        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(HoaDonView hoadonView)
        {
            if (hoadonView == null)
            {
                // Trả về lỗi nếu đối tượng null
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(ApiUri);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response = await client.PutAsJsonAsync($"{ApiUri}/Hoadon/{hoadonView.Id}/update-trangthai-hoadon?nextTrangThai={hoadonView.TrangThai}", new { status = hoadonView.TrangThai });

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Cập nhật trạng thái đơn hàng thành công!";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            string errorMessage = await response.Content.ReadAsStringAsync();
                            TempData["ErrorMessage"] = $"{errorMessage}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Đã có lỗi xảy ra: {ex.Message}";
                }
            }

            // Nếu ModelState không hợp lệ, trả về lại view
            return View("OrderDetail", hoadonView);
        }


        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int id)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PutAsJsonAsync($"{ApiUri}/Hoadon/{id}/update-trangthai-hoadon?nextTrangThai={6}",6);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Hủy đơn hàng thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(response.StatusCode.ToString(), "Lỗi Hủy");
                }
            }
           return RedirectToAction("Index");
        }
        public async Task<List<HoaDonView>> LoadData()
        {
            List<FullHoaDonDTO> listHoaDon = await GetListHoaDon();
            foreach (var hd in listHoaDon)
            {
                // Khai báo thuộc tính khách hàng trong HoaDonView
                KhachhangDTO khachhang = await GetKhachHangById(hd.Id_KhachHang);
                KhachHangView khachHangView = new KhachHangView
                {
                    Id = hd.Id_KhachHang,
                    Ten = khachhang.Ten,
                    Sdt = khachhang.Sdt
                };

                // Lấy các chi tiết hóa đơn theo id hóa đơn
                List<ChiTietHoaDonDTO> listChiTietHoaDon = await GetChiTietHoaDonByHoaDonId(hd.Id);

                // Tạo một từ điển để nhóm sản phẩm theo tên và tính tổng số lượng
                Dictionary<string, int> groupedProducts = new Dictionary<string, int>();

                // Hoàn thiện các thuộc tính SanPham trong HoaDonView
                foreach (var cthd in listChiTietHoaDon)
                {
                    // Lấy chi tiết sản phẩm
                    ChiTietSanPhamDTO chiTietSanPham = await GetChiTietSanPhamById(cthd.Id_ChiTietSanPham);
                    SanPhamDTO sanpham = await GetSanPhamById(chiTietSanPham.Id_SanPham);

                    // Kiểm tra nếu sản phẩm đã tồn tại trong nhóm
                    if (groupedProducts.ContainsKey(sanpham.Ten))
                    {
                        groupedProducts[sanpham.Ten] += cthd.SoLuong;
                    }
                    else
                    {
                        groupedProducts[sanpham.Ten] = cthd.SoLuong;
                    }
                }

                // Chuyển đổi từ điển thành danh sách các SanPhamView
                List<SanPhamView> lstsanphamview = groupedProducts.Select(kv => new SanPhamView
                {
                    Ten = kv.Key,
                    SoLuong = kv.Value
                }).ToList();

                // Tạo đối tượng HoaDonView và thêm vào danh sách
                listHoaDonView.Add(new HoaDonView
                {
                    Id = hd.Id,
                    TongTien = hd.TongTien,
                    PhiVanChuyen = hd.PhiVanChuyen,
                    DiaChiGiaoHang = hd.DiaChiGiaoHang,
                    NgayTao = hd.NgayTao,
                    TrangThai = hd.TrangThai,
                    KhachHangs = khachHangView,
                    SanPhams = lstsanphamview,
                });
            }
            return listHoaDonView;
        }

        ///Phương thức trả về cho view một list viewmodel của hóa đơn
        public async Task<IActionResult> Index()
        {
             listHoaDonView = await LoadData();
             listHoaDonView = listHoaDonView.Where(x => x.TrangThai != ETrangThaiHD.HuyDon ).ToList();
             listHoaDonView = listHoaDonView.Where(x => x.TrangThai != ETrangThaiHD.HoanThanhDon ).ToList();
             listHoaDonView = listHoaDonView.OrderBy(x => x.NgayTao ).ToList();

            if (!listHoaDonView.Any())
            {
                ModelState.AddModelError(string.Empty, "Không có bất kỳ đơn hàng nào 1");
                return View();
            }
            else
            {
                return View(listHoaDonView);
            }
        }   
        [HttpPost("FilterStatus")]
        public async Task<IActionResult> FilterStatus(int filterorder)
        {
            // Tải danh sách hóa đơn
            listHoaDonView = await LoadData();

            // Kiểm tra nếu trạng thái là "Tất cả"
            if (filterorder == ETrangThaiHD.None.GetHashCode()) // Giả sử `None` là trạng thái tương ứng với "Tất cả"
            {
                ModelState.AddModelError(string.Empty, "Không có bất kỳ đơn hàng nào.");
                return View("Index");
            }
            if (filterorder != 0)
            {
                //ModelState.AddModelError(string.Empty, $"Không tìm thấy hóa đơn với trạng thái khac 0 {(ETrangThaiHD)FilterStatus}.");
                listHoaDonView = listHoaDonView.Where(x => x.TrangThai.GetHashCode() == filterorder).ToList();
                return View("Index", listHoaDonView);
            }

            // Lọc danh sách theo trạng thái
            var filteredList = listHoaDonView.Where(x => x.TrangThai.GetHashCode() == filterorder).ToList();
            // Trả về View với danh sách được lọc
            return View("Index", filteredList);
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            // Giả sử LoadData() là một phương thức bất đồng bộ để lấy danh sách HoaDonView
            List<HoaDonView> listHoaDonView = await LoadData();

            if (listHoaDonView == null || !listHoaDonView.Any())
            {
                // Nếu danh sách rỗng hoặc null, trả về lỗi
                TempData["ErrorMessage"] = "Danh sách đơn hàng không có dữ liệu.";
                return RedirectToAction("Index"); // Hoặc chuyển hướng về trang khác nếu không có dữ liệu
            }

            // Tìm kiếm HoaDonView theo Id
            HoaDonView viewDetail = listHoaDonView.FirstOrDefault(x => x.Id == id);

            if (viewDetail == null)
            {
                // Nếu không tìm thấy đối tượng với id, trả về thông báo lỗi
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng với ID: " + id;
                return RedirectToAction("Index"); // Hoặc chuyển hướng về trang khác nếu không tìm thấy
            }

            // Truyền dữ liệu vào View
            return View(viewDetail);
        }




    }
}
