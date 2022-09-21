using SocialMedia.Core.Entities;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface ISecurityRepository
    {
      Security GetLoginByCredentials(UserLogin login);  

      Task<int> RegisterUser(Security security);
      
    } 
}