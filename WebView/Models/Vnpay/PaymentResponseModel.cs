namespace WebView.Models.Vnpay
{
    public class PaymentResponseModel
    {
        public string OrderDescription { get; set; }
        public string TransactionId { get; set; }
        public string VnpTxnRef { get; set; } // orderId
        public string PaymentMethod { get; set; }
        public string PaymentId { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
        public string VnpDate { get; set; }
        public decimal Amount { get; set; }
    }
}
