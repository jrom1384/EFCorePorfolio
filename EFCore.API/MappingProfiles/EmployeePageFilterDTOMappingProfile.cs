using AutoMapper;
using EFCore.API.Models;
using EFCore.Common;
using EFCore.DTO;

namespace EFCore.API.MappingProfiles
{
    public class EmployeePageFilterDTOMappingProfile : Profile
    {
        public EmployeePageFilterDTOMappingProfile()
        {
            CreateMap<EmployeeSearchRequest, EmployeePageFilterDTO>()
                .ForMember(dest => dest.SearchString, opt => opt.MapFrom(src => src.Search))
                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder.ToSortOrder()))
                .ForMember(dest => dest.CurrentPageIndex, opt => opt.MapFrom(src => src.PageIndex))

                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToNullableGenderEnum()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive.ToNullableBool()));
        }
    }
}
