using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SocialMedia.Infraestructure.Repositories
{   
    public class SocialMediaRepository: ISocialMediaRepository
    {
        protected string ConnectionString { get; set; }

        public SocialMediaRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
     
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }

        protected SqlCommand GetCommand(SqlConnection connection, string commandText, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);
            command.CommandType = commandType;
            return command;
        }

        protected SqlParameter GetParameter(string parameter, object value)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, value != null ? value : DBNull.Value);
            parameterObject.Direction = ParameterDirection.Input;
            return parameterObject;
        }

        protected SqlParameter GetParameterOut(string parameter, SqlDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, type); ;

            if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText || type == SqlDbType.Text)
            {
                parameterObject.Size = -1;
            }

            parameterObject.Direction = parameterDirection;

            if (value != null)
            {
                parameterObject.Value = value;
            }
            else
            {
                parameterObject.Value = DBNull.Value;
            }

            return parameterObject;
        }

        public async Task<int> ExecuteNonQuery(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int returnValue = -1;

            try
            {
                using (SqlConnection connection = this.GetConnection())
                {
                    SqlCommand cmd = this.GetCommand(connection, procedureName, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex) when( ex.Message == "")
            {
                //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);
                throw;
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<int> Insert(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int returnValue = -1;            

            try
            {
                using (SqlConnection connection = this.GetConnection())
                {
                    SqlCommand cmd = this.GetCommand(connection, procedureName, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    await cmd.ExecuteNonQueryAsync();

                    cmd = new SqlCommand("select @@Identity", connection);
                    var objResult = await cmd.ExecuteScalarAsync();
                    if ( !Int32.TryParse( Convert.ToString(objResult), out returnValue)) 
                    {
                        returnValue = -1;
                    }
                }
            }
            catch (Exception)
            {                
                throw;
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected object ExecuteScalar(string procedureName, List<SqlParameter> parameters)
        {
            Task<object> returnValue = null;

            try
            {
                using (SqlConnection connection = this.GetConnection())
                {
                    SqlCommand cmd = this.GetCommand(connection, procedureName, CommandType.StoredProcedure);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = cmd.ExecuteScalarAsync();
                }
            }
            catch (Exception)
            {
                //LogException("Failed to ExecuteScalar for " + procedureName, ex, parameters);
                throw;
            }

            return returnValue;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> GetDataReader(string procedureName, List<SqlParameter> parameters, 
                                            CommandType commandType = CommandType.StoredProcedure)
        {            
            SqlDataReader task = null;
            try
            {
                SqlConnection connection = this.GetConnection();
                {
                    SqlCommand cmd = this.GetCommand(connection, procedureName, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());                        
                    }
                    task = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception)
            {                
                throw;
            }
            
            return task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string procedureName, List<SqlParameter> parameters,
                                            CommandType commandType = CommandType.StoredProcedure)
        {                        
            SqlDataAdapter adapter = null;            
            DataTable dataTable = null;
            SqlCommand cmd = null;

            try
            {
                using (SqlConnection connection = this.GetConnection()) 
                {
                    cmd = this.GetCommand(connection, procedureName, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());                        
                    }

                    adapter = new SqlDataAdapter(cmd);                    

                    var dataSet = new DataSet();                                        
                    adapter.Fill(dataSet);                    
                    dataTable = dataSet.Tables[0];
                };
            }
            catch (Exception)
            {                
                throw;
            }
            finally
            {
                if (adapter != null){ adapter.Dispose(); adapter = null; }
                if (cmd != null) { cmd.Dispose(); cmd = null;  }
            }

            return dataTable;
        }
    }
}
