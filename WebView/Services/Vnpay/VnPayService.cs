using Newtonsoft.Json;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WebView.Areas.BanHangOnline.HoangDTO;
using WebView.Libraries;
using WebView.Models.Vnpay;
using WebView.Repository;

namespace WebView.Services.Vnpay
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["Vnpay:PaymentBackReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (((int)model.Amount) * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{RemoveVietnameseAccentsAndSpecialCharacters(model.OrderDescription)}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);
            pay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            // setup session với vnp_TxnRef để lưu phí vận chuyển và id mã giảm giá
            var sessionThanhToan = new SessionThanhToanModel
            {
                VnpTxnRef = tick,
                IdHoaDon = model.IdHoaDon,
                PhuongThucThanhToan = model.PhuongThucThanhToan,
            };
            var lstSessionThanhToan = context.Session.GetObjectFromJson<List<SessionThanhToanModel>>("SessionThanhToan");
            if (lstSessionThanhToan != null && lstSessionThanhToan.Count >= 1)
            {
                lstSessionThanhToan.Add(sessionThanhToan);
            }
            else
            {
                lstSessionThanhToan = new List<SessionThanhToanModel> { sessionThanhToan };
            }


            context.Session.Remove("SessionThanhToan");
            context.Session.SetString("SessionThanhToan", JsonConvert.SerializeObject(lstSessionThanhToan));
            return paymentUrl;
        }

        public StringContent RefundPayment(RefundRequest request, HttpContext context)
        {

            var pay = new VnPayLibrary();

            var vnp_Amount = request.Amount * 100; // VnPay tính amount theo cent
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_TransactionDate = request.TransactionDate; // Thời gian giao dịch gốc
            var vnp_TransactionNo = request.TransactionNo; // Mã giao dịch gốc
            var vnp_TxnRef = request.TxnRef; // Mã tham chiếu giao dịch
            var vnp_CreateBy = request.CreateBy; // Người yêu cầu refund
            var vnp_TransactionType = "02"; // Loại giao dịch refund (02: Hoàn toàn, 03: Hoàn 1 phần)
            var vnp_OrderInfo = $"Refund for transaction {vnp_TxnRef}";

            // Tạo dictionary chứa các tham số
            var vnp_Params = new Dictionary<string, string>
                {
                    { "vnp_RequestId", GenerateRandomCode() },
                    { "vnp_Version",  _configuration["Vnpay:Version"] },
                    { "vnp_Command", "refund" },
                    { "vnp_TmnCode",  _configuration["Vnpay:TmnCode"] },
                    { "vnp_TransactionType", vnp_TransactionType },
                    { "vnp_TxnRef", vnp_TxnRef },
                    { "vnp_Amount", vnp_Amount.ToString() },
                    { "vnp_OrderInfo", vnp_OrderInfo },
                    { "vnp_TransactionNo", vnp_TransactionNo },
                    { "vnp_TransactionDate", vnp_TransactionDate },
                    { "vnp_CreateBy", vnp_CreateBy },
                    { "vnp_CreateDate", vnp_CreateDate },
                    { "vnp_IpAddr", pay.GetIpAddress(context) },
                };

            // Tạo chữ ký
            string signData = string.Join("|", new[]
            {
                    vnp_Params["vnp_RequestId"],
                    vnp_Params["vnp_Version"],
                    vnp_Params["vnp_Command"],
                    vnp_Params["vnp_TmnCode"],
                    vnp_Params["vnp_TransactionType"],
                    vnp_Params["vnp_TxnRef"],
                    vnp_Params["vnp_Amount"],
                    vnp_Params["vnp_TransactionNo"],
                    vnp_Params["vnp_TransactionDate"],
                    vnp_Params["vnp_CreateBy"],
                    vnp_Params["vnp_CreateDate"],
                    vnp_Params["vnp_IpAddr"],
                    vnp_Params["vnp_OrderInfo"],
                });

            string vnp_SecureHash = HmacSHA512(_configuration["Vnpay:HashSecret"], signData);
            vnp_Params.Add("vnp_SecureHash", vnp_SecureHash);
            var content = new StringContent(JsonConvert.SerializeObject(vnp_Params), Encoding.UTF8, "application/json");
            return content;
        }
        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            return response;
        }
        private string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
        static string RemoveVietnameseAccentsAndSpecialCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Normalize string to FormD to separate base characters and diacritical marks
            string normalizedText = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalizedText)
            {
                // Check if the character is not a non-spacing mark (diacritical mark)
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            // Return the string normalized back to FormC and remove special characters
            string noAccentsText = sb.ToString().Normalize(NormalizationForm.FormC);

            // Use regex to remove special characters, keeping letters, digits, and spaces
            string cleanText = Regex.Replace(noAccentsText, @"[^a-zA-Z0-9\s]", "");

            // Trim extra spaces
            return Regex.Replace(cleanText, @"\s+", " ").Trim();
        }
        public string GetFutureDate(int days)
        {
            // Lấy ngày hiện tại và thêm số ngày
            DateTime now = DateTime.Now.AddDays(days);

            // Định dạng ngày theo yêu cầu
            string yyyy = now.Year.ToString();
            string MM = now.Month.ToString("D2"); // Tháng từ 1-12, không cần +1
            string dd = now.Day.ToString("D2");
            string HH = now.Hour.ToString("D2");
            string mm = now.Minute.ToString("D2");
            string ss = now.Second.ToString("D2");

            // Kết hợp thành chuỗi định dạng yyyyMMddHHmmss
            return $"{yyyy}{MM}{dd}{HH}{mm}{ss}";
        }

        private static readonly Random _random = new Random();
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateRandomCode()
        {
            return new string(Enumerable.Repeat(Characters, 32)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

    }
}
