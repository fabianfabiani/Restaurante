using AutoMapper;
using Restaurante.Dto;
using Restaurante.Entities;

namespace Restaurante.Mappers
{
    public class UsuarioMapper : Profile
    {
        public UsuarioMapper()
        {
            CreateMap<UsuarioRequestCreateDto, Usuario>()
          .ForMember(dest => dest.RolId, opt => opt.MapFrom(src => src.IdRol))
          .ForMember(dest => dest.SectorId, opt => opt.MapFrom(src => src.IdSector));


            CreateMap<Usuario, UsuarioResponseDto>()
                  .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.Descripcion))
                  .ForMember(dest => dest.Sector, opt => opt.MapFrom(src => src.Sector.descripcion));
        }
    }
}
