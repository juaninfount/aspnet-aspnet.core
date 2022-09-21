using SocialMedia.Core.Interfaces;

namespace SocialMedia.Infraestructure.Repositories
{
    //// En caso se implemente un ORM: Inyectar dependencia de contexto 
    public class BaseRepository<T> : IRepository<T> where T:SocialMedia.Core.Entities.BaseEntity
    {        
        
        public BaseRepository()
        {   
            
        }

        /*
        public async Task<int> Insert(T entity)
        {                       
            throw new NotImplementedException();             
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            //_databaseRepository.GetDataTable()
            throw new NotImplementedException();
        }

        public async Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async  Task<bool> Update(int id, T Entity)
        {
            throw new NotImplementedException();
        }

        */
    }
}