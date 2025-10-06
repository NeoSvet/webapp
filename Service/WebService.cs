using MySiteApp.Account;
using System.Net.Http.Json;

namespace MySiteApp.Service
{
  public class WebService
  {
    private readonly HttpClient _http;

    public WebService(HttpClient http)
    {
      _http = http;
    }

    public async Task<User?> LoginAsync(string login, string password)
    {
      var request = new LoginRequest { Login = login, Password = password };
      var response = await _http.PostAsJsonAsync("https://neosvet.somee.com/api/account", request);

      if (response.IsSuccessStatusCode)
      {
        return await response.Content.ReadFromJsonAsync<User>();
      }

      return null;
    }

    public async Task<string> LoadAsync(string url)
    {     
      var request = new UrlForm { Url = url };
      var response = await _http.PostAsJsonAsync("https://neosvet.somee.com/api/raw", request);

      if (response.IsSuccessStatusCode)
      {
        return await response.Content.ReadAsStringAsync();
      }

      return "";
    }
  }
}
