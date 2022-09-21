using System;

namespace SocialMedia.Core.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
         IPostRepository PostRepository {get;}
         IUserRepository UserRepository {get;}        

         ISecurityRepository SecurityRepository {get;}
    }
}