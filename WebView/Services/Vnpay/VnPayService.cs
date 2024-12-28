using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using WebView.Libraries;
using WebView.Models.Vnpay;

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
            pay.AddRequestData("vnp_Amount", (((double)model.Amount) * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{RemoveVietnameseAccentsAndSpecialCharacters(model.Name)} {RemoveVietnameseAccentsAndSpecialCharacters(model.OrderDescription)} {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);
            pay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }
        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            return response;
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

    }
}
