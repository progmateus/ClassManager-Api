
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClassManager.Domain.Contexts.Auth.Commands;
using ClassManager.Domain.Contexts.Users.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace ClassManager.Domain.Contexts.Auth.Services
{
  public class TokenService
  {
    public string Create(UserViewModel user, string secret, DateTime expiresAt)
    {
      var handler = new JwtSecurityTokenHandler();

      var key = Encoding.ASCII.GetBytes(secret);

      var credentials = new SigningCredentials(
        new SymmetricSecurityKey(key),
      SecurityAlgorithms.HmacSha256
      );

      var tokenDescription = new SecurityTokenDescriptor
      {
        SigningCredentials = credentials,
        Expires = expiresAt,
        Subject = GenerateClaims(user)
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

    public ClaimsCommand GetJwtClaims(string token)
    {
      var handler = new JwtSecurityTokenHandler();

      var jwtToken = handler.ReadJwtToken(token);

      var claims = jwtToken.Claims;

      var userId = claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "";

      var username = claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "";


      return new ClaimsCommand
      {
        Id = userId,
        Name = username,
      };
    }

    public bool ValidateToken(string token)
    {
      var handler = new JwtSecurityTokenHandler();

      return handler.CanReadToken(token);
    }
  }
}