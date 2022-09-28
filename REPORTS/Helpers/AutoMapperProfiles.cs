using AutoMapper;
using DOMAIN.Models;
using REPORTS.Dtos;

namespace REPORTS.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Form, FormDto>().ReverseMap();
        }
    }
}
