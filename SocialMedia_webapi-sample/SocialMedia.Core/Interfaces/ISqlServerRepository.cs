
using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SocialMedia.Core.Interfaces
{
    public interface ISocialMediaRepository
    {
        Task<SqlDataReader> GetDataReader(string procedureName, List<SqlParameter> parameters,
                                            CommandType commandType = CommandType.StoredProcedure);

        DataTable GetDataTable(string procedureName, List<SqlParameter> parameters,
                                            CommandType commandType = CommandType.StoredProcedure);

        Task<int> Insert(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure);

        Task<int> ExecuteNonQuery(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure);        
    }
}
