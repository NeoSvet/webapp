using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace MySiteApp.Account
{
  public class AuthStateProvider : AuthenticationStateProvider
  {
    private readonly IJSRuntime _js;

    public AuthStateProvider(IJSRuntime js)
    {
      _js = js;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      try
      {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", "user");
        if (json == null)
          return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var user = JsonSerializer.Deserialize<User>(json);
        if (user != null)
        {
          var identity = new ClaimsIdentity(new[]
          {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        }, "custom");

          return new AuthenticationState(new ClaimsPrincipal(identity));
        }
      }
      catch { }
      return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public void NotifyUserAuthentication(User user)
    {
      var identity = new ClaimsIdentity(new[]
      {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        }, "custom");

      var principal = new ClaimsPrincipal(identity);
      NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public void Logout()
    {
      _js.InvokeVoidAsync("localStorage.removeItem", "user");
      NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
    }
  }
}
