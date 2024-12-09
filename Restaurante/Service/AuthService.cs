using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Restaurante.Dto;
using Restaurante.Entities;
using System.Security.Claims;
using System.Text;

namespace Restaurante.Service
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string CreateToken(UsuarioResponseDto usuario)
        {
            var jwtSettings = this._configuration.GetSection("JwtSettings").Get<JwtSettings>();
            JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();

            Claim[] claims = new[]
            {
                new Claim("Rol", usuario.Rol),
                new Claim("User", usuario.Nombre),
                new Claim("Sector", usuario.Sector),
            };
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            SigningCredentials key = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationInMinutes),
                Audience = jwtSettings.Audience,
                Issuer = jwtSettings.Issuer,
                SigningCredentials = key,
            };

            string token = jsonWebTokenHandler.CreateToken(securityTokenDescriptor);
            return token;
        }

        public async Task<IDictionary<string, object>> ValidarToken(string accessToken)
        {
            JsonWebTokenHandler jsonWebTokenHandler = new JsonWebTokenHandler();
            JwtSettings? jwtSettings = this._configuration.GetSection("JwtSettings").Get<JwtSettings>();

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            SigningCredentials symmetricSecurityKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters();

            tokenValidationParameters.ValidateIssuerSigningKey = true;
            tokenValidationParameters.ValidIssuer = jwtSettings.Issuer;
            tokenValidationParameters.ValidAudience = jwtSettings.Audience;
            tokenValidationParameters.IssuerSigningKey = key;
            tokenValidationParameters.ClockSkew = TimeSpan.Zero;

            TokenValidationResult tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(accessToken, tokenValidationParameters);

            if (!tokenValidationResult.IsValid)
            {
                throw new Exception("El token proporcionado no es valido");
            }

            return tokenValidationResult.Claims;
        }
    }
}
