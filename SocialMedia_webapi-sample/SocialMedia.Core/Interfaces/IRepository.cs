using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
    public interface IRepository<T> where T: SocialMedia.Core.Entities.BaseEntity
    {
        /*
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(int id);

        Task<int> Insert(T entity);

        Task<bool> Update(int id, T entity);

        Task<bool> Delete(int id);
        */
    }
}