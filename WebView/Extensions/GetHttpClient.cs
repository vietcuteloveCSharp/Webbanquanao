namespace WebView.Extensions
{
    public class GetHttpClient
    {
        private IHttpClientFactory httpClientFactory;
        private const string HeaderKey = "Authorization";

        public GetHttpClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        // Thêm token ở local store và thêm vào header của request
        public HttpClient GetPrivateHttpClient()
        {
            // Mở kết nối dến Server có tên "SystemApiClient"
            var client = httpClientFactory.CreateClient("SystemApiClient");
            return client;
        }

        // Trả về Httpclient ko có chứa thông tin Authorization -> tăng tính bảo mật khi xóa token của user
        public HttpClient GetPublichHttpClient()
        {
            var client = httpClientFactory.CreateClient("SystemApiClient");
            client.DefaultRequestHeaders.Remove(HeaderKey);
            return client;
        }
    }
}
