namespace WebView.Models.Vnpay
{
    public class PaymentInformationModel
    {
        public string OrderType { get; set; } = "other";
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }
        public int IdHoaDon { get; set; } = 0;
    }
}
