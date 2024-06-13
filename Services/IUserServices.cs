using Clase_1.DTOS;
using HomeBankingMindHub.Models;
using System.Security.Claims;

namespace Clase_1.Services
{
    public interface IUserServices
    {
        public void Verify(ClientUserDTO client);
        public void ValidateJwtSettings();
        public string CreateToken(Client user);
        public ClaimsIdentity Login(ClientUserDTO client);
    }
}
