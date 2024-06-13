using Clase_1.Configuration;
using Clase_1.DTOS;
using Clase_1.Repositories;
using Clase_1.Repositories.Implementations;
using HomeBankingMindHub.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clase_1.Services.Implementations
{
    public class UserServices : IUserServices
    {

        private readonly IClientServices _clientServices;
        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration _config;
        private readonly JwtSettings _jwtSettings;


        public UserServices(IClientServices clientServices, IClientRepository clientRepository, IConfiguration config, IOptions<JwtSettings> jwtSettings)
        {
            _clientServices = clientServices;
            _clientRepository = clientRepository;
            _config = config;
            _jwtSettings = jwtSettings.Value;
        }

        public void Verify(ClientUserDTO client)
        {
            Client user = _clientRepository.GetClientByEmail(client.Email);

            if (user == null)
            {
                throw new Exception("Por favor rellene todos los campos");
            }
            else
            {
                if (!BCrypt.Net.BCrypt.EnhancedVerify(client.Password, user.Password))
                {
                    throw new Exception("La contraseña es incorrecta");
                }
            }
        }

        public void ValidateJwtSettings()
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Issuer) || string.IsNullOrWhiteSpace(_jwtSettings.Audience) || string.IsNullOrWhiteSpace(_jwtSettings.SecretKey))
            {
                throw new Exception("Config values are null or empty");
            }
        }

        public string CreateToken(Client user)
        {
            ValidateJwtSettings();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name , user.Email),
                    new Claim(ClaimTypes.Role , "Client"),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            Console.WriteLine(tokenHandler.WriteToken(token));
            return tokenHandler.WriteToken(token);
        }

        public ClaimsIdentity Login(ClientUserDTO client)
        {
            Client user = _clientRepository.GetClientByEmail(client.Email);
            Verify(client);

            var claims = new List<Claim>
                {
                    new Claim("Client", user.Email)
                };
            if (client.Email.Equals("lusoto@gmail.com"))
            {
                claims.Add(new Claim("Role", "Admin"));

            }

            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
