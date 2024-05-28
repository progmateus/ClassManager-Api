
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClassManager.Shared.Contracts;
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
        Subject = GenerateClaims(data)
      };
      var token = handler.CreateToken(tokenDescription);
      return handler.WriteToken(token);
    }

    private ClaimsIdentity GenerateClaims(AuthData user)
    {
      var ci = new ClaimsIdentity();

      ci.AddClaim(new Claim("id", user.Id.ToString()));
      ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
      ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
      ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
      ci.AddClaim(new Claim("avatar", user.Avatar ?? ""));

      foreach (var role in user.Roles)
        ci.AddClaim(new Claim(ClaimTypes.Role, role));

      return ci;
    }
  }
}