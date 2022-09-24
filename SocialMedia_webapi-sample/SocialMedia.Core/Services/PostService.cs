using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Core.CustomEntities;
using Microsoft.Extensions.Options;

namespace SocialMedia.Core.Services
{
    public class PostService: IPostService
    {        
        private readonly IUnitOfWork _UnitOfWork = null;
        private readonly PaginationOptions _PaginationOptions = null;
        
        public PostService( IUnitOfWork UnitOfWork, IOptions<PaginationOptions> options)
        {
            _UnitOfWork = UnitOfWork;
            _PaginationOptions = options.Value;
        }

        public async Task<int> Insert(Publicacion publicacion) 
        {
            try
            {
                if(publicacion == null)
                {
                    throw new BusinessException("Publicación inválida");
                }

                if(publicacion.IdUsuario == 0)
                {
                    throw new BusinessException("Usuario de Publicación inválido");
                }

                Usuario usuario = await _UnitOfWork.UserRepository.GetById(publicacion.IdUsuario);                
                if(usuario == null || usuario.IdUsuario == 0)
                {
                    throw new BusinessException("Usuario no encontrado");
                }

                var userPost = await _UnitOfWork.PostRepository.GetPostsByUser(publicacion.IdUsuario);
                if(userPost.Count() > 0 && userPost.Count() < 10)
                {
                    var lastPost = userPost.OrderByDescending(x=>x.Fecha).First();
                    if( (DateTime.Now-lastPost.Fecha).TotalDays < 7 )
                    {
                        throw new BusinessException("No está habilitado para publicar");
                    }
                }

                if(publicacion.Descripcion.ToLower().Contains("sexo"))
                {
                    throw new BusinessException("Contenido no permitido");
                }

                return await _UnitOfWork.PostRepository.Insert(publicacion);
            }
            catch (Exception)
            {
                throw;
            }
        }
                
        public async Task<IEnumerable<Publicacion>> GetPosts(PostQueryFilter filters)
        {
            var posts = await this._UnitOfWork.PostRepository.GetAll(filters);
            if(posts != null && posts.Count() > 0 && filters != null)
            {            
                filters.PageNumber = filters.PageNumber <= 0 ? 1 : filters.PageNumber;
                filters.PageSize = filters.PageSize <= 0 ? 10 : filters.PageSize;
                posts = PagedList<Publicacion>.Create(posts, filters.PageNumber, filters.PageSize);
            }
            return posts;
        }
        
        public async Task<Publicacion> GetPost(int id)
        {   
            try
            {                
                return await this._UnitOfWork.PostRepository.GetById(id);
            }
            catch (System.Exception)
            {                
                throw;
            }                     
        }
                      
        public async Task Update(int id, Publicacion publicacion)
        {                     
            try
            {
                await this._UnitOfWork.PostRepository.Update(id, publicacion);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task Delete(int id)
        {            
            try
            {        
                await this._UnitOfWork.PostRepository.Delete(id);
            }
            catch (System.Exception)
            {
                throw;
            }
        }       
    }
}
