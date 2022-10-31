using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using Dapper;

public class UserRepository : IUserRepository
{
    ISocialMediaRepository _databaseRepository = null;
    IDapperContext _IDapperContext = null;

    public UserRepository(ISocialMediaRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;            
    }

    public UserRepository(IDapperContext IDapperContext)
    {
        _IDapperContext = IDapperContext;
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }
  
    /*
    public async Task<Usuario> GetById(int id)
    {
        List<SqlParameter> parameters = null;
        SqlDataReader dataReader = null;
        SocialMedia.Core.Entities.Usuario  usuario = null;

        try
        {
            parameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input 
                }
            };
            
            dataReader = await _databaseRepository.GetDataReader(@"
                        select top 1 IdUsuario,Nombres,Apellidos,Email,FechaNacimiento,Telefono,Activo
                        from Usuario where IdUsuario = @id ", parameters, CommandType.Text);
            if(dataReader != null && dataReader.HasRows)
            {
                 while(dataReader.Read()){
                    usuario = new Usuario();
                    usuario.Activo = dataReader["Activo"] == DBNull.Value ? false : Convert.ToBoolean(dataReader["Activo"]);
                    usuario.Apellidos =  dataReader["Apellidos"] == DBNull.Value ? "" : dataReader["Apellidos"].ToString().Trim();
                    usuario.Nombres = dataReader["Nombres"] == DBNull.Value ? "" : dataReader["Nombres"].ToString().Trim();
                    usuario.Email = dataReader["Email"] == DBNull.Value ? "" : dataReader["Email"].ToString().Trim();
                    usuario.FechaNacimiento  =  dataReader["FechaNacimiento"] == DBNull.Value ? new DateTime() :  Convert.ToDateTime(dataReader["FechaNacimiento"]) ;
                    usuario.Telefono = dataReader["Telefono"] == DBNull.Value ? "" : dataReader["Telefono"].ToString().Trim();
                    usuario.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);
                }
            }
           
            return usuario;
        }
        catch (Sy
        stem.Exception)
        {            
            throw;
        }
        finally
        {
            if(dataReader != null){
                if(!dataReader.IsClosed){
                    dataReader.Close();
                }   
                dataReader.Dispose();
            }
             dataReader = null;                        
        }
    }
    */
    
    /// <summary>
    /// Version query para Dapper
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Usuario> GetById(int id)
    {
        String query = @"select top 1 IdUsuario,Nombres,Apellidos,Email,FechaNacimiento,Telefono,Activo
                        from Usuario where IdUsuario = @id ";
        SocialMedia.Core.Entities.Usuario usuario = null;
        try
        {
            using(var connection = _IDapperContext.CreateConnection())
            {
                usuario = await connection.QuerySingleOrDefaultAsync<Usuario>(query, new { id });                
            }
            return usuario;
        }
        catch(Exception)
        {
            throw;
        }  
    }

    /*
    public async Task<IEnumerable<Usuario>> GetAll()
    {        
        SqlDataReader dataReader = null;
        List<SocialMedia.Core.Entities.Usuario> usuarios = null;
        
        try
        {
            usuarios = new List<Usuario>();            

            dataReader = await _databaseRepository.GetDataReader(@"
                        select IdUsuario,Nombres,Apellidos,Email,FechaNacimiento,Telefono,Activo
                        from Usuario ", null, CommandType.Text);
            while(dataReader.Read()){
                Usuario usuario = new Usuario();
                usuario.Activo = dataReader["Activo"] == DBNull.Value ? false : Convert.ToBoolean(dataReader["Activo"]);
                usuario.Apellidos =  dataReader["Apellidos"] == DBNull.Value ? "" : dataReader["Apellidos"].ToString().Trim();
                usuario.Nombres = dataReader["Nombres"] == DBNull.Value ? "" : dataReader["Nombres"].ToString().Trim();
                usuario.Email = dataReader["Email"] == DBNull.Value ? "" : dataReader["Email"].ToString().Trim();
                usuario.FechaNacimiento  =  dataReader["FechaNacimiento"] == DBNull.Value ? new DateTime() :  Convert.ToDateTime(dataReader["FechaNacimiento"]) ;
                usuario.Telefono = dataReader["Telefono"] == DBNull.Value ? "" : dataReader["Telefono"].ToString().Trim();
                usuario.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);
                usuarios.Add(usuario);
            }
            return usuarios;
        }
        catch (System.Exception)
        {            
            throw;
        }
        finally
        {
            if(dataReader != null){
                if(!dataReader.IsClosed){
                    dataReader.Close();
                }   
                dataReader.Dispose();
            }
             dataReader = null;                        
        }
    }
    */
    
