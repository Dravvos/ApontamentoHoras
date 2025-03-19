using ApontamentoHoras.DTO;
using ApontamentoHoras.IdentityServer.Configuration;
using ApontamentoHoras.IdentityServer.Models;
using ApontamentoHoras.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ApontamentoHoras.IdentityServer.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _service;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount([FromBody] SignUpDTO dto, string returnUrl)
        {
            if (await _userManager.FindByNameAsync(dto.Username) != null)
            {
                return BadRequest("Username already in use");
            }
            if(await _userManager.FindByEmailAsync(dto.Email) != null)
            {
                return BadRequest("Email already in use");
            }

            var result = await _userManager.CreateAsync(new ApplicationUser { UserName = dto.Username, Nome = dto.Nome, Sobrenome = dto.Sobrenome, Email = dto.Email }, dto.Password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(dto.Username);
                await _userManager.AddToRoleAsync(user, IdentityConfiguration.Client);
                await _userManager.AddClaimsAsync(user, new Claim[]
              {
                    new Claim(ClaimTypes.Name,dto.Username),
                    new Claim(ClaimTypes.Email,dto.Email),
                    new Claim(ClaimTypes.GivenName,dto.Nome),
                    new Claim(ClaimTypes.Surname,dto.Sobrenome),
                    new Claim(ClaimTypes.Role, IdentityConfiguration.Client)
              });
                return Created();
            }
            else
                return BadRequest(result.Errors);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (await _userManager.FindByNameAsync(dto.Username) == null)
            {
                return BadRequest("Invalid username/password");
            }
            
            var result = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(dto.Username);
                var token = _service.GenerateTokenAsync(user);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddHours(3)
                };

                Response.Cookies.Append("AuthToken", token, cookieOptions);
                return Ok("Logged in succesfully");
            }
            else
                return BadRequest("Invalid credentials");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound();
            
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return NoContent();            
            else
                return BadRequest(result.Errors);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            Request.Cookies.TryGetValue("AuthToken", out var token);
            
            //var user = await _userManager.GetUserAsync(User);
            //return Ok(new { user.Nome, user.Sobrenome, user.Email, userRoles});
            return Ok();
        }
    }
}
