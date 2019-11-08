using AutoMapper;
using EFCore.API.Models;
using EFCore.Common;
using EFCore.DTO;
using System;

namespace EFCore.API.MappingProfiles
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<NewEmployeeRequest, EmployeeDTO>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => Convert.ToDateTime(src.DateOfBirth)))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToGenderEnum()));

            CreateMap<EmployeeRequest, EmployeeDTO>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => Convert.ToDateTime(src.DateOfBirth)))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToGenderEnum()));

            CreateMap<EmployeeDTO, EmployeeResponse>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("MM/dd/yyyy")))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));
        }
    }
}
