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
        }

    }
}
