using SocialMedia.Core.Interfaces;

namespace SocialMedia.Infraestructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly IPostRepository _PostRepository = null;
        private readonly IUserRepository _UserRepository = null;

        private readonly ISocialMediaRepository _SocialMediaRepository = null;
        private readonly ISecurityRepository _SecurityRepository = null;

        public UnitOfWork(ISocialMediaRepository ISocialMediaRepository)
        {
            _SocialMediaRepository = ISocialMediaRepository;
        }

        public IPostRepository      PostRepository => _PostRepository ?? new PostRepository(_SocialMediaRepository);

        public IUserRepository     UserRepository => _UserRepository ?? new UserRepository(_SocialMediaRepository);
        public ISecurityRepository SecurityRepository => _SecurityRepository ?? new SecurityRepository(_SocialMediaRepository);

        public void Dispose()
        {            
        }
    }
}