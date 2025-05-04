namespace WebView.Models.Vnpay
{
    public class RefundRequest
    {
        public decimal Amount { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionNo { get; set; }
        public string TxnRef { get; set; }
        public string CreateBy { get; set; } = "KhacHangShopCanMan";
        public string TransactionType { get; set; }
    }
}
