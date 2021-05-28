using API.Models;
using AutoMapper;

namespace API.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, Employee>();
        }
    }
}