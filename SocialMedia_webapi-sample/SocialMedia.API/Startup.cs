using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using SocialMedia.Core.Services;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Infraestructure.Options;
using SocialMedia.Infraestructure.Services;
using SocialMedia.Infraestructure.Repositories;
using SocialMedia.Infraestructure.Filters;
using SocialMedia.Infraestructure.Interfaces;
using SocialMedia.Infraestructure.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SocialMedia.API
{
    
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers(options => 
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options => 
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling     = Newtonsoft.Json.NullValueHandling.Ignore;
            }).ConfigureApiBehaviorOptions(options => 
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            
            //////////////////////////////////////////////////////////////////////////////////////
            // revisar ==> \SocialMedia.Infraestructure\Extensions\ServiceCollectionExtension.cs
                        
            services.AddSwagger();
            /*
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SocialMedia.API", Version = "v1" });
            });
            */
            
            //////////////////////////////////////////////////////////////////////////////////////
            // revisar ==> \SocialMedia.Infraestructure\Extensions\ServiceCollectionExtension.cs
            services.AddOptions();
            /*
            // configuraciones de paginacion - optionsValue pattern
            services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));
            // configuraciones de passworh y valores hash
            services.Configure<PasswordOptions>(Configuration.GetSection("PasswordOptions"));
            */
            //////////////////////////////////////////////////////////////////////////////////////
            // revisar ==> \SocialMedia.Infraestructure\Extensions\ServiceCollectionExtension.cs

            services.AddDatabases(Configuration);
            // cadena de conexion por defecto
            /*
            services.AddTransient<ISocialMediaRepository>(options => 
                                new SocialMediaRepository(Configuration.GetConnectionString("DefaultConnection")));
            */
            //////////////////////////////////////////////////////////////////////////////////////
            // revisar ==> \SocialMedia.Infraestructure\Extensions\ServiceCollectionExtension.cs
            
            services.AddServices();

            /*
            // inyeccion de dependencias IService
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ISecurityService, SecurityService>();
            // inyeccion de dependencias de interfaces del Unit of Work
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IPasswordService, PasswordHasherService>();
            services.AddSingleton<IUriService>( provider => 
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(absoluteUri);
            });
            */
            //////////////////////////////////////////////////////////////////////////////////////

            services.AddMvc(options => 
            { 
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(options => {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });

            // middleware autenticacion
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer   = Configuration["Authentication:Issuer"],
                    //ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                                        System.Text.Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                               
            }
           
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>{
                endpoints.MapControllers();
            });
        }
    }
}
