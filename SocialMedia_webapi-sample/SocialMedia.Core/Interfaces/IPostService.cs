using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostService
    {
        Task<int> Insert(Publicacion publicacion);

        Task<IEnumerable<Publicacion>> GetPosts(PostQueryFilter filters);

        Task<Publicacion> GetPost(int id);

        Task<bool> Update(int id, Publicacion publicacion);

        Task<bool> Delete(int id);

    } 

}