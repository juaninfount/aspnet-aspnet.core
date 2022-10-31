using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using SocialMedia.Infraestructure.Interfaces;
using SocialMedia.Infraestructure.Options;
using System.Security.Claims;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;

namespace SocialMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class TokenController : Controller
    {
        private readonly IConfiguration _configuration = null;
        private readonly ISecurityService _ISecurityService = null;
        private readonly IPasswordService _IPasswordService = null;
                
        public TokenController(IConfiguration configuration, ISecurityService ISecurityService, IPasswordService IPasswordService)
        {
            this._configuration = configuration;
            this._ISecurityService = ISecurityService;
            this._IPasswordService = IPasswordService;
        }

        [HttpPost]
        public IActionResult Authentication(UserLogin login)
        {
            var validation = IsValidUser(login);
            if (validation.Item1)
            {
                var token = GenerateToken(validation.Item2);
                return Ok(new { token });
            }
            return NotFound();
        }

        private (bool, Security) IsValidUser(UserLogin login)
        {
            var user =_ISecurityService.GetLoginByCredentials(login).Result; // sync call
            var isValid = _IPasswordService.Check(user.Password, login.Password); // comprobar el password almacenado vs el ingresado
            return (user != null && isValid, user);
        }

        private string GenerateToken(Security security)
        {
                        
            // headers
            var symetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials  = new SigningCredentials(symetricSecurityKey, 
                                                            SecurityAlgorithms.HmacSha256);
            var headerKey = new JwtHeader(signingCredentials);

            // claims
            var claims = new[]
            {
             new Claim(ClaimTypes.Name, security.UserName),
             new Claim("User", security.User),
             new Claim(ClaimTypes.Role, security.Role.ToString())
            };

            // payload
            var payload = new JwtPayload(
                        issuer: _configuration["Authentication:Issuer"], 
                        audience: null,
                        claims: claims, 
                        notBefore: DateTime.Now,
                        expires: DateTime.UtcNow.AddMinutes(2)
                        );

            var token = new JwtSecurityToken(headerKey, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
