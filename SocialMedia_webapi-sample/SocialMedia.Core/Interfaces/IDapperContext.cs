using System.Data;

namespace SocialMedia.Core.Interfaces
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}