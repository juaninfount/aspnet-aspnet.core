using SocialMedia.Core.Entities;
using SocialMedia.Core.Dtos;
using AutoMapper;

namespace SocialMedia.Infraestructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<PublicacionDto, Publicacion>();
            CreateMap<Publicacion,PublicacionDto>();

            //CreateMap<SecurityDto, Security>().ReverseMap();
            CreateMap<SecurityDto, Security>();
            CreateMap<Security,SecurityDto>();
        }

    }
}
