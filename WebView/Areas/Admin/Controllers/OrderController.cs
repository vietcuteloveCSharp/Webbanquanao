using DAL.Entities;
using DTO.NTTuyen.ChiTietHoaDon;
using DTO.NTTuyenDTO.ChiTietSanPhams;
using DTO.VuvietanhDTO.HoadonsDTO;
using DTO.VuvietanhDTO.KhachHangs;
using DTO.VuvietanhDTO.Sanphams;
using Enum.EnumVVA;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static WebView.Areas.Admin.ViewModels.ViewHoaDon;

namespace WebView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private List<HoaDonView> listHoaDonView;
        private const string ApiUri = "https://localhost:7169/api";
        public OrderController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
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

        [HttpPost("Update-Order")]
        public async Task<IActionResult> UpdateOrder(HoaDonView hoadonView)
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
                   await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(response.StatusCode.ToString(), "Error");
                }
            }
            return View(hoadonView);
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

                // Khai báo thuộc tính khách hàng trong HoaDonView(Thuộc tính khách hàng là một đối tượng KhachHangView)
                KhachhangDTO khachhang = await GetKhachHangById(hd.Id_KhachHang);
                // Hoàn thiện thuộc tính khách hàng trong HoaDonView
                KhachHangView khachHangView = new KhachHangView
                {   
                    Id= hd.Id_KhachHang,
                    Ten = khachhang.Ten,
                    Sdt = khachhang.Sdt
                };


                // Khai báo thuộc tính sản phẩm trong HoaDonView (Sanpham là một list các đối tượng SanPhamview)
                List<SanPhamView> lstsanphamview = new List<SanPhamView>();

                // Lấy các chi tiết hóa đơn theo id hóa đơn
                List<ChiTietHoaDonDTO> listChiTietHoaDon = await GetChiTietHoaDonByHoaDonId(hd.Id);
                // hoàn thiện các thuộc tính SanPham trong HoaDonView
                foreach (var cthd in listChiTietHoaDon)
                {
                    //Lấy ra những chi tiết sản phẩm trong hóa đơn dựa vào chi tiết hóa đơn
                    ChiTietSanPhamDTO chiTietSanPham = await GetChiTietSanPhamById(cthd.Id_ChiTietSanPham);
                    // Lấy ra sản phẩm dựa vào id sản phẩm trong chi tiết sản phẩm
                    SanPhamDTO sanpham = await GetSanPhamById(chiTietSanPham.Id_SanPham);
                    // Bổ sung các thuộc tính của thuộc tính SanPham trong HoaDonView
                    lstsanphamview.Add(new SanPhamView
                    {
                        Ten = sanpham.Ten,
                        SoLuong = cthd.SoLuong
                    });
                }
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
            if (listHoaDonView == null)
            {
                ModelState.AddModelError(string.Empty, "Không có bất kỳ đơn hàng nào");
                return View();
            }
            return View(listHoaDonView);    
        }

        public async Task<IActionResult> FilterByStatus(ETrangThaiHD filterorder)
        {
            listHoaDonView = await LoadData();
            if (filterorder.GetHashCode()==0)
            {
                return View("Index", listHoaDonView);
            }
            else
            {
                var list = listHoaDonView.Where(x => x.TrangThai == filterorder).ToList();
                if (list.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "Không tìm thấy hóa đơn có trạng thái này.");
                }
                return View("Index", list);
            }
        }
        public async Task<IActionResult> SearchById(int id)
        {
            listHoaDonView = await LoadData();
            var viewDetail = listHoaDonView.FirstOrDefault(x => x.Id == id);
            // Xử lý khi không tìm thấy hóa đơn
            if (viewDetail == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy hóa đơn với ID này.");
            }
            // Tạo danh sách chứa thông tin hóa đơn tìm thấy
            var list = new List<HoaDonView>();
            if (viewDetail != null)
            {
                list.Add(viewDetail);
            }

            return View("Index", list);
        }
        public async Task<IActionResult> OrderDetail(int id)
        {
            listHoaDonView = await LoadData();
            HoaDonView viewDetail = new HoaDonView();
            viewDetail = listHoaDonView.FirstOrDefault(x => x.Id == id);
            return View(viewDetail);
            
        }
    }
}
