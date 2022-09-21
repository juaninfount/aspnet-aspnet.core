using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;

namespace SocialMedia.Core.Interfaces
{
    public interface ISecurityService
    {
        Security GetLoginByCredentials(UserLogin login);
        
        Task<int> RegisterUser(Security security);
    }
}