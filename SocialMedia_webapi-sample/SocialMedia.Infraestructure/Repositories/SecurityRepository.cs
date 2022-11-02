using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using Dapper;

namespace SocialMedia.Infraestructure.Repositories
{
    public class SecurityRepository: ISecurityRepository
    {
        //ISocialMediaRepository _databaseRepository = null;
         IDapperContext _IDapperContext = null;

        /*
        public SecurityRepository(ISocialMediaRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }*/

        public SecurityRepository(IDapperContext IDapperContext)
        {
            _IDapperContext = IDapperContext;
        }

        /*
        public  Security GetLoginByCredentials(UserLogin login)
        {
            string sql = "";
           
            try
            {
            sql = string.Format(" select top 1 [SecurityID],[User],[UserName],[Password],[Role] " +
                                  " from dbo.[Security] s where  s.[User] = '{1}' ", 
                                  login.Password, login.User);
                                   
            var resultset = _databaseRepository.GetDataTable(sql, null, System.Data.CommandType.Text);
          
            Security objSecurity = new Security();
            
            if (resultset != null && resultset.Rows.Count > 0) 
            {
                foreach (DataRow dr in resultset.Rows) 
                {
                    objSecurity = new Security()
                    {
                        Id = Convert.ToInt32(dr["SecurityID"]),
                        User = Convert.ToString(dr["User"]).Trim(),
                        UserName = Convert.ToString(dr["UserName"]).Trim(),
                        Password = Convert.ToString(dr["Password"]).Trim()                        
                    };

                    if(Convert.ToString(dr["Role"]).ToLower().Trim().Contains("admin"))
                    {
                        objSecurity.Role = Roletype.Administrator;
                    }

                    if(Convert.ToString(dr["Role"]).ToLower().Trim().Contains("operator"))
                    {
                        objSecurity.Role = Roletype.Operator;
                    }
                                                                                                                               
                }

              
            }

            return objSecurity;
            }
            catch (System.Exception)
            {                
                throw;
            }
        }
        */


        public async Task<Security> GetLoginByCredentials(UserLogin login)
        {
            var query = "";
            Security objSecurity;
            try
            {
                query = string.Format(" select top 1 s.[SecurityID],s.[User],s.[UserName],s.[Password],( " +
                                " CASE s.Role WHEN 'admin' THEN " +  (int)Roletype.Administrator + " WHEN 'operator' THEN "+ (int)Roletype.Operator + " END " +
                                " ) " +
                                " from dbo.[Security] s where s.[User] = '{0}' ", 
                                 login.User);

                using (var connection = _IDapperContext.CreateConnection())
                {
                    objSecurity = await connection.QuerySingleOrDefaultAsync<Security>(query);
                }
                return objSecurity;
            }
            catch (System.Exception)
            {                
                throw;
            }
        }
        
        /*
        public async Task<int>  RegisterUser(Security security)
        {
            StringBuilder str = null;
            List<SqlParameter> parameters = null;
            int id = 0;

            try
            {
                parameters = new List<SqlParameter>();
                str = new StringBuilder();
                str.AppendFormat(@"INSERT INTO [dbo].[Security]
                                    ([User]
                                    ,[UserName]
                                    ,[Password]
                                    ,[Role])
                                    VALUES
                                    ('{0}',
                                    '{1}',
                                    '{2}',
                                    '{3}')", security.User, security.UserName, security.Password, security.Role);
                
                id = await _databaseRepository.Insert(str.ToString(), parameters, CommandType.Text); 
                return id;
            }
            catch (System.Exception)
            {                
                throw;
            } 
        }
        */

        public async Task<int>  RegisterUser(Security security)
        {
            StringBuilder str = null;
            int id = 0;
            try
            {
                str = new StringBuilder();
                str.AppendFormat(@"INSERT INTO [dbo].[Security]
                                    ([User]
                                    ,[UserName]
                                    ,[Password]
                                    ,[Role])
                                    VALUES
                                    ('{0}',
                                    '{1}',
                                    '{2}',
                                    '{3}')", security.User, security.UserName, security.Password, security.Role);

                using(var connection = _IDapperContext.CreateConnection())
                {
                    id = await connection.ExecuteScalarAsync<int>(str.ToString(), param: security);                    
                }
                return id;
            }
            catch (System.Exception)
            {                
                throw;
            } 
        }
    }
}