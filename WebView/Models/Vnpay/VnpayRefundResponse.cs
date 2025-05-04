namespace WebView.Models.Vnpay
{
    public class VnpayRefundResponse
    {
        public string vnp_ResponseCode { get; set; }
        public string vnp_Message { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_Amount { get; set; }
        public string vnp_TransactionNo { get; set; }
        public string vnp_TransactionDate { get; set; }
        public string vnp_ResponseId { get; set; }
        public string vnp_SecureHash { get; set; }
    }
}
