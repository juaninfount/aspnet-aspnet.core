using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Entities;
using Microsoft.Extensions.Configuration;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Infraestructure.Interfaces;
using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace SocialMedia.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]    
    public class PostController : ControllerBase
    {
        private readonly IPostService _PostService = null;        
        private readonly IMapper _Mapper = null;
        private readonly IUriService _IUriService = null;   

        private readonly ISecurityService _ISecurityService  = null;

        public PostController(IPostService postService, 
                              IMapper mapper, 
                              IUriService IUriService,
                              ISecurityService ISecurityService)
        {            
            this._PostService = postService;
            this._Mapper = mapper;
            this._IUriService = IUriService;
            this._ISecurityService = ISecurityService;
        }

        [HttpGet(Name = nameof(GetPosts))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPosts([FromQuery]PostQueryFilter filters)
        {                        
            var posts =  await this._PostService.GetPosts(filters); // Entidades
            if (posts == null || posts.Count() == 0) {
                return NotFound();
            }
            
            var publicacionesDto = this._Mapper.Map<IList<PublicacionDto>>(posts);                        

            var m = (PagedList<Publicacion>)posts; 
            var metadata = new Metadata()
            {
                TotalCount      = m.TotalCount,
                PageSize        = m.PageSize,
                CurrentPage     = m.CurrentPage,
                TotalPages      = m.TotalPages,
                HasNextPage     = m.HasNextPage,
                HasPreviousPage = m.HasPreviousPage,
                NextPageUrl     = _IUriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString(),
                PreviousPageUrl = _IUriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPosts))).ToString(),
            };

            var response = new Responses.ApiResponse<IList<PublicacionDto>>(publicacionesDto)
            {
                Metadata = metadata
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(metadata));
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            PublicacionDto publicacionDto = null;
            var post = await this._PostService.GetPost(id);
            if (post == null || post.IdPublicacion == 0){
                return NotFound();
            }

            publicacionDto = this._Mapper.Map<PublicacionDto>(post);            
            var response = new Responses.ApiResponse<PublicacionDto>(publicacionDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] PublicacionDto publicacionDto)
        {
            int returnValue;
            Publicacion publicacion = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                publicacion = this._Mapper.Map<Publicacion>(publicacionDto);
                returnValue = await this._PostService.Insert(publicacion);
                var response = new Responses.ApiResponse<int>(returnValue);
                return Ok(response);
            }
            catch (Exception)
            {
                throw;
            }                        
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] PublicacionDto publicacionDto)
        {
            bool returnValue = false;
            Publicacion publicacion = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                publicacion = this._Mapper.Map<Publicacion>(publicacionDto);
                returnValue = await this._PostService.Update(id, publicacion);               
                var response = new Responses.ApiResponse<bool>(returnValue);
                return Ok(response);                
            }
            catch (Exception)
            {
                throw;
            }            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            bool returnValue = false;
            try
            {
                returnValue = await this._PostService.Delete(id);            
                if (!returnValue){
                    return BadRequest();
                } 

                var response = new Responses.ApiResponse<bool>(returnValue);
                return Ok(response);
            }
            catch (Exception)
            {
                throw;
            }           
        }

    }
}
