using SocialMedia.Core.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace SocialMedia.Core.Interfaces
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}