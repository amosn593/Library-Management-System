using AutoMapper;
using DOMAIN.Models;
using DTO.Models;

namespace DTO.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Form, FormDto>();
        }
    }
}
