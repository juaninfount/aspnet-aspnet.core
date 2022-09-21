using System;
using System.Collections.Generic;
using System.Linq;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace SocialMedia.Infraestructure.Repositories
{
    public class PostRepository: IPostRepository
    {
        ISocialMediaRepository _databaseRepository = null;

        public PostRepository(ISocialMediaRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Publicacion>> GetAll(PostQueryFilter filters)
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

            /*
            var posts = Enumerable.Range(1, 10).Select(x => new Post()
            {
                PostId = x,
                Date = DateTime.Now,
                Description = " Description " + x,
                Image = $"https://misapis.com/{x}",
                UserId = x * 2
            });*/

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

        /// <summary>
        /// 
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

        /// <summary>
        /// 
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="publicacion"></param>
        /// <returns></returns>
        public async Task<bool> Update(int id, Publicacion publicacion) 
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
                return count > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

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
        }
        
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
        
    }
}
