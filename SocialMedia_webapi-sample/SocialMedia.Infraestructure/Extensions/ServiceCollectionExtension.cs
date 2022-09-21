using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infraestructure.Repositories;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Infraestructure.Options;
using SocialMedia.Core.Services;
using Microsoft.OpenApi.Models;
using SocialMedia.Infraestructure.Interfaces;
using SocialMedia.Infraestructure.Services;

namespace SocialMedia.Infraestructure.Extensions
{

    public static class ServiceCollectionExtension
    {
        
        public static void AddDatabases(this IServiceCollection services, 
                                        IConfiguration configuration)
        {
            services.AddTransient<ISocialMediaRepository>(options => 
                                new SocialMediaRepository(configuration.GetConnectionString("DefaultConnection")));
        }

         public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PaginationOptions>(options => configuration.GetSection("Pagination").Bind(options));
            services.Configure<PasswordOptions>(options => configuration.GetSection("PasswordOptions").Bind(options));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ISecurityService, SecurityService>();
            //services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IPasswordService, PasswordHasherService>();
            services.AddSingleton<IUriService>(provider =>
            {
                var accesor = provider.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(absoluteUri);
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1" });                
            });

            return services;
        }
    }
}


