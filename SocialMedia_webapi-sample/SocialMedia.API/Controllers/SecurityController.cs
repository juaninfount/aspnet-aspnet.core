using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Dtos;
//using SocialMedia.Core.Enumerations;
using SocialMedia.Infraestructure.Interfaces;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Threading.Tasks;
using SocialMedia.API.Responses;
using SocialMedia.Core.Enumerations;

namespace SocialMedia.API.Controllers
{
    [Authorize(Roles=nameof(Roletype.Administrator))]
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class SecurityController: ControllerBase
    {
        private readonly IMapper _mapper = null;
        private readonly ISecurityService _securityService = null;
        private readonly IPasswordService _passwordService = null;

        public SecurityController(ISecurityService securityService, IMapper mapper, IPasswordService passwordService)
        {
            _securityService = securityService;
            _mapper =    mapper;
            _passwordService = passwordService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SecurityDto securityDto)
        {
            var security = _mapper.Map<Security>(securityDto);
            security.Password = _passwordService.Hash(security.Password);
            await _securityService.RegisterUser(security);

            securityDto = _mapper.Map<SecurityDto>(security);            
            var response = new ApiResponse<SecurityDto>(securityDto);
            return Ok(response);
        }
    }
}