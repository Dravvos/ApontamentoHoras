using ApontamentoHoras.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace ApontamentoHoras.IdentityServer.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