    public async Task<IEnumerable<Usuario>> GetAll()
    {
        IEnumerable<Usuario> usuarios = new List<Usuario>();
        var query = @"select IdUsuario,Nombres,Apellidos,Email,FechaNacimiento,Telefono,Activo
                      from Usuario";
        try
        {
            using (var connection = _IDapperContext.CreateConnection())
            {
                usuarios = await connection.QueryAsync<Usuario>(query);
                return usuarios;
            }
        }
        catch(Exception)
        {
            throw;
        }
    }

    /*
    public async Task<int> Insert(Usuario usuario)
    {
        List<SqlParameter> parameters = null;
        int returnValue;

        try
        {
            parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter()
            {
                ParameterName = "@Nombres",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = usuario.Nombres
            });

            parameters.Add(new SqlParameter()
            {
                ParameterName = "@Apellidos",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = usuario.Apellidos
            });

            parameters.Add(new SqlParameter()
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = usuario.Email
            });

            parameters.Add(new SqlParameter()
            {
                ParameterName = "@FechaNacimiento",
                SqlDbType = SqlDbType.DateTime,
                Direction = ParameterDirection.Input,
                Value = usuario.FechaNacimiento
            });

            returnValue = await _databaseRepository.Insert("INSERT INTO [dbo].[Usuario] " +
                                                            " ([Nombres],[Apellidos],[Email],[FechaNacimiento]) VALUES " +
                                                            " (@Nombres,@Apellidos,@Email,@FechaNacimiento)", 
                                                            parameters, CommandType.Text);
        }
        catch (Exception)
        {
            throw;
        }

        return returnValue;
    }
    */

    public async Task<int> Insert(Usuario usuario)
    {
        int id;
        try
        {
            var query = "INSERT INTO [dbo].[Usuario] " +
                        " ([Nombres],[Apellidos],[Email],[FechaNacimiento]) VALUES " +
                        " (@Nombres,@Apellidos,@Email,@FechaNacimiento)";

            var parameters = new DynamicParameters();
            parameters.Add("Nombres", usuario.IdUsuario, DbType.String);
            parameters.Add("Apellidos", usuario.Apellidos, DbType.String);
            parameters.Add("Email", usuario.Email, DbType.String);
            parameters.Add("FechaNacimiento", usuario.FechaNacimiento, DbType.DateTime);  
            using(var connection = _IDapperContext.CreateConnection())
            {
                id = await connection.QuerySingleAsync<int>(query, parameters);                    
            }
        }
        catch(Exception)
        {
            throw;
        }
        return id;
    }

    /*
    public async Task<bool> Update(int id, Usuario usuario)
    {
        List<SqlParameter> parameters = null;
        int count = 0;

        try
        {
                parameters = new List<SqlParameter>()
                {
                    new SqlParameter()
                    {
                        ParameterName = "@IdUsuario",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = usuario.IdUsuario
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@FechaNacimiento",
                        SqlDbType = SqlDbType.DateTime,
                        Direction = ParameterDirection.Input,
                        Value = usuario.FechaNacimiento
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Nombres",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Value = usuario.Nombres
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Apellidos",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Value = usuario.Apellidos
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Email",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Value = usuario.Apellidos
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Telefono",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Value = usuario.Telefono
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Activo",
                        SqlDbType = SqlDbType.Bit,
                        Direction = ParameterDirection.Input,
                        Value = usuario.Activo
                    }
                };

                count = await this._databaseRepository.ExecuteNonQuery(@"UPDATE [dbo].[Usuario]
                                                                                SET [Nombres] = @Nombres
                                                                                    ,[Apellidos] = @Apellidos
                                                                                    ,[Email] = @Email
                                                                                    ,[FechaNacimiento] = @FechaNacimiento
                                                                                    ,[Telefono] = @Telefono
                                                                                    ,[Activo] = @Activo
                                                                                WHERE IdUsuario=@IdUsuario", parameters, CommandType.Text);
                return count > 0;
            }
            catch (Exception)
            {
                throw;
            }
    }
    */

    public async Task<bool> Update(int id, Usuario usuario)
    {
        var query = @"UPDATE [dbo].[Usuario]
                    SET  [Nombres]         = @Nombres
                        ,[Apellidos]       = @Apellidos
                        ,[Email]           = @Email
                        ,[FechaNacimiento] = @FechaNacimiento
                        ,[Telefono]        = @Telefono
                        ,[Activo]          = @Activo
                    WHERE IdUsuario=@IdUsuario";
        try
        {   
            var parameters = new DynamicParameters();
            parameters.Add("IdUsuario", id, DbType.Int32);
            parameters.Add("Nombres", usuario.Nombres, DbType.String);
            parameters.Add("Apellidos", usuario.Apellidos, DbType.String);
            parameters.Add("Email", usuario.Email, DbType.String);
            parameters.Add("FechaNacimiento", usuario.FechaNacimiento, DbType.DateTime);
            parameters.Add("Telefono", usuario.Telefono, DbType.String);
            parameters.Add("Activo", usuario.Activo, DbType.Boolean);
            using (var connection = _IDapperContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
}