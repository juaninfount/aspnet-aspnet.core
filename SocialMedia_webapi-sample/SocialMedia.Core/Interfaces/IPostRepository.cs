using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostRepository
    {
        
        Task<IList<Publicacion>> GetAll(PostQueryFilter filters);

        Task<Publicacion> GetById(int id);

        Task<int> Insert(Publicacion publicacion);

        Task<bool> Update(int id, Publicacion publicacion);

        Task<bool> Delete(int id);        

        Task<IEnumerable<Publicacion>>  GetPostsByUser(int id);
        
    }
}
