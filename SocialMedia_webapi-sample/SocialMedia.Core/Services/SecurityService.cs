using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Services
{
    public class SecurityService: ISecurityService
    {  
        private readonly IUnitOfWork _UnitOfWork = null;
        public SecurityService(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public Security GetLoginByCredentials(UserLogin login)
        {
           return _UnitOfWork.SecurityRepository.GetLoginByCredentials(login);
        }

        public async Task<int> RegisterUser(Security security)
        {
            return await _UnitOfWork.SecurityRepository.RegisterUser(security);
        }
        
    }
}