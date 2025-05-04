using WebView.Models.Vnpay;

namespace WebView.Services.Vnpay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
        StringContent RefundPayment(RefundRequest request, HttpContext context);
    }
}
