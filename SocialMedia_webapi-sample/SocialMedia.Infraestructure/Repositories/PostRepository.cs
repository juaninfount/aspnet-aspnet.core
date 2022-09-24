using System;
using System.Collections.Generic;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System.Threading.Tasks;
//using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace SocialMedia.Infraestructure.Repositories
{
    public class PostRepository: IPostRepository
    {
        ISocialMediaRepository _databaseRepository = null;
        IDapperContext _IDapperContext = null;

        public PostRepository(ISocialMediaRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;                                
        }

        public PostRepository(IDapperContext IDapperContext)
        {
            _IDapperContext = IDapperContext;
        }

        /// <summary>
        /// Version query para ADO.NET
        /// </summary>
        /// <returns></returns>
        /*
        public async Task<IEnumerable<Publicacion>> GetAll(PostQueryFilter filters)
        {
            string sql = "";
           
            try
            {
              sql = "select IdPublicacion,IdUsuario,Fecha,Descripcion,Imagen from Publicacion where 1=1 ";

            if(filters != null && filters.Date.HasValue && filters.Date.Value != new DateTime())
            {
                sql += $" and Fecha = cast('{filters.Date.Value.ToShortDateString()}' as Date) ";
            }

            if(filters != null && !string.IsNullOrEmpty(filters.Description) && !string.IsNullOrWhiteSpace(filters.Description))
            {
                sql += $" and Descripcion like '%{filters.Description}%' ";
            }

            if(filters != null && filters.UserId != null &&  filters.UserId > 0)
            {
                sql += $" and IdUsuario = {filters.UserId} ";
            }

            var resultset = _databaseRepository.GetDataTable(sql, null, System.Data.CommandType.Text);                                   
            IList<Publicacion> posts = new List<Publicacion>();
            
            if (resultset != null && resultset.Rows.Count > 0) 
            {
                foreach (DataRow dr in resultset.Rows) 
                {
                    var newpost = new Publicacion()
                    {
                        IdPublicacion = Convert.ToInt32(dr["IdPublicacion"]),
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        Fecha = Convert.ToDateTime(dr["Fecha"]),
                        Descripcion = Convert.ToString(dr["Descripcion"]).Trim(),
                        Imagen = Convert.ToString(dr["Imagen"]).Trim()
                    };
                    posts.Add(newpost);                                                                                                             
                }
            }

            await Task.Delay(0);
            return posts;    
            }
            catch (System.Exception)
            {                
                throw;
            }
        }
        */
        
        /// <summary>
        /// Version query para Dapper
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Publicacion>> GetAll(PostQueryFilter filters)
        {
            string sql = "";
            IEnumerable<Publicacion> posts = new List<Publicacion>();
           
            try
            {
              sql = "select IdPublicacion,IdUsuario,Fecha,Descripcion,Imagen from Publicacion where 1=1 ";

                if(filters != null && filters.Date.HasValue 
                    && filters.Date.Value != new DateTime()){
                    sql += $" and Fecha = cast('{filters.Date.Value.ToShortDateString()}' as Date) ";
                }

                if(filters != null && !string.IsNullOrEmpty(filters.Description) 
                    && !string.IsNullOrWhiteSpace(filters.Description)){
                    sql += $" and Descripcion like '%{filters.Description}%' ";
                }

                if(filters != null && filters.UserId != null &&  filters.UserId > 0){
                    sql += $" and IdUsuario = {filters.UserId} ";
                }

                using (var connection = _IDapperContext.CreateConnection())
                {
                    posts = await connection.QueryAsync<Publicacion>(sql);
                    return posts;
                }
            }
            catch (System.Exception)
            {                
                throw;
            }
        }

        /*
        /// <summary>
        /// Version para ADO.NET
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Publicacion> GetById(int id) 
        {
            Publicacion publicacion = new Publicacion();
            List<SqlParameter> parameters = null;
            SqlParameter param = null;            
            SqlDataReader dataReader = null;

            try
            {
                parameters = new List<SqlParameter>();

                param = new SqlParameter("@id", id) { SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input };
                parameters.Add(param);

                dataReader = await _databaseRepository.GetDataReader(
                                                           "select IdPublicacion,IdUsuario,Fecha,Descripcion,Imagen " +
                                                           "from Publicacion where IdPublicacion=@id" +
                                                           "", parameters, System.Data.CommandType.Text);                                                
                while (dataReader.Read())
                {
                    publicacion = new Publicacion()
                    {
                        IdPublicacion = Convert.ToInt32(dataReader["IdPublicacion"]),
                        IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]),
                        Fecha = Convert.ToDateTime(dataReader["Fecha"]),
                        Descripcion = Convert.ToString(dataReader["Descripcion"]).Trim(),
                        Imagen = Convert.ToString(dataReader["Imagen"]).Trim()
                    };
                }                                    
            }
            catch (Exception)
            {
                throw;
            }
            finally {
                if (dataReader != null) 
                {                    
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            
            return publicacion;
        }
        */

         /// <summary>
        /// Version query para Dapper
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Publicacion> GetById(int id) 
        {
            Publicacion publicacion = new Publicacion();
            
            try
            {
                var query = "select IdPublicacion,IdUsuario,Fecha,Descripcion,Imagen " +
                             "from Publicacion where IdPublicacion=@id";
                using (var connection = _IDapperContext.CreateConnection())
                {
                    publicacion = await connection.QuerySingleOrDefaultAsync<Publicacion>(query, new { id });
                    return publicacion;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
        /// <summary>
        /// Version para ADO.NET
        /// </summary>
        /// <param name="publicacion"></param>
        /// <returns></returns>
        public async Task<int> Insert(Publicacion publicacion)
        {
            List<SqlParameter> parameters = null;
            int returnValue;

            try
            {
                parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter()
                {
                    ParameterName = "@IdUsuario",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = publicacion.IdUsuario
                });

                parameters.Add(new SqlParameter()
                {
                    ParameterName = "@Fecha",
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    Value = publicacion.Fecha
                });

                parameters.Add(new SqlParameter()
                {
                    ParameterName = "@Descripcion",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Value = publicacion.Descripcion
                });

                parameters.Add(new SqlParameter()
                {
                    ParameterName = "@Imagen",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Value = publicacion.Imagen
                });

                returnValue = await _databaseRepository.Insert("INSERT INTO [dbo].[Publicacion] " +
                                                                " ([IdUsuario],[Fecha],[Descripcion],[Imagen]) VALUES " +
                                                                " (@IdUsuario,@Fecha,@Descripcion,@Imagen)", 
                                                                parameters, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }

            return returnValue;
        }
        */

        /// <summary>
        /// Version para Dapper
        /// </summary>
        /// <param name="publicacion"></param>
        /// <returns></returns>
        public async Task<int> Insert(Publicacion publicacion)
        {
            int id;
            try
            {
                var query = "INSERT INTO [dbo].[Publicacion] " +
                             " ([IdUsuario],[Fecha],[Descripcion],[Imagen]) VALUES " +
                             " (@IdUsuario,@Fecha,@Descripcion,@Imagen)";

                var parameters = new DynamicParameters();
                parameters.Add("IdUsuario", publicacion.IdUsuario, DbType.Int32);
                parameters.Add("Fecha", publicacion.Fecha, DbType.DateTime );
                parameters.Add("Descripcion", publicacion.Descripcion, DbType.String );
                parameters.Add("Imagen", publicacion.Imagen, DbType.String );
                
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
        /// <summary>
        /// Version para ADO.NET
        /// </summary>
        /// <param name="id"></param>
        /// <param name="publicacion"></param>
        /// <returns></returns>
        public async void Update(int id, Publicacion publicacion) 
        {
            List<SqlParameter> parameters = null;
            int count = 0;

            try
            {
                parameters = new List<SqlParameter>();
                
                parameters.Clear();
                parameters = new List<SqlParameter>
                {
                    new SqlParameter()
                    {
                        ParameterName = "@IdUsuario",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = publicacion.IdUsuario
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Fecha",
                        SqlDbType = SqlDbType.DateTime,
                        Direction = ParameterDirection.Input,
                        Value = publicacion.Fecha
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Descripcion",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Value = publicacion.Descripcion
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@Imagen",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Value = publicacion.Imagen
                    },
                    new SqlParameter()
                    {
                        ParameterName = "@IdPublicacion",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = id
                    }
                };

                count = await this._databaseRepository.ExecuteNonQuery(@"update Publicacion
                                                                        set Fecha = @Fecha,
                                                                        Descripcion = @Descripcion,
                                                                        Imagen = @Imagen,
                                                                        IdUsuario = @IdUsuario
                                                                        where IdPublicacion = @IdPublicacion", parameters, CommandType.Text);                
            }
            catch (Exception)
            {
                throw;
            }
        }
        */

        /// <summary>
        /// Version para Dapper
        /// </summary>
        /// <param name="id"></param>
        /// <param name="publicacion"></param>
        /// <returns></returns>
        public async Task Update(int id, Publicacion publicacion)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("IdUsuario", id, DbType.Int32);
                parameters.Add("Fecha", publicacion.Fecha,DbType.DateTime);
                parameters.Add("Descripcion", publicacion.Descripcion, DbType.String);
                parameters.Add("Imagen", publicacion.Imagen, DbType.String);
                parameters.Add("IdPublicacion", publicacion.IdPublicacion, DbType.Int32);

                var query = @"update Publicacion
                                set Fecha = @Fecha,
                                Descripcion = @Descripcion,
                                Imagen = @Imagen,
                                IdUsuario = @IdUsuario
                                where IdPublicacion = @IdPublicacion";

                using (var connection = _IDapperContext.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>        
        /// <returns></returns>
        public async Task<bool> Delete(int id)
        {
            List<SqlParameter> parameters = null;
            int count = 0;

            try
            {
                parameters = new List<SqlParameter>();

                parameters.Clear();
                parameters = new List<SqlParameter>
                {                   
                    new SqlParameter()
                    {
                        ParameterName = "@IdPublicacion",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = id
                    }
                };

                count = await this._databaseRepository.ExecuteNonQuery(@"delete from Publicacion where IdPublicacion= @IdPublicacion", parameters, CommandType.Text);
                return count > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }*/
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>        
        /// <returns></returns>
        public async Task Delete(int id)
        {
            try
            {
                var query = @"delete from Publicacion where IdPublicacion= @id";
                var parameters = new DynamicParameters();
                parameters.Add("id", id, DbType.Int32);

                using (var connection = _IDapperContext.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
        /// <summary>
        /// Version para ADO.NET
        /// </summary>
        /// <param name="id"></param>        
        /// <returns></returns>
        public async Task<IEnumerable<Publicacion>> GetPostsByUser(int id)
        {
            List<Publicacion>  publicaciones = null;
            List<SqlParameter> parameters = null;
            SqlParameter param = null;
            SqlDataReader dataReader = null;

            try
            {
                parameters    = new List<SqlParameter>();
                publicaciones = new List<Publicacion>();

                param = new SqlParameter("@id", id)
                {   SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input 
                };

                parameters.Add(param);
                dataReader = await _databaseRepository.GetDataReader(@"select IdPublicacion,IdUsuario,Fecha,
                                                                    Descripcion,Imagen
                                                                    from Publicacion where IdUsuario=@id ",
                                                                    parameters, 
                                                                    System.Data.CommandType.Text);
                
                if(dataReader != null && dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        var publicacion = new Publicacion()
                        {
                            IdPublicacion = Convert.ToInt32(dataReader["IdPublicacion"]),
                            IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]),
                            Fecha = Convert.ToDateTime(dataReader["Fecha"]),
                            Descripcion = Convert.ToString(dataReader["Descripcion"]).Trim(),
                            Imagen = Convert.ToString(dataReader["Imagen"]).Trim()
                        };
                        publicaciones.Add(publicacion);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally {
                if (dataReader != null) 
                {                    
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            
            return publicaciones;
        }
        */

        /// <summary>
        /// Version para ADO.NET
        /// </summary>
        /// <param name="id"></param>        
        /// <returns></returns>
        public async Task<IEnumerable<Publicacion>> GetPostsByUser(int id)
        {
            IEnumerable<Publicacion> posts = new List<Publicacion>();

            try
            {                
                var query = @"select IdPublicacion,IdUsuario,Fecha,
                              Descripcion,Imagen
                              from Publicacion where IdUsuario=@id ";
                
                using (var connection = _IDapperContext.CreateConnection())
                {                    
                    posts = await connection.QueryAsync<Publicacion>(query, new {id});
                }
            }
            catch (Exception)
            {
                throw;
            }

            return posts;
        }
        
    }
}
