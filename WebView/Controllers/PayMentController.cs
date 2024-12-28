using Microsoft.AspNetCore.Mvc;
using WebView.Models.Vnpay;
using WebView.Services.Vnpay;

namespace WebView.Controllers
{
    public class PayMentController : Controller
    {
        private readonly IVnPayService _vnPayService;

        public PayMentController(IVnPayService vnPayService)
        {

            _vnPayService = vnPayService;
        }

        [HttpPost]
        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
        [HttpGet]
        public IActionResult PaymentCallbackVnpay(PaymentResponseModel model)
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }

    }
}
