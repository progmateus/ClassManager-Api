
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClassManager.Domain.Contexts.Users.ViewModels;
using ClassManager.Domain.Shared.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Domain.Contexts.Auth.Services
{
  public class TokenService
  {
    public string Create(AuthData data)
    {
      var handler = new JwtSecurityTokenHandler();

      var key = Encoding.ASCII.GetBytes(Configuration.Secrets.JwtPrivateKey);

      var credentials = new SigningCredentials(
        new SymmetricSecurityKey(key),
      SecurityAlgorithms.HmacSha256
      );

      var tokenDescription = new SecurityTokenDescriptor
      {
        SigningCredentials = credentials,
        Expires = DateTime.UtcNow.AddHours(2),
        Subject = GenerateClaims(data.User)
      };
      var token = handler.CreateToken(tokenDescription);
      return handler.WriteToken(token);
    }

    private ClaimsIdentity GenerateClaims(UserViewModel user)
    {
      var ci = new ClaimsIdentity();

      ci.AddClaim(new Claim("id", user.Id.ToString()));
      ci.AddClaim(new Claim(ClaimTypes.Name, user.Username ?? ""));
      ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name ?? ""));
      ci.AddClaim(new Claim(ClaimTypes.Email, user.Email ?? ""));
      ci.AddClaim(new Claim("avatar", user.Avatar ?? ""));
      foreach (var userRole in user.UsersRoles)
        ci.AddClaim(new Claim(ClaimTypes.Role, $"@{userRole.TenantId} @{userRole.Role?.Name ?? ""}"));

      return ci;
    }
  }
}