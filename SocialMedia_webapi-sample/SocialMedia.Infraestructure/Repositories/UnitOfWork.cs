using SocialMedia.Core.Interfaces;

namespace SocialMedia.Infraestructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly IPostRepository _PostRepository = null;
        private readonly IUserRepository _UserRepository = null;

        //private readonly ISocialMediaRepository _SocialMediaRepository = null;
        private readonly ISecurityRepository _SecurityRepository = null;
        private readonly IDapperContext _dapperContext = null;

        public UnitOfWork(IDapperContext dapperContext)
        {            
            _dapperContext = dapperContext;
        }
        /* 
        public UnitOfWork(ISocialMediaRepository ISocialMediaRepository, IDapperContext dapperContext)
        {
            _SocialMediaRepository = ISocialMediaRepository;
            _dapperContext = dapperContext;
        } */

       
        //public IPostRepository      PostRepository => _PostRepository ?? new PostRepository(_SocialMediaRepository);
        public IPostRepository PostRepository => _PostRepository ?? new PostRepository(_dapperContext);
        //public IUserRepository     UserRepository => _UserRepository ?? new UserRepository(_SocialMediaRepository);
        public IUserRepository     UserRepository => _UserRepository ?? new UserRepository(_dapperContext);
        
        //public ISecurityRepository SecurityRepository => _SecurityRepository ?? new SecurityRepository(_SocialMediaRepository);
        public ISecurityRepository SecurityRepository => _SecurityRepository ?? new SecurityRepository(_dapperContext);

        public void Dispose()
        {            
        }
    }
}