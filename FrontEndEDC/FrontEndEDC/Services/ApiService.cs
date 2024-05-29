namespace FrontEndEDC.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<string> GetAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
