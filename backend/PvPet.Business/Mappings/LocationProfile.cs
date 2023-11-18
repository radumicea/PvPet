using AutoMapper;
using NetTopologySuite.Geometries;
using PvPet.Business.DTOs;

namespace PvPet.Business.Mappings;

public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<Point, LocationDto>()
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.X))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Y))
            .ForAllMembers(i => i.ExplicitExpansion());
        CreateMap<LocationDto, Point>()
            .ConstructUsing(src => new Point(src.Longitude, src.Latitude))
            // Set EPSG:4326 (WGS 84)
            .AfterMap((src, dest) => dest.SRID = 4326);
    }
}
