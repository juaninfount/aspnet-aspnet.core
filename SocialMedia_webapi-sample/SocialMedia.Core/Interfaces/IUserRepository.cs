using SocialMedia.Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SocialMedia.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Usuario>> GetAll();

        Task<Usuario> GetById(int id);

        Task<int> Insert(Usuario usuario);

        Task<bool> Update(int id, Usuario usuario);

        Task<bool> Delete(int id);
    }
}