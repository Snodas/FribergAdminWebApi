using AutoMapper;
using FribergAdminWebApi.Data.Dto;
using FribergAdminWebApi.Models;

namespace FribergAdminWebApi.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Employee

            CreateMap<EmployeeCreateDto, Employee>();

            CreateMap<Employee, EmployeeProfileDto>();

            CreateMap<Employee, EmployeeUserDto>();

            CreateMap<EmployeeProfileDto, Employee>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.HourlyRate, opt => opt.Ignore())
                .ForMember(dest => dest.SocialSecurityNumber, opt => opt.Ignore())
                .ForMember(dest => dest.ApiUserId, opt => opt.Ignore())
                .ForMember(dest => dest.ApiUser, opt => opt.Ignore())
                .ForMember(dest => dest.WorkEntries, opt => opt.Ignore())
                .ForMember(dest => dest.Salaries, opt => opt.Ignore());


            //Salary

            CreateMap<Salary, SalaryDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src =>
                $"{src.Employee.FirstName} {src.Employee.LastName}"));

            //WorkEntry
        }

    }
}
