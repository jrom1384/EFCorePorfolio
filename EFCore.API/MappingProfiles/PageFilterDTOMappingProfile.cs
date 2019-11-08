using AutoMapper;
using EFCore.API.Models;
using EFCore.Common;
using EFCore.DTO;

namespace EFCore.API.MappingProfiles
{
    public class PageFilterDTOMappingProfile : Profile
    {
        public PageFilterDTOMappingProfile()
        {
            CreateMap<SearchRequest, PageFilterDTO>()
                .ForMember(dest => dest.SearchString, opt => opt.MapFrom(src => src.Search))
                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder.ToSortOrder()))
                .ForMember(dest => dest.CurrentPageIndex, opt => opt.MapFrom(src => src.PageIndex));
        }
    }
}
